using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Idle,
        Attack,
        Chase,
    }

    public EnemyState curEnemyState;

    private EnemyHealth health;

    private PlayerStateMachine player;
    
    //Patrolling between points
    public Transform[] patrolPoints;
    private int curPatrolPoint;

    //Attacking Player
    public EnemySword sword;
    private bool isAttacking = false;
    public float attackTime = .5f;
    private float origAttackTime;
    public float attackDelay;
    private float origDelay;

    //For chasing Player
    public float lookRadius = 5f;
    public float stopRadius = 1f;
    Transform target;
    NavMeshAgent agent;
    private float origAgentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        player = FindObjectOfType<PlayerStateMachine>();
        transform.position = patrolPoints[0].position;
        origDelay = attackDelay;
        origAttackTime = attackTime;
        agent = GetComponent<NavMeshAgent>();
        origAgentSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        

        switch (curEnemyState)
        {
            case EnemyState.Idle:
                EnemyIdle();
                break;
            case EnemyState.Patrol:
                EnemyPatrol();
                break;
            case EnemyState.Attack:
                EnemyAttack();
                break;
            case EnemyState.Chase:
                EnemyChase();
                break;
        }
    }

    public void EnemyIdle()
    {
        //if needing to add an idle state
    }

    public void EnemyPatrol()
    {
        if (health.isDead)
            return;
        //enemy looks in the direction it is going
        FaceTarget(patrolPoints[curPatrolPoint].transform);
        agent.SetDestination(patrolPoints[curPatrolPoint].position);

        //sets the distance between the enemy and the player
        float distance = Vector3.Distance(player.transform.position, transform.position);


        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            //sets the next patrol point
            curPatrolPoint = (curPatrolPoint + 1) % patrolPoints.Length;
        }

        if (distance <= lookRadius) //checks to see if the player is within the enemy's look radius
        {
            //agent.SetDestination(player.transform.position);
            curEnemyState = EnemyState.Attack;
            //isAttacking = true;
        }
    }

    public void EnemyChase()
    {
        if (health.isDead)
        {
            return;
        }

        FaceTarget(player.transform);
        agent.SetDestination(player.transform.position);

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance <= lookRadius)
        {
            curEnemyState = EnemyState.Attack;
        }
    }

    public void EnemyAttack()
    {
        if (health.isDead)
        {
            sword.Swing(false);
            return;
        }
            

        FaceTarget(player.transform);

        float distance = Vector3.Distance(player.transform.position, transform.position); //gets distance between player and enemy

        agent.SetDestination(player.transform.position);
        //sword.Swing(true);
        //isAttacking = true;
        
        if (isAttacking)
        {
            sword.Swing(true);
            attackTime -= Time.deltaTime;
            attackDelay = origDelay;
        }
        else if (!isAttacking)
        {
            attackDelay -= Time.deltaTime;
            sword.Swing(false);
            attackTime = origAttackTime;
        }

        if(attackTime <= 0)
        {
            isAttacking = false;
        }

        if(attackDelay <= 0)
        {
            isAttacking = true;
        }

        if (distance > lookRadius)
        {
            sword.Swing(false);
            curEnemyState = EnemyState.Chase;
            //curEnemyState = EnemyState.Patrol;
        }
        else if(distance <= stopRadius)
        {
            agent.speed = 0;
            //agent.SetDestination(this.transform.position);
        }
        else
        {
            agent.speed = origAgentSpeed;
        }
    }

    void FaceTarget(Transform target) //Function that sets the enemy to look in the direction they are moving towards
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos() //Shows the radius of the trigger area in the scene view
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }
}

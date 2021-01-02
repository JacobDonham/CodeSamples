using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootPatrol : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Shoot,
        Chase,
    }

    public EnemyState curState;

    private NavMeshAgent agent;
    private EnemyHealth enemyHealth;
    private PlayerStateMachine player;
    public Transform[] patrolPoints;
    private int curPoint;

    public float radius = 5f;
    public float chaseRadius = 10f;
    private float curDistance;

    public EnemyShooting enemyShooting;
    //public EnemyProjectile projectile;
    //public GameObject firePoint;
    public float timeBetweenShots;
    public float origTBS;

    public Renderer eyes;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = FindObjectOfType<PlayerStateMachine>();
        enemyShooting = GetComponent<EnemyShooting>();
        origTBS = timeBetweenShots;
        transform.position = patrolPoints[0].position;
        curState = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.isDead)
        {
            return;
        }

        curDistance = Vector3.Distance(transform.position, player.transform.position);

        switch (curState)
        {
            case EnemyState.Patrol:
                EnemyPatrol();
                break;
            case EnemyState.Shoot:
                EnemyAttack();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
        }
    }

    public void EnemyPatrol()
    {
        agent.SetDestination(patrolPoints[curPoint].position);
        FaceTarget(patrolPoints[curPoint].transform);

        if(!agent.pathPending && agent.remainingDistance < .05f)
        {
            curPoint = (curPoint + 1) % patrolPoints.Length;
        }

        if (curDistance <= radius)
        {
            curState = EnemyState.Shoot;
        }
    }

    public void EnemyAttack()
    {
        FaceTarget(player.transform);

        agent.SetDestination(this.transform.position);
        

        
        if (timeBetweenShots <= 0 && curDistance <= radius)
        {

            enemyShooting.ShootGun();
            timeBetweenShots = origTBS;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
            eyes.material.color = Color.Lerp(Color.red, Color.yellow, timeBetweenShots);
            //GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, timeBetweenShots);
        }
        

        if (curDistance > radius)
        {
            curState = EnemyState.Chase;
        }
    }

    public void ChasePlayer()
    {
        FaceTarget(player.transform);
        //agent.SetDestination(this.transform.position);

        agent.SetDestination(player.transform.position);

        if (curDistance <= radius)
        {
            timeBetweenShots = origTBS;
            eyes.material.color = Color.Lerp(Color.red, Color.yellow, timeBetweenShots);
            curState = EnemyState.Shoot;
        }
        else if (curDistance > chaseRadius)
        {
            //curState = EnemyState.Patrol;
        }
    }

    void FaceTarget(Transform target) //Function that sets the enemy to look in the direction they are moving towards
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Move,
        Shoot,
    }

    public EnemyState curEnemyState;

    private PlayerStateMachine player;
    private NavMeshAgent agent;
    public Renderer body;
    public EnemyShooting enemyShooting;
    public Transform[] patrolPoints;
    private int curPoint;
    //public EnemyProjectile projectile;
    //public GameObject firePoint;
    public float radius = 5f;
    private float curDistance;
    public float timeBetweenShots;
    public float origTBS;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStateMachine>();
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Renderer>();
        transform.position = patrolPoints[0].position;
        origTBS = timeBetweenShots;
        curEnemyState = EnemyState.Move;
    }

    // Update is called once per frame
    void Update()
    {
        curDistance = Vector3.Distance(transform.position, player.transform.position);
        FaceTarget(player.transform);

        switch (curEnemyState)
        {
            case EnemyState.Move:
                MoveEnemy();
                break;
            case EnemyState.Shoot:
                ShootEnemy();
                break;
        }
    }

    public void MoveEnemy()
    {
        //Debug.Log("Enemy is moving");
        agent.SetDestination(patrolPoints[curPoint].position);

        if (!agent.pathPending && agent.remainingDistance < .05f)
        {
            curPoint = (curPoint + 1) % patrolPoints.Length;
        }

        if(curDistance <= radius)
        {
            curEnemyState = EnemyState.Shoot;
        }
    }

    public void ShootEnemy()
    {
        Debug.Log("Enemy is shooting");

        if (timeBetweenShots <= 0 && curDistance <= radius)
        {
            //Instantiate(projectile, firePoint.transform.position, transform.rotation);
            enemyShooting.ShootGun();
            timeBetweenShots = origTBS;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
            body.material.color = Color.Lerp(Color.red, Color.yellow, timeBetweenShots);
        }

        if(curDistance > radius)
        {
            curEnemyState = EnemyState.Move;
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
    }
    
}

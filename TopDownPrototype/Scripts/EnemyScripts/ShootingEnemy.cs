using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    private float origAgentSpeed;
    private EnemyHealth enemyHealth;
    private PlayerStateMachine player;
    private float curDistance;
    public float stopDistance = 5f;
    public float retreatDistance = 2.5f;

    private EnemyShooting enemyShoots;
    public Renderer enemyEyes;
    
    //public EnemyProjectile projectile;
    //public GameObject firePoint;
    public float timeBetweenShots;
    public float origTBS;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = FindObjectOfType<PlayerStateMachine>();
        enemyShoots = GetComponent<EnemyShooting>();
        origTBS = timeBetweenShots;
        origAgentSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.isDead)
        {
            return;
        }

        curDistance = Vector3.Distance(transform.position, player.transform.position);

        FaceTarget(player.transform);

        if (curDistance > stopDistance)
        {
            agent.SetDestination(player.transform.position);
            agent.speed = origAgentSpeed;
            Debug.Log("Enemy is chasing the player");
        }
        else if (curDistance < stopDistance && curDistance > retreatDistance)
        {
            //agent.SetDestination(this.transform.position);
            agent.speed = 0f;
            Debug.Log("Enemy is staying still");
        }
        else if (curDistance < retreatDistance)
        {
            agent.SetDestination(player.transform.position);
            agent.speed = -5 * origAgentSpeed;
            Debug.Log("Enemy is retreating");
        }
            
        

        
        if (timeBetweenShots <= 0 && curDistance < stopDistance)
        {
            //Instantiate(projectile, firePoint.transform.position, transform.rotation);
            enemyShoots.ShootGun();
            timeBetweenShots = origTBS;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
            enemyEyes.material.color = Color.Lerp(Color.red, Color.yellow, timeBetweenShots);
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
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
    }
}

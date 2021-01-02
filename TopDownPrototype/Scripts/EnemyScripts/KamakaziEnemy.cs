using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamakaziEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    private EnemyHealth enemyHealth;
    private PlayerStateMachine player;
    public GameObject triggerRadius;
    private float curDistance;
    public float radius;
    public float lookRadius;
    public float explosiveDelay;
    private bool inRange = false;
    public float explosiveDamage = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = FindObjectOfType<PlayerStateMachine>();
        //triggerRadius.SetActive(false);
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

        if(curDistance > lookRadius)
        {
            agent.SetDestination(this.transform.position);
        }
        else if(curDistance <= lookRadius)
        {
            agent.SetDestination(player.transform.position);
        }

        if (curDistance <= radius)
        {
            inRange = true;
        }

        if (inRange)
        {
            explosiveDelay -= Time.deltaTime;
            triggerRadius.SetActive(true);
            //triggerRadius.transform.localScale *= Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, explosiveDelay);
        }

        if (explosiveDelay <= 0)
        {
            if (curDistance <= radius)
            {
                player.GetComponent<PlayerHealth>().Damage(explosiveDamage);
                Destroy(this.gameObject);
                //Debug.Log("Enemy exploded");
            }
            else
            {
                Destroy(this.gameObject);
            }
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
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

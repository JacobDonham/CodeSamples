using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [SerializeField]
    float damage = 1f;
    //private HealthComponent healthComponent;
    //private PlayerHealth playerHealth;
    //private EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        //playerHealth = FindObjectOfType<PlayerHealth>();
        //enemyHealth = FindObjectOfType<EnemyHealth>();
        //healthComponent = FindObjectOfType<HealthComponent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (playerHealth == null)
           // return;

        if(other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().Damage(damage);
            //playerHealth.Damage(damage);
        }

        if(other.tag == "Enemy")
        {
            //other.GetComponent<EnemyHealth>().TakeDamage(damage);
            //enemyHealth.TakeDamage(damage);
        }
        

        /*if (healthComponent == null)
            return;
        healthComponent.DamageTaken(damage);*/
    }
}

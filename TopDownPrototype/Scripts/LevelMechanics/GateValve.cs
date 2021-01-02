using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateValve : MonoBehaviour
{
    public EnemyHealth[] enemies;
    //public Spawner enemySpawner;
    public int spawnedAmount;
    public int killAmount = 3;

    public GameObject doorClose;
    public GameObject doorDestroy;

    private void Start()
    {
        enemies = FindObjectsOfType<EnemyHealth>();

        if (doorClose != null)
        {
            doorClose.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (doorClose != null)
            {
                doorClose.SetActive(true);
            }
            
            spawnedAmount = enemies.Length;
            this.gameObject.GetComponent<Collider>().enabled = false;
        }

        //Debug.Log(other.gameObject);
    }

    private void Update()
    {
        //Debug.Log(enemies.LongLength);
        //Debug.Log(spawnedAmount - killAmount);

        if (enemies.LongLength <= spawnedAmount - killAmount)
        {
            //open doors and unlock collectables
            if(doorClose != null)
            {
                doorClose.SetActive(false);
            }
            
            doorDestroy.SetActive(false);
            //Debug.Log("Player killed all enemies in this area");
        }
        else if(enemies.LongLength > spawnedAmount - killAmount)
        {
            enemies = FindObjectsOfType<EnemyHealth>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //enemies = FindObjectsOfType<EnemyHealth>();
    }
}

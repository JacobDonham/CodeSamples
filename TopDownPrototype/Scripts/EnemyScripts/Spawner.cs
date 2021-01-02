using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;
    public int enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEnemies();
        }
        
    }

    public void SpawnEnemies()
    {
        if (enemies >= spawnPoints.Length)
        {
            GetComponent<Collider>().enabled = false;
            return;
        }

        for(enemies = 0; enemies < spawnPoints.Length; enemies++)
        {
            Instantiate(enemy, spawnPoints[enemies].position, spawnPoints[enemies].rotation);
        }
    }
}

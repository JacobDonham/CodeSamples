using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public EnemyHealth[] enemies;
    public GameObject key;
    public GameObject blockingDoor;
    public GameObject closingDoor;
    public GameObject sectionToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (key.gameObject != null)
        {
            key.SetActive(false);
        }
        */
        key.SetActive(false);

        if (closingDoor != null)
        {
            closingDoor.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<EnemyHealth>();

        if (enemies.Length < 1)
        {
            if(blockingDoor != null)
            {
                blockingDoor.SetActive(false);

                if (closingDoor != null)
                {
                    closingDoor.SetActive(false);
                }
            }

            if(key.gameObject != null)
            {
                key.SetActive(true);
                //Debug.Log("Key is running");
            }
            //Debug.Log("The length of the enemies array is " + enemies.Length);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sectionToSpawn != null && other.CompareTag("Player"))
        {
            sectionToSpawn.SetActive(true);
        }

        if (enemies.Length >= 1 && other.CompareTag("Player"))
        {
            if (closingDoor != null)
            {
                closingDoor.SetActive(true);
                key.SetActive(false);
            }
        }
    }
}

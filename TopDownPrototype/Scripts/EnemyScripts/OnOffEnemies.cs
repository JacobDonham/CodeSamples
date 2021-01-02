using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffEnemies : MonoBehaviour
{
    public GameObject sectionToSpawn;
    public GameObject GateTrigger;

    // Start is called before the first frame update
    void Start()
    {
        sectionToSpawn.SetActive(false);
        if(GateTrigger != null)
        {
            GateTrigger.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sectionToSpawn.SetActive(true);
            if(GateTrigger != null)
            {
                GateTrigger.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && GateTrigger == null)
        {
            sectionToSpawn.SetActive(false);
        }
        else if (other.CompareTag("Player") && !GateTrigger.activeInHierarchy)
        {
            sectionToSpawn.SetActive(false);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokingDeath : MonoBehaviour
{
    public GameObject spikeBlock;
    public float resetTime = 5f;

    private void Awake()
    {
        spikeBlock.SetActive(false);
    }

    private void GrowDeath()
    {
        spikeBlock.SetActive(true);
        StartCoroutine(ResetSpikes());
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Bullet") || other.CompareTag("IceBullet") || other.CompareTag("Enemy"))
        {
            return;
        }*/
        if (other.CompareTag("Player"))
        {
            if (spikeBlock.activeInHierarchy != true)
            {
                StartCoroutine(SetSpikes());
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        if (other.CompareTag("Bullet") || other.CompareTag("IceBullet") || other.CompareTag("Enemy"))
        {
            return;
        }*/

        if (other.CompareTag("Player"))
        {
            GrowDeath();
        }
        
        /*
        if (spikeBlock.activeInHierarchy == true)
        {
            
        }*/
        
    }

    public IEnumerator SetSpikes()
    {
        yield return new WaitForSeconds(resetTime);

        spikeBlock.SetActive(true);
        StartCoroutine(ResetSpikes());
    }

    public IEnumerator ResetSpikes()
    {
        yield return new WaitForSeconds(resetTime);

        spikeBlock.SetActive(false);
        
    }
}

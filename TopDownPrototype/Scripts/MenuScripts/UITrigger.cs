using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrigger : MonoBehaviour
{
    public GameObject tutorialCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            tutorialCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            tutorialCanvas.SetActive(false);
    }
}

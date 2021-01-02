using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPuzzleDoor : MonoBehaviour
{
    public int amountToHit = 1;
    public int amountOfTargets;
    public GameObject doorToOpen;
    public GameObject key;

    private void Start()
    {
        if(key != null)
        {
            key.SetActive(false);
        }
        
    }

    public void OpenThisDoor()
    {
        if(amountOfTargets >= amountToHit)
        {
            if(doorToOpen != null)
            {
                doorToOpen.SetActive(false);
            }
            else if(key != null)
            {
                key.SetActive(true);
            }
        }
    }
}

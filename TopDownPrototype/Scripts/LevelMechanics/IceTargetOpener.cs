using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTargetOpener : MonoBehaviour
{
    public int targetsToHit = 3;
    public int targetsShot;
    public GameObject[] iceFloor;
    

    public void OpenIceDoor()
    {
        iceFloor[targetsShot - 1].SetActive(true);

        /*
        for (int targetsShot = 0; targetsShot < iceFloor.Length; targetsShot++)
        {
            iceFloor[targetsShot].SetActive(true);
        }
        */
        /*
        if(targetsShot == 1)
        {
            iceFloor[0].SetActive(true);
        }
        */
    }
}

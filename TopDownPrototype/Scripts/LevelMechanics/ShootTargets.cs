using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTargets : MonoBehaviour
{
    //public int amountOfTargets;
    public OpenPuzzleDoor openTheDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MagicBase>() != null)
        {
            //amountOfTargets++;
            openTheDoor.amountOfTargets++;
            openTheDoor.OpenThisDoor();
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
    
}

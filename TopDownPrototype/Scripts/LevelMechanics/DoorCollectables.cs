using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollectables : MonoBehaviour
{
    public CollectableDoor doorOpener;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorOpener.gatheredCollectables++;
            doorOpener.OpenThisDoor();
            Destroy(this.gameObject);
        }
        
    }
}

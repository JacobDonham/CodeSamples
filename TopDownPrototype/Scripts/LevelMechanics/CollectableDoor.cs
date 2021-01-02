using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableDoor : MonoBehaviour
{
    public int collectablesNeeded = 3;
    public int gatheredCollectables;
    public GameObject doorToOpen;
    public GameObject key;
    public Text goalText;

    private void Start()
    {
        //goalText.enabled = false;
        if(key != null)
            key.SetActive(false);
    }

    public void OpenThisDoor()
    {
        goalText.text = gatheredCollectables.ToString() + "/ " + collectablesNeeded.ToString() + " Collected";

        if (gatheredCollectables >= collectablesNeeded)
        {
            if(doorToOpen != null)
            {
                doorToOpen.SetActive(false);
            }
            
            if(key != null)
            {
                key.SetActive(true);
            }
            
            gatheredCollectables -= collectablesNeeded;
            //disable once the the key is picked up
            goalText.enabled = false;
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //goalText.enabled = true;
            //goalText.text = gatheredCollectables.ToString() + "/ " + collectablesNeeded.ToString();
        }
            
    }
}

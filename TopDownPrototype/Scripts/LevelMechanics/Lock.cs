using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour
{

    public PlayerStateMachine player;
    public GameObject lockedArea;
    public GameObject keyAmountText;
    public GameObject interactText;

    public bool inArea = false;

    //private bool inArea = false;

    // Start is called before the first frame update
    void Start()
    {
        lockedArea.SetActive(true);
        interactText.SetActive(false);
    }

    private void Update()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        if(player.KeyAmount > 0)
        {
            if (Input.GetKeyDown(KeyCode.E) && inArea)
            {
                Destroy(lockedArea);
                player.KeyAmount -= 1;

            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            inArea = true;
            if(player.KeyAmount > 0)
            {
                interactText.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            inArea = false;
            interactText.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public PlayerStateMachine player;
    public Text keyAmountText;

    private void Awake()
    {
        keyAmountText.text = player.KeyAmount.ToString() + " Keys";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            player.KeyAmount += 1;
            keyAmountText.text = player.KeyAmount.ToString() + " Keys";
            Destroy(this.gameObject);
        }
    }
}

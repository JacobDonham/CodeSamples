using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Health"))
            {
                other.GetComponent<PlayerHealth>().Heal();
                Destroy(this.gameObject);
            }
            else if (this.gameObject.CompareTag("Freeze"))
            {
                //make frozen bullets
                other.GetComponent<WeaponState>().hasIceGun = true;
                PlayerStateMachine player = FindObjectOfType<PlayerStateMachine>();
                player.hasIceGun = true;
                Destroy(this.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMechanic : MonoBehaviour
{
    public bool isAttacked;
    public GameObject shield;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        shield.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void BlockAttack()
    {
        shield.SetActive(true);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SwordMechanic>() != null)
        {
            
            other.GetComponent<SwordMechanic>().damage = 0;
        }
    }
}

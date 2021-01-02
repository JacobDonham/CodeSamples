using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    public bool isCracked = false;
    public int iceSteps = 0;

    public Color crackedColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnIce()
    {
        Debug.Log("Player cracked the ice");
        isCracked = true;
        iceSteps++;
    }

    public void DestroyIce()
    {
        Debug.Log("Player destroyed the ice");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object in the trigger area");
        

        if (iceSteps > 0)
        {
            DestroyIce();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (!isCracked)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
            this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.yellow;
            GetComponentInParent<Renderer>().material.color = crackedColor;
            OnIce();
        }
    }
}

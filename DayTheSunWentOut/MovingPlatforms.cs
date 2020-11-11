using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{

    private BasicPlayerController player;
    private Color playerColor;

    private int curPoint;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.5f;

    public bool isFroze = false;
    float oldSpeed;
    float newSpeed = 0.0f;
    public float freezeTime = 0.0f;
    private Color oldColor;

    private void Awake()
    {
        player = FindObjectOfType<BasicPlayerController>();
        oldColor = playerColor;
        transform.position = patrolPoints[0].position;
        for (int i = 0; i < transform.childCount; i++)
        {
            oldColor = transform.GetChild(i).GetComponent<Renderer>().material.color;

        }
        if (this.gameObject.GetComponent<Renderer>() != null)
        {
            oldColor = GetComponent<Renderer>().material.color;
        }

        oldSpeed = patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isIced();

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[curPoint].position, patrolSpeed * Time.deltaTime);
        
        if (transform.position == patrolPoints[curPoint].position)
        {
            curPoint = (curPoint + 1) % patrolPoints.Length;
        }

    }

    void isIced()
    {
        if (freezeTime > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (gameObject.GetComponentInChildren<BasicPlayerController>() == false)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, freezeTime);
                }
                
            }
            if (this.gameObject.GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, freezeTime);
            }

            patrolSpeed = newSpeed;
            isFroze = true;
            freezeTime -= Time.deltaTime;

        }
        else if (freezeTime <= 0)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                if(gameObject.GetComponentInChildren<BasicPlayerController>() == false)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.color = oldColor;
                }
                

            }
            if (this.gameObject.GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = oldColor;
            }

            
            isFroze = false;
            patrolSpeed = oldSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
       
    }

    private void OnTriggerStay(Collider other)
    {
        playerColor = oldColor;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }

    void Detach()
    {
        transform.DetachChildren();
    }
}

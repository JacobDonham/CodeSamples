using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreezeObject : MonoBehaviour
{
    private float origSpeed;
    public float freezeTime = 5f;
    private bool isFroze;

    private void Update()
    {
        if (isFroze)
        {
            freezeTime -= Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.blue, freezeTime);
            
            if(freezeTime <= 0)
            {
                isFroze = false;
                freezeTime += 5f;
            }
        }
    }

    public void FreezeItem()
    {
        isFroze = true;
        if(this.GetComponent<NavMeshAgent>() != null)
        {
            origSpeed = GetComponent<NavMeshAgent>().speed;
            GetComponent<NavMeshAgent>().speed = 0f;
            //StartCoroutine(Freeze());
        }

        if(GetComponent<EnemyShooting>() != null)
        {
            GetComponent<EnemyShooting>().isFroze = true;
        }

        /*
        if (GetComponent<TurretEnemy>() != null)
        {
            GetComponent<TurretEnemy>().timeBetweenShots = freezeTime;
        }
        else if (GetComponent<ShootingEnemy>() != null)
        {
            GetComponent<ShootingEnemy>().timeBetweenShots = freezeTime;
        }
        else if (GetComponent<ShootPatrol>() != null)
        {
            GetComponent<ShootPatrol>().timeBetweenShots = freezeTime;
        }
        */
    }

    /*
    public IEnumerator Freeze()
    {

        GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.blue, freezeTime);
        yield return new WaitForSeconds(freezeTime);
        GetComponent<NavMeshAgent>().speed = origSpeed;

        if (GetComponent<TurretEnemy>() != null)
        {
            GetComponent<TurretEnemy>().timeBetweenShots = GetComponent<TurretEnemy>().origTBS;
        }
        else if (GetComponent<ShootingEnemy>() != null)
        {
            //GetComponent<ShootingEnemy>().timeBetweenShots = GetComponent<ShootingEnemy>().origTBS;
        }
        else if (GetComponent<ShootPatrol>() != null)
        {
            GetComponent<ShootPatrol>().timeBetweenShots = GetComponent<ShootPatrol>().origTBS;
        }
    }
    */
}

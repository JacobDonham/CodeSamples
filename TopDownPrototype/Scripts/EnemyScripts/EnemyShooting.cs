using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public EnemyProjectile projectile;
    public GameObject firePoint;
    //public float timeBetweenShots;
    //public float origTBS;

    public bool isFroze = false;
    public float freezeTime = 5f;
    public float origFreezeTime;

    // Start is called before the first frame update
    void Start()
    {
        //origTBS = timeBetweenShots;
        origFreezeTime = freezeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFroze)
        {
            freezeTime -= Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.blue, freezeTime);

            if(freezeTime <= 0f)
            {
                isFroze = false;
                freezeTime = origFreezeTime;
            }
        }
    }

    public void ShootGun()
    {
        if (isFroze)
            return;

        Instantiate(projectile, firePoint.transform.position, transform.rotation);
    }

    /*
    public void StopShooting()
    {
        StopCoroutine(Shoot());
    }

    
    IEnumerator Shoot()
    {
        Instantiate(projectile, firePoint.transform.position, transform.rotation);
        yield return null;
        //yield return new WaitForSeconds(timeBetweenShots);
    }
    */
}

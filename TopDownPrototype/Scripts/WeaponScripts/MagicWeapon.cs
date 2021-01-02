using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWeapon : MonoBehaviour
{
    public MagicBase magic;
    public float magicSpeed = 5f;
    public float timeBetweenShots;
    private float origTBS;
    public Transform firePoint;

    private void Awake()
    {
        origTBS = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot();
        timeBetweenShots -= Time.deltaTime;
    }

    public void Shoot()
    {
        if(timeBetweenShots <= 0)
        {
            MagicBase newMagic = Instantiate(magic, firePoint.position, firePoint.rotation) as MagicBase;
            newMagic.speed = magicSpeed;
            //play audio source
            timeBetweenShots = origTBS;
        }

    }
}

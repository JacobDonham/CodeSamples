using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    public float curHealth;

    public bool takeDamage = false;
    public float hurtTime = .5f;

    public float deathTime = 2.5f;
    public bool isDead = false;

    //private WeaponState weaponState;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        //weaponState = FindObjectOfType<WeaponState>();
    }

    private void Update()
    {
        if (takeDamage)
        {
            hurtTime -= Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, hurtTime);

            if (hurtTime <= 0)
            {
                GetComponent<Renderer>().material.color = Color.white;
                takeDamage = false;
                hurtTime = .5f;
            }

            //Debug.Log(curHealth);
        }

        if (deathTime > Time.deltaTime && isDead)
        {
            deathTime -= Time.deltaTime;
            GetComponent<Collider>().enabled = false;
            
            GetComponent<Renderer>().material.color = Color.Lerp(Color.black, Color.red, deathTime);

            if (deathTime <= Time.deltaTime)
            {
                //Debug.Log("Enemy is gone");
                Destroy(this.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        takeDamage = true;

        if (curHealth <= 0)
        {
            //weaponState.currentAmmo += 2;
            isDead = true;
            //Debug.Log("Enemy dead");
        }
    }
}

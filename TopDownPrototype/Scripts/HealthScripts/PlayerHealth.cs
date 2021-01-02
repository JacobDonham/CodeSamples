using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //public Slider healthBar;
    public Image healthBar;

    public AudioClip hurtClip;
    public AudioClip deathClip;

    public float maxHealth = 5f;
    public float curHealth;

    public bool takeDamage = false;
    public float hurtTime = .5f;

    public float deathTime = 2.5f;
    public bool isDead = false;

    public bool isImmortal = false;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        //healthBar.value = curHealth / maxHealth;
        healthBar.transform.localScale = new Vector2(curHealth / maxHealth, healthBar.transform.localScale.y);
    }

    private void Update()
    {
        if (takeDamage)
        {
            hurtTime -= Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, hurtTime);
            //healthBar.value = curHealth / maxHealth;
            

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
                Destroy(this.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void Damage(float damage)
    {
        if (isImmortal)
            return;

        curHealth -= damage;
        takeDamage = true;
        healthBar.transform.localScale = new Vector2(curHealth / maxHealth, healthBar.transform.localScale.y);
        SoundManager.instance.PlaySingle(hurtClip);

        if (curHealth <= 0)
        {
            isDead = true;
            SoundManager.instance.PlaySingle(deathClip);
            //Debug.Log("Player dead");
        }
    }

    public void Heal()
    {
        curHealth = maxHealth;
        healthBar.transform.localScale = new Vector2(curHealth / maxHealth, healthBar.transform.localScale.y);
    }
}

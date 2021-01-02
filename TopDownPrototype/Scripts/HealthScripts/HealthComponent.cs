using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;

    public int thisScene;

    public delegate void HealthDelegate(float damage);
    HealthDelegate callBack;
    HealthDelegate onHurt;
    HealthDelegate onDead;

    // Start is called before the first frame update
    void Awake()
    {
        curHealth = maxHealth;
        thisScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void BindCallBack(HealthDelegate function)
    {
        callBack += function;
    }
    public void CallcallBack(HealthDelegate function)
    {
        callBack?.Invoke(curHealth / maxHealth);
    }
    public void UnBindCallBack(HealthDelegate function)
    {
        callBack -= function;
    }

    public void BindonHurt(HealthDelegate function)
    {
        callBack += function;
    }
    public void CallonHurt()
    {
        onHurt?.Invoke(curHealth / maxHealth);
    }
    public void UnBindonHurt(HealthDelegate function)
    {
        callBack -= function;
    }

    public void BindonDead(HealthDelegate function)
    {
        callBack += function;
    }
    public void CallOnDead()
    {
        if(onDead != null)
        {
            SceneManager.LoadScene(thisScene);
        }
    }
    public void UnBindOnDead(HealthDelegate function)
    {
        callBack -= function;
    }

    public void DamageTaken(float damage)
    {
        //if (curHealth == 0f)
        //    return;

        curHealth = Mathf.Clamp(curHealth - damage, 0f, maxHealth);

        if(curHealth > 0f)
        {
            CallonHurt();
            Debug.Log("Health is called");
        }
        else
        {
            //CallOnDead();
            if (this.CompareTag("Player"))
            {
                //SceneManager.LoadScene(thisScene);
                Debug.Log("Death is called");
            }

            if (CompareTag("Enemy"))
            {
                Debug.Log("The other object has been eliminated");
                GetComponent<Material>().color = Color.Lerp(Color.white, Color.red, 1f);
                Destroy(this.gameObject);
            }
                
            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageTaken(1f);
    }
}

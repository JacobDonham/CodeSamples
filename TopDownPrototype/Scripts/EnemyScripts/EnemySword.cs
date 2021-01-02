using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public float damage = 1f;
    public float origDamage;

    public Animator anim;
    public ParticleSystem particle;
    public GameObject particleChild;
    //private PlayerStateMachine player;

    //private Vector3 forward;
    //private Vector3 toPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        origDamage = damage;
        anim = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        //player = FindObjectOfType<PlayerStateMachine>();
    }
    
    public void Swing(bool attack)
    {
        if (attack)
        {

            particleChild.SetActive(true);
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            particle.Play();
            anim.SetBool("isAttacking", true);

            
        }
        else if (!attack)
        {
            particleChild.SetActive(false);
            particle.Stop();
            anim.SetBool("isAttacking", false);
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }
    }
    /*
    public void Attack()
    {
        particleChild.SetActive(true);
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        particle.Play();
        anim.SetBool("isAttacking", true);
        //swordCollider.SetActive(true);
        //particle.Play();
    }

    public void StopAttack()
    {
        particleChild.SetActive(false);
        particle.Stop();
        anim.SetBool("isAttacking", false);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        //swordCollider.SetActive(false);
        //particle.Stop();
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            particleChild.SetActive(false);
            particle.Stop();
            anim.SetBool("isAttacking", false);
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }

        if (other.GetComponent<PlayerHealth>() != null)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toPlayer = other.gameObject.transform.position - this.transform.position;

            if (other.GetComponent<PlayerStateMachine>().isBlocking != true)
            {
                Debug.Log("Player is getting injured");
                other.gameObject.GetComponent<PlayerHealth>().Damage(damage);
            }
            else if(Vector3.Dot(forward, toPlayer) < 0)
            {
                Debug.Log("Dot Product Works");
                other.gameObject.GetComponent<PlayerHealth>().Damage(damage);
            }

            
        }
    }
}

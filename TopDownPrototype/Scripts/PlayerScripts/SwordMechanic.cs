using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SwordMechanic : MonoBehaviour
{
    public float damage = 1f;
    public float origDamage;

    public float knockForce;
    public float knockTime;
    
    //public float rotateSpeed;
    //public GameObject handPoint;
    public Animator anim;
    public ParticleSystem particle;
    public GameObject particleChild;

    public AudioClip enemyHit;
    public AudioClip wallHit;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        origDamage = damage;
        anim = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
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
        else if(!attack)
        {
            GetComponent<Collider>().enabled = false;
            particleChild.SetActive(false);
            particle.Stop();
            anim.SetBool("isAttacking", false);
            GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Sword is hitting wall");
            SoundManager.instance.PlaySingle(wallHit);
            
            Swing(false);
            GetComponent<Collider>().enabled = false;
            particleChild.SetActive(false);
            particle.Stop();
            anim.SetBool("isAttacking", false);
            GetComponent<Renderer>().enabled = false;
            
        }

        if (other.tag == "Enemy")
        {
            //Debug.Log("Enemy is getting hurt");
            Rigidbody enemy = other.GetComponent<Rigidbody>();
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            SoundManager.instance.PlaySingle(enemyHit);

            if(enemy != null)
            {
                Vector3 distance = enemy.transform.position - transform.position;
                distance = distance.normalized * knockForce;
                enemy.AddForce(distance, ForceMode.Impulse);
                StartCoroutine(KnockBack(enemy));
            }
        }
    }

    private IEnumerator KnockBack(Rigidbody enemy)
    {
        yield return new WaitForSeconds(knockTime);

        if (enemy != null)
        {
            
            enemy.velocity = Vector3.zero;
        }
    }
}

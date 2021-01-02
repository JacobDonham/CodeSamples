using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float swordDamage = 1f;
    private float origDamage;

    public GameObject swordCollider;
    private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        swordCollider.SetActive(false);
    }

    public void Attack()
    {
        swordCollider.SetActive(true);
        particle.Play();
    }

    public void StopAttack()
    {
        swordCollider.SetActive(false);
        particle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            Debug.Log("Enemy is injured");
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(swordDamage);
        }
    }
}

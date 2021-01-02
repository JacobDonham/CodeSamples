using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicBase : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float AOETime = 5f;
    public float magicDamage;
    public float scaleSize = 10f;
    public bool hasExpanded = false;

    private EnemyHealth enemy;
    private PlayerHealth player;
    public bool usingIceBullet = false;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            if (usingIceBullet)
            {

                other.GetComponent<FreezeObject>().FreezeItem();

            }

            if (other.GetComponent<EnemyHealth>() != null)
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(magicDamage);
                Destroy(this.gameObject);
            }
        }
    }
}

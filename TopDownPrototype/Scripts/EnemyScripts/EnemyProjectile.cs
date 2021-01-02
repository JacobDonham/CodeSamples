using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public float projectileLife = 5f;

    public float knockForce;
    public float knockTime;

    private PlayerStateMachine player;
    private GameObject shield;
    private Quaternion enemyRotation;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStateMachine>();
        shield = player.shield;
        target = new Vector3(player.transform.position.x,player.transform.position.y, player.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        projectileLife -= Time.deltaTime;
        if(projectileLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            /*
            CharacterController controller = other.GetComponent<CharacterController>();

            if(controller != null)
            {
                Vector3 distance = controller.transform.position - transform.position;
                distance = distance.normalized * knockForce;
                //controller.Move(distance);
                StartCoroutine(playerKnock(controller, distance));
            }
            */
            /*
            if (shield.activeInHierarchy)
            {
                damage = 0f;
            }*/
            other.GetComponent<PlayerHealth>().Damage(damage);
            Destroy(this.gameObject);
        }

        


        if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    /*
    private IEnumerator playerKnock(CharacterController controller, Vector3 distance)
    {
        if(controller != null)
        {
            yield return new WaitForSeconds(knockTime);
            controller.Move(distance);
            //yield return null;
        }
    }
    */
}

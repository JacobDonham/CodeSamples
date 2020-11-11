using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Animations;

public class BasicPlayerController : MonoBehaviour
{

    public static BasicPlayerController ActivePlayer;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController _controller;
    public bool facingRight = true;
    private GameObject StartPosition;
    private SceneManager sceneManager;
    private TrailRenderer playerTail;
    private PlayerHeath playerHealth;

    private Animator animator;
    public Renderer hairChange;
    public Color purple;
    public Color yellow;
    public Color red;

    [Header("Player Stats")]
    public float speed = 5.0f;
    public int jumpAmout = 2;
    public int prevJumpAmount;
    private int oldJumpAmout;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField]
    private float fallMultiplier = 10f;
    [SerializeField]
    private float lowJump = 3f;
    private float gravity = 20.0f;
    private float oldGravity;
    private float stickyGravity = 0;
    private bool isSticky = false;

    [Header("Pickup Stats")]
    public float pickupTimer = 0.0f;
    public bool speedUp = false;
    public float speedPickupSpeed = 20f;
    private float oldSpeed;

    private bool enemyAttacked = false;
    private float _playerDamage = 2.5f;
    private float enemyAttackTime = 0f;

    [Header("Wall and Ceiling Stuff")]
    public Transform _onCeiling;
    private bool _onTheCeiling;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHeath>();
        playerTail = GetComponentInChildren<TrailRenderer>();
        prevJumpAmount = jumpAmout;
        _controller = GetComponent<CharacterController>();
        oldSpeed = speed;
        oldGravity = gravity;
        oldJumpAmout = jumpAmout;
        sceneManager = FindObjectOfType<SceneManager>();
        LoadPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.win)
        {
            return;
        }

        if(playerHealth.curHealth <= 0)
        {
            return;
        }

        if (enemyAttacked)
        {
            enemyAttackTime += Time.deltaTime;
            if(enemyAttackTime >= .5)
            {
                enemyAttackTime = 0;
                enemyAttacked = false;
            }
        }

        if (_controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(0, 0, Input.GetAxis("Horizontal"));
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;
            jumpAmout = oldJumpAmout;
            prevJumpAmount = oldJumpAmout;
            animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));
            animator.SetBool("Jumping", false);
            animator.SetBool("Jump2", false);
        }
        else
        {
            float temp = moveDirection.y;
            moveDirection = new Vector3(0, 0, Input.GetAxis("Horizontal"));
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;
            moveDirection.y = temp;
        }

        if (Input.GetButtonDown("Jump") && jumpAmout > 0)
        {
            
            if(jumpAmout == prevJumpAmount)
            {
                moveDirection.y = jumpSpeed;
                jumpAmout -= 1;
                animator.SetBool("Jumping", true);
            }
            else if(jumpAmout != prevJumpAmount && animator.GetBool("Jump2") == true)
            {
                moveDirection.y = jumpSpeed;
                jumpAmout -= 1;
                animator.SetBool("Jumping", true);
                animator.SetBool("Jump2", false);
            }
            else if (jumpAmout != prevJumpAmount && animator.GetBool("Jumping") == true)
            {
                moveDirection.y = jumpSpeed;
                jumpAmout -= 1;
                animator.SetBool("Jump2", true);
                animator.SetBool("Jumping", false);
            }
            else if (jumpAmout != prevJumpAmount && animator.GetBool("Jumping") != true && animator.GetBool("Jump2") != true)
            {
                moveDirection.y = jumpSpeed;
                prevJumpAmount -= 1;
                animator.SetBool("Jumping", true);
            }
            else if(jumpAmout == 0)
            {

                return;
            }
        }

        TailChange();

        //if the players velocity on the y axis is less than 0
        if (_controller.velocity.y < 0)
        {
            //applies a gravity multiplier to the player while in the air
            moveDirection += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_controller.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //if the player releases the jump button, set the player gravity by a low multiplier
            moveDirection += Vector3.up * Physics.gravity.y * (lowJump - 1) * Time.deltaTime;
        }

        if (moveDirection.x > 0 && !facingRight)
        {
            PlayerFlip();
        }
        else if (moveDirection.x < 0 && facingRight)
        {
            PlayerFlip();
        }

        // Apply gravity  Notice the y gets divided by deltaTime twice
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        // Move the controller
        _controller.Move(moveDirection * Time.deltaTime);
    }

    void PlayerFlip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    //changes color of the player tail
    void TailChange()
    {
        if(jumpAmout >= 2)
        {
            playerTail.material.color = purple;
            hairChange.material.color = purple;

        }
        else if(jumpAmout == 1)
        {
            playerTail.material.color = yellow;
            hairChange.material.color = yellow;
        }
        else if(jumpAmout <= 0)
        {
            playerTail.material.color = red;
            hairChange.material.color = red;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            if(enemyAttackTime <= 0)
            {
                gameObject.GetComponent<PlayerHeath>().DamageTaken(5f);
                enemyAttacked = true;
                Debug.Log("Player took damage");
            }
        }

        if (hit.gameObject.CompareTag("DeathVolume"))
        {
            this.gameObject.GetComponent<PlayerHeath>().DamageTaken(5);
        }
        
        if(!_controller.isGrounded && hit.gameObject.tag != "Wall")
        {
           // moveDirection.y = (gravity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Enemy")
        //{
        //    //Calls the Damage taken function from the Player Health Script
        //    this.gameObject.GetComponent<PlayerHeath>().DamageTaken(5);
        //}
    }

    //info for the checkpoints
    public void SavePlayer()
    {
        Debug.Log("Saving PlayerData");
        PlayerData data = new PlayerData();
        data.Position.x = transform.position.x;
        data.Position.y = transform.position.y;
        data.Position.z = transform.position.z;
        SaveAndLoad.SavePlayer(this);
    }


    public void LoadPlayer()
    {
        string path = Application.persistentDataPath + "player.capstone";
        if (File.Exists(path))
        {
            Debug.Log("Loading PlayerData");
            PlayerData data = SaveAndLoad.LoadPlayer();

            if (data.Levelindex == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                var p = data.Position;
                Vector3 pos = new Vector3(p.x, p.y, p.z);
                transform.position = pos;
            }           
        }
        else
        {
            Debug.LogWarning("No save file");
        }

    }

}

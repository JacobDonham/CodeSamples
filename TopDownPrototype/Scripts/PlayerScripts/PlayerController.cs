using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStateMachine
    {
        Idle,
        Walk,
        Attack,
        Magic,
    }

    private PlayerStateMachine curPlayerState;
    private PlayerStateMachine prevPlayerState;

    private PlayerHealth health;
    public SwordMechanic[] enemySword;
    public float blockDamage = .25f;
    bool isAttacking = false;

    private CharacterController playerController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 prevDirection;
    private Animator anim;
    public GameObject sword;
    public GameObject shield;

    private Camera mainCamera;
    Vector3 pointToLook;

    public float playerSpeed = 10;
    public float gravity = 20f;
    private float horizontalInput;
    private float VerticalInput;
    public float dashDistance = 5f;

    private MagicWeapon magicWeapon;

    // Start is called before the first frame update
    void Start()
    {
        enemySword = FindObjectsOfType<SwordMechanic>();
        curPlayerState = PlayerStateMachine.Idle;
        health = GetComponent<PlayerHealth>();
        playerController = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
        anim = GetComponentInChildren<Animator>();
        shield.SetActive(false);
        magicWeapon = GetComponent<MagicWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.isDead)
        {
            return;
        }

        //transform.forward = moveDirection;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //Add Everything to state machine to get the game to run and play better
        /*switch (curPlayerState)
        {
            case PlayerStateMachine.Walk:
                PlayerMove();
                break;
            case PlayerStateMachine.Attack:
                PlayerAttack();
                break;
            case PlayerStateMachine.Magic:
                PlayerMagic();
                break;
            case PlayerStateMachine.Idle:
                PlayerIdle();
                break;
        }*/

        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            GetComponentInChildren<SwordMechanic>().Swing(true);
        }
        else
        {
            GetComponentInChildren<SwordMechanic>().Swing(false);
            isAttacking = false;
        }

        if (Input.GetButton("Fire2") && !isAttacking)
        {
            //use dot product to see if the enemy is in front of player to block damage
            //this would allow the player to still take damage from behind or to the player's side

            shield.SetActive(true);
            moveDirection = Vector3.zero;
            foreach(SwordMechanic swords in enemySword)
            {
                swords.damage = blockDamage;
            }
        }
        else
        {
            shield.SetActive(false);
            moveDirection = new Vector3(horizontalInput, (gravity * -1 * Time.deltaTime), VerticalInput);
            foreach (SwordMechanic swords in enemySword)
            {
                swords.damage = swords.origDamage;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            magicWeapon.Shoot();
        }
            

        horizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        
        moveDirection *= playerSpeed;

        if(moveDirection != Vector3.zero)
        {
            prevDirection = moveDirection;
        }
        

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Trying to dash");
            moveDirection *= dashDistance;
        }

        //Debug.Log("The player's current state is " + curPlayerState);
        

        playerController.Move(moveDirection * Time.deltaTime);
        
    }

    /*public void PlayerIdle()
    {
        moveDirection = new Vector3(horizontalInput, 0, VerticalInput);
        moveDirection *= playerSpeed;

        if(horizontalInput > 0f || VerticalInput > 0f)
        {
            curPlayerState = PlayerStateMachine.Walk;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            curPlayerState = PlayerStateMachine.Attack;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            curPlayerState = PlayerStateMachine.Magic;
        }
    }

    public void PlayerMove()
    {
        moveDirection = new Vector3(horizontalInput, 0, VerticalInput);
        moveDirection *= playerSpeed;

        if (horizontalInput == 0f || VerticalInput == 0f)
        {
            curPlayerState = PlayerStateMachine.Idle;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            curPlayerState = PlayerStateMachine.Attack;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            curPlayerState = PlayerStateMachine.Magic;
        }
    }

    public void PlayerAttack()
    {
        //GetComponentInChildren<SwordMechanic>().Swing();

        moveDirection = new Vector3(horizontalInput, 0, VerticalInput);
        moveDirection *= playerSpeed;

        if (horizontalInput == 0f || VerticalInput == 0f)
        {
            curPlayerState = PlayerStateMachine.Idle;
        }
        else if (horizontalInput > 0f || VerticalInput > 0f)
        {
            curPlayerState = PlayerStateMachine.Walk;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            curPlayerState = PlayerStateMachine.Magic;
        }
    }

    public void PlayerMagic()
    {
        //shoot magic out of left hand

        moveDirection = new Vector3(horizontalInput, 0, VerticalInput);
        moveDirection *= playerSpeed;

        if (horizontalInput == 0f || VerticalInput == 0f)
        {
            curPlayerState = PlayerStateMachine.Walk;
        }
        else if (horizontalInput > 0f || VerticalInput > 0f)
        {
            curPlayerState = PlayerStateMachine.Walk;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            curPlayerState = PlayerStateMachine.Attack;
        }
    }*/
}

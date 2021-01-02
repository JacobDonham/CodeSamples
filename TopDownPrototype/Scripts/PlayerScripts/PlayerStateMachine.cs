using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Attack,
        Block,
        Shoot
    }

    public PlayerState curPlayerState;

    private Camera mainCamera;
    Vector3 pointToLook;

    private CharacterController playerController;
    private PlayerHealth health;
    //public SwordAttack sword;
    public SwordMechanic sword;
    private WeaponState weapon;
    private Vector3 moveDir = Vector3.zero;
    private float horizontalInput;
    private float verticalInput;
    public float playerSpeed = 10f;
    public float gravity = 20f;

    [Header("Sword Attack")]
    public float attackTime = .5f;
    private float origAttackTime;
    //public float attackDelay = .5f;
    private float origAttackDelay;
    private bool swordAttacking = false;
    //private ParticleSystem particle;
    
    public GameObject shield;
    public bool isBlocking;

    public int KeyAmount;

    //For Saving the game
    public int level;
    public Vector3 checkpointPosition;
    public bool hasIceGun;
    public bool hasShotGun;
    //private SoundManager soundManager;
    

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
        health = GetComponent<PlayerHealth>();
        weapon = GetComponent<WeaponState>();
        curPlayerState = PlayerState.Idle;
        origAttackTime = attackTime;
        //origAttackDelay = attackDelay;
        shield.SetActive(false);
        level = SceneManager.GetActiveScene().buildIndex;
        //LoadPlayer();
        /*
        if (hasIceGun || hasShotGun)
        {
            weapon.UpdateWeapons();
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (health.isDead)
            return;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //transform.forward = moveDir;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        switch (curPlayerState)
        {
            case PlayerState.Idle:
                PlayerIdle();
                break;
            case PlayerState.Move:
                PlayerMove();
                break;
            case PlayerState.Attack:
                PlayerAttack();
                break;
            case PlayerState.Block:
                PlayerBlock();
                break;
            case PlayerState.Shoot:
                PlayerShoot();
                break;
        }
        

        moveDir.y -= gravity;
            
        //Debug.Log(curPlayerState);

        playerController.Move(moveDir * Time.deltaTime);
    }

    public void PlayerIdle()
    {
        //for animations

        moveDir = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = Vector3.ClampMagnitude(moveDir, 1) * playerSpeed;
        //moveDir *= playerSpeed;

        //State machine transition
        if (moveDir != Vector3.zero)
        {
            curPlayerState = PlayerState.Move;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            swordAttacking = true;
            //sword.Attack();
            sword.Swing(true);
            curPlayerState = PlayerState.Attack;
        }
        else if (Input.GetButtonDown("Block"))
        {
            curPlayerState = PlayerState.Block;
        }
        else if (Input.GetButton("Fire2"))
        {
            //sword.StopAttack();
            sword.Swing(false);
            attackTime = origAttackTime;
            weapon.ShootGun();
            curPlayerState = PlayerState.Shoot;
        }
    }

    public void PlayerMove()
    {
        //for animations

        moveDir = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = Vector3.ClampMagnitude(moveDir, 1) * playerSpeed;
        //moveDir *= playerSpeed;

        //State machine transition
        if (moveDir == Vector3.zero)
        {
            curPlayerState = PlayerState.Idle;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            swordAttacking = true;
            //sword.Attack();
            sword.Swing(true);
            curPlayerState = PlayerState.Attack;
        }
        else if (Input.GetButtonDown("Block"))
        {
            curPlayerState = PlayerState.Block;
        }
        else if (Input.GetButton("Fire2"))
        {
            //sword.StopAttack();
            sword.Swing(false);
            attackTime = origAttackTime;
            weapon.ShootGun();
            curPlayerState = PlayerState.Shoot;
        }
    }

    public void PlayerAttack()
    {
        moveDir = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = Vector3.ClampMagnitude(moveDir, 1) * playerSpeed;
        //moveDir *= playerSpeed;

        if (Input.GetButtonDown("Fire1"))
        {
            

            if (attackTime > 0)
            {
                //sword.Attack();
                swordAttacking = true;
                sword.Swing(true);
            }
        }

        if (swordAttacking)
            attackTime -= Time.deltaTime;

        if (attackTime <= 0)
        {
            swordAttacking = false;
        }

        if (!swordAttacking || health.isDead)
        {
            //sword.StopAttack();
            sword.Swing(false);
            attackTime = origAttackTime;
        }

        //State machine transition
        if (moveDir == Vector3.zero && !swordAttacking)
        {
            curPlayerState = PlayerState.Idle;
        }
        else if (moveDir != Vector3.zero && !swordAttacking)
        {
            curPlayerState = PlayerState.Move;
        }
        else if (Input.GetButtonDown("Block") && !swordAttacking)
        {
            curPlayerState = PlayerState.Block;
        }
        else if (Input.GetButton("Fire2") && !swordAttacking)
        {
            //sword.StopAttack();
            sword.Swing(false);
            attackTime = origAttackTime;
            weapon.ShootGun();
            curPlayerState = PlayerState.Shoot;
        }
    }

    public void PlayerBlock()
    {
        moveDir = Vector3.zero;

        if (Input.GetButton("Block"))
        {
            //block stuff
            isBlocking = true;
            shield.SetActive(true);
        }
        else if (!Input.GetButton("Block"))
        {
            isBlocking = false;
            shield.SetActive(false);
        }

        if(moveDir == Vector3.zero && !Input.GetButton("Block"))
        {
            curPlayerState = PlayerState.Idle;
        }
        else if (moveDir != Vector3.zero && !Input.GetButton("Block"))
        {
            curPlayerState = PlayerState.Move;
        }
        else if (Input.GetButtonDown("Fire1") && !Input.GetButton("Block"))
        {
            swordAttacking = true;
            //sword.Attack();
            sword.Swing(true);
            curPlayerState = PlayerState.Attack;
        }
        else if (Input.GetButton("Fire2") && !Input.GetButton("Block"))
        {
            //sword.StopAttack();
            sword.Swing(false);
            attackTime = origAttackTime;
            weapon.ShootGun();
            curPlayerState = PlayerState.Shoot;
        }
    }

    public void PlayerShoot()
    {
        moveDir = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = Vector3.ClampMagnitude(moveDir, 1) * playerSpeed;
        //moveDir *= playerSpeed;

        if (Input.GetButtonDown("Fire2"))
        {
            weapon.ShootGun();
        }
        
        //State machine transitions
        if(moveDir == Vector3.zero && !Input.GetButton("Fire2"))
        {
            curPlayerState = PlayerState.Idle;
        }
        else if (moveDir != Vector3.zero && !Input.GetButton("Fire2"))
        {
            curPlayerState = PlayerState.Move;
        }
        else if (Input.GetButtonDown("Fire1") && !Input.GetButton("Fire2"))
        {
            swordAttacking = true;
            //sword.Attack();
            sword.Swing(true);
            curPlayerState = PlayerState.Attack;
        }
        else if (Input.GetButtonDown("Block") && !Input.GetButton("Fire2"))
        {
            curPlayerState = PlayerState.Block;
        }

    }
    
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        /*
        if(hasIceGun || hasShotGun)
        {
            hasIceGun = data.iceGunActive;
            hasShotGun = data.shotGunActive;
        }
        else
        {
            data.iceGunActive = false;
            data.shotGunActive = false;
        }*/

        Vector3 position;
        position.x = data.checkpointPos[0];
        position.y = data.checkpointPos[1];
        position.z = data.checkpointPos[2];
        transform.position = position;

    }
}

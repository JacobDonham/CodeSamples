using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_script : MonoBehaviour {

    private NewPatrollingEnemy[] enemy;
    private MovingPlatforms[] movers;
    private RotationSystem[] spinners;
    private TurretWeapon[] turrets;
    public float respawnTime = 2.5f;
    float oldRespawnTime;
    bool pickedUp;

    private void Awake()
    {
        oldRespawnTime = respawnTime;
        enemy = FindObjectsOfType<NewPatrollingEnemy>();
        spinners = FindObjectsOfType<RotationSystem>();
        movers = FindObjectsOfType<MovingPlatforms>();
        turrets = FindObjectsOfType<TurretWeapon>();
    }
    private void Update()
    {
        ItemPickedUp();
    }

    private void ItemPickedUp()
    {
        if (pickedUp)
        {
            respawnTime -= Time.deltaTime;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            return;
        }
        
        if(respawnTime <= 0)
        {
            pickedUp = false;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            respawnTime = oldRespawnTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        BasicPlayerController player = other.GetComponent<BasicPlayerController>();
        if (player != null) {

            pickedUp = true;

            if(this.gameObject.tag == "Ice")
            {

                foreach(NewPatrollingEnemy badguy in enemy)
                {
                    badguy.GetComponent<NewPatrollingEnemy>().frozeTime = 5f;
                }

                foreach(MovingPlatforms platform in movers)
                {
                    platform.GetComponent<MovingPlatforms>().freezeTime = 2.5f;
                }

                foreach(RotationSystem spinner in spinners)
                {
                    spinner.GetComponent<RotationSystem>().frozenTime = 2.5f;
                }

                foreach(TurretWeapon turret in turrets)
                {
                    turret.GetComponent<TurretWeapon>().freezeTurret = 5f;
                }
            }

            if(gameObject.tag == "Speed")
            {
                other.GetComponent<BasicPlayerController>().pickupTimer = 5f;
            }

            if (gameObject.CompareTag("ExtraJump"))
            {
                //other.GetComponent<NewJumpForMasters>().amountJumps += 1;
                other.GetComponent<BasicPlayerController>().jumpAmout += 1;
                other.GetComponent<BasicPlayerController>().prevJumpAmount += 1;
                other.GetComponent<BasicPlayerController>().prevJumpAmount = other.GetComponent<BasicPlayerController>().jumpAmout;
            }

            if (gameObject.tag == ("Health"))
            {
                other.GetComponent<PlayerHeath>().NewHealth();
                Debug.Log("GotHealth");
                other.GetComponent<PlayerHeath>().UpdateBar();
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public GameObject DebugCanvas;

    public PlayerHealth playerHealth;
    public PlayerStateMachine player;
    public EnemyHealth[] enemies;

    //public GameObject checkpointCanvas;
    public int curCheckpoint;
    public Checkpoint[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        player = FindObjectOfType<PlayerStateMachine>();
        //checkpointCanvas.SetActive(false);
        DebugCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<EnemyHealth>();

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (!DebugCanvas.activeInHierarchy)
                DebugCanvas.SetActive(true);
            else if (DebugCanvas.activeInHierarchy)
                DebugCanvas.SetActive(false);
        }
    }

    public void Immortality()
    {
        //playerHealth.curHealth = 500f;
        if(!playerHealth.isImmortal)
            playerHealth.isImmortal = true;
        else if(playerHealth.isImmortal)
            playerHealth.isImmortal = false;
    }

    public void TakeDamage()
    {
        playerHealth.Damage(1f);
    }

    public void KillPlayer()
    {
        playerHealth.Damage(100f);
    }

    public void DestroyEnemies()
    {
        //enemies = FindObjectsOfType<EnemyHealth>();

        foreach(EnemyHealth enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(100f);
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    public void NextCheckPoint()
    {
        curCheckpoint += 1;
        player.transform.position = checkpoints[curCheckpoint].transform.position;
        /*
        if (!checkpointCanvas.activeInHierarchy)
        {
            checkpointCanvas.SetActive(true);

        }
        else if (checkpointCanvas.activeInHierarchy)
        {
            checkpointCanvas.SetActive(false);
        }*/
    }

    public void PrevCheckpoint()
    {
        curCheckpoint -= 1;
        player.transform.position = checkpoints[curCheckpoint].transform.position;
    }
    /*
    public void TeleportToCheckpoint(int number)
    {
        //code to teleport to a checkpoint
        player.transform.position = checkpoints[number].transform.position;
        Debug.Log("Teleporting to the " + number + " checkpoint");
    }
    */
}

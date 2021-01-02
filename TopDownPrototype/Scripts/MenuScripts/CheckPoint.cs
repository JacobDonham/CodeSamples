using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //private Manager gameManager;

    public int checkPointNumber;
    public bool isActive = false;
    
    private Checkpoint[] checkpoints;
    private PlayerStateMachine player;
    //private SavePlayerInfo player;

    private Renderer checkpointRender;
    public Material ActivePoint;
    public Material InactivePoint;

    private void Start()
    {
        player = FindObjectOfType<PlayerStateMachine>();
        checkpointRender = GetComponent<Renderer>();
        checkpointRender.material = InactivePoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.checkpointPosition = transform.position;
            isActive = true;
            CheckpointOn();
            player.SavePlayer();
        }
    }

    public void CheckpointOn()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        foreach(Checkpoint cp in checkpoints)
        {
            cp.CheckpointOff();
            cp.isActive = false;
        }

        checkpointRender.material = ActivePoint;
    }

    public void CheckpointOff()
    {
        checkpointRender.material = InactivePoint;
    }
}

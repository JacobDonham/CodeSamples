using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public bool iceGunActive;
    public bool shotGunActive;
    public float[] checkpointPos;
    public int Level;
    public float effectsSound;
    public float musicSound;

    public PlayerData(PlayerStateMachine player)
    {
        iceGunActive = player.hasIceGun;
        shotGunActive = player.hasShotGun;
        Level = player.level;

        

        checkpointPos = new float[3];
        checkpointPos[0] = player.checkpointPosition.x;
        checkpointPos[1] = player.checkpointPosition.y;
        checkpointPos[2] = player.checkpointPosition.z;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTargets : MonoBehaviour
{
    public IceTargetOpener opener;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceBullet"))
        {
            opener.targetsShot++;
            opener.OpenIceDoor();
            Destroy(this.gameObject);
        }
    }
}

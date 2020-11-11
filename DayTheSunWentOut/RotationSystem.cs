using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotationSystem : MonoBehaviour {

    public Vector3 sceneRot = Vector3.back;
    private BasicPlayerController playerController;
    private PlayerHeath playerHealth;
    public float spinDegrees = 90f;
    public float newspinDegrees = 0f;
    public float oldSpinDegrees;
    public float frozenTime;
    private bool isFroze;
    private Color oldColor;
    private Quaternion startRot;
   
    private void Awake()
    {
        playerController = FindObjectOfType<BasicPlayerController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            oldColor = transform.GetChild(i).GetComponent<Renderer>().material.color;
        }

        oldSpinDegrees = spinDegrees;
        startRot = transform.rotation;
    }

    private void Update () {
        isIced();
        transform.Rotate(sceneRot, spinDegrees * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHeath>().DamageTaken(10f);
        }
    }

    void isIced()
    {
        if (frozenTime > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, frozenTime);

            }
            spinDegrees = 0;
            frozenTime -= Time.deltaTime;
            transform.rotation = startRot;
        }
        else if (frozenTime <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Renderer>().material.color = oldColor;

            }
            spinDegrees = oldSpinDegrees;
        }
    }
}

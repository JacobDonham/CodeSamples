using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController playerController;
    private Vector3 moveDirection = Vector3.zero;
    public GameObject sword;

    private Camera mainCamera;

    public float playerSpeed = 10;
    private float horizontalInput;
    private float VerticalInput;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //attack
        }

        horizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0, VerticalInput);
        moveDirection *= playerSpeed;

        playerController.Move(moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy can go fuck itself");
            other.GetComponent<EnemyHealth>().TakeDamage(1f);
        }
    }
}

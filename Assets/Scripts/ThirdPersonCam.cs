using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;

    public float rotationSpeed;
    private bool use = true;
   
  

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (use) {
            // rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            //orientation.forward = viewDir.normalized;

            // roate player object
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            //playerObj.forward = orientation.forward;
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            //playerObj.forward = transform.forward;
        }
        
    }

    public void SwitchMovement()
    {
        use = !use;
    }

}

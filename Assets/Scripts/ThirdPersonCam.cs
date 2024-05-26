using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public Camera cam;

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
     
            orientation.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y,0);

            playerObj.forward = orientation.forward;
            
        }
        
    }

    public void SwitchMovement()
    {
        use = !use;
    }

}

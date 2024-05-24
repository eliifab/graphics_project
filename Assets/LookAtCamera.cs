using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    public CameraSwitcher camctrl;

    void Update()
    {
        Camera cam = camctrl.GetCamera();
        gameObject.transform.LookAt(cam.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
   public Camera targetCamera;
   public Canvas canvas;


   void Update()
   {
        // Get the current scale of the camera
        float cameraScale = targetCamera.orthographicSize;


        // Apply this scale to the Canvas
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            canvas.transform.localScale = new Vector3(cameraScale, cameraScale, 1);
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            // For ScreenSpace-Camera, adjust the scale or position as needed
            // This example simply sets the Canvas scale
            canvas.transform.localScale = new Vector3(cameraScale, cameraScale, 1);
        }
   }
}

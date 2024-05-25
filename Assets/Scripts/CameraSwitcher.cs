using UnityEngine;


public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex;

    void Start()
    {
        // Ensure only the first camera is enabled at the start
        currentCameraIndex = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = (i == currentCameraIndex);

        }
    }

    public void SwitchCamera()
    {
        // Disable the current camera
        cameras[currentCameraIndex].enabled = false;

        // Move to the next camera
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Enable the next camera
        cameras[currentCameraIndex].enabled = true;

        
    }

    public Camera GetCamera()
    {
        currentCameraIndex = (currentCameraIndex) % cameras.Length;

        return cameras[currentCameraIndex];
        
    }
}
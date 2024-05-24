using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    void Awake()
    {
        // Get all AudioListener components in the scene
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // Disable all AudioListeners except the first one
        for (int i = 1; i < audioListeners.Length; i++)
        {
            audioListeners[i].enabled = false;
        }
    }
}
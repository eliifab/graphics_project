using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // https://www.youtube.com/watch?v=FQNZwcd6FaY
    public Image healthbar;
    
    public void UpdateHealth(float fraction)
    {
        healthbar.fillAmount = fraction;
    }
}

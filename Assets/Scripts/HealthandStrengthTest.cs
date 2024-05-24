using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthandStrengthTest : MonoBehaviour
{
    
    public CharacterHealth CharHealth;
        
    public CharacterStrength CharStrength;

    public KeyCount keycount;



    void Update()
    {
        if (Input.GetKey(KeyCode.K)) {
            CharHealth.TakeDamage(1);
        }
        if (Input.GetKey(KeyCode.H)) {
            CharHealth.GetHealth(1);
        }
        if (Input.GetKey(KeyCode.J)) {
            CharStrength.GetStrength(1);
        }

        if (Input.GetKey(KeyCode.M)) {
            keycount.Collect(1);
        }
    }
}

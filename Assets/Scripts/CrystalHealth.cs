using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHealth : MonoBehaviour
{
    
    public int AddHealth;
    public CharacterHealth charhealth;

   private void OnTriggerEnter(Collider collision)
    {
        gameObject.SetActive(false);
        charhealth.GetHealth(AddHealth);
        
    }
}
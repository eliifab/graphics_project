using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalStrength : MonoBehaviour
{
    public int AddStrength;
    public CharacterStrength charstrength;

   private void OnTriggerEnter(Collider collision)
    {
        gameObject.SetActive(false);
        charstrength.GetStrength(AddStrength);
        
    }
}

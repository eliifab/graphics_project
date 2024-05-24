using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterHealth : MonoBehaviour
{
    public int maxHealth;
    public Healthbar healthbar;
    public TMP_Text CurrentHealth;

    private int curHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        CurrentHealth.text = "Health: " + curHealth.ToString();

    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth < 0) {
            curHealth = 0;
        }
        healthbar.UpdateHealth((float)curHealth / (float)maxHealth);
        CurrentHealth.text = "Health: " + curHealth.ToString();

    }

    public void GetHealth(int crystal_health)
    {
        curHealth += crystal_health;
        if (curHealth > maxHealth) {
            maxHealth = curHealth;
        }
        healthbar.UpdateHealth((float)curHealth / (float)maxHealth);
        CurrentHealth.text = "Health: " + curHealth.ToString();


    }

    public int GetHealthCount()
    {
        return curHealth;

    }


}

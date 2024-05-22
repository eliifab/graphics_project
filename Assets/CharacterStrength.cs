using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStrength : MonoBehaviour
{
    public int firstStrength;

    public TMP_Text CurrentStrength;

    private int curStrength;

    // Start is called before the first frame update
    void Start()
    {
        curStrength = firstStrength;
        CurrentStrength.text = "Strength: " + curStrength.ToString();

    }


    public void GetStrength(int new_strength)
    {
        curStrength += new_strength;
        CurrentStrength.text = "Strength: " + curStrength.ToString();


    }


}

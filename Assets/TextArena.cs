using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextArena : MonoBehaviour
{
    public TMP_Text Text1;
    public TMP_Text Text2;
    public TMP_Text Text3;
    public TMP_Text Text4;
    public TMP_Text Text5;
    public TMP_Text Text6;

    private bool myturn = true;


    void Update()
    {
        if (myturn)
        {
            Text1.text = "Attack 1: Key [u]";
            Text2.text = "Attack 2: Key [i]";
            Text3.text = "Attack 3: Key [o]";
            Text4.text = "Average Damage";
            Text5.text = "Middle weak to middle high damage";
            Text6.text = "Weak to high damage";
        }
        else
        {
            Text1.text = "";
            Text2.text = "";
            Text3.text = "";
            Text4.text = "";
            Text5.text = "";
            Text6.text = "";
        }
    }

    public void Turn()
    {
        myturn = !myturn;
    }
}

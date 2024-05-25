using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyCount : MonoBehaviour
{
    public int firstCount;

    public TMP_Text CurrentCount;

    private int curCount;

    // Start is called before the first frame update
    void Start()
    {
        curCount = firstCount;
        CurrentCount.text = curCount.ToString() + "/3";

    }


    public void Collect(int new_key)
    {
        curCount += new_key;
        CurrentCount.text = curCount.ToString() + "/3";


    }
}

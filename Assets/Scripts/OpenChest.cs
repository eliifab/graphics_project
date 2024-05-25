using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenChest : MonoBehaviour
{
    
    public KeyCount keycount;
    public TMP_Text ChestMessage;

    private Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider collision)
    {
        // output text depending on amount of keys 
        if (keycount.Amount() == 3) {
            ChestMessage.text = "Press x to open chest";
        }

        else {
            ChestMessage.text = "Not enough keys";
        }

        if (Input.GetKey(KeyCode.X)) {
            Animator.SetTrigger("Open");
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        ChestMessage.text = "";
        
    }
}

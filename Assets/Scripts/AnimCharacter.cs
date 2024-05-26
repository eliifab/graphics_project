using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimCharacter : MonoBehaviour
{

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public Animator CharAnimator;
    public bool go = true;

    private int changed = 0;
    private int changed2 = 0;

    bool grounded;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && go)
        {
            changed = 1;
            if (changed != changed2) {
                CharAnimator.SetTrigger("Go");
            }

            changed2 = changed;
        }

            
        
        else{
            changed = 2;

            if (changed != changed2){
                CharAnimator.SetTrigger("Stand");
                changed2 = changed;
            }
        }
       
       
    }

    public void Switch()
    {
        go = !go;
    }

}
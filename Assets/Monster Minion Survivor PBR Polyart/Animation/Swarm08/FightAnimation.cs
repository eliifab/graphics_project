using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightAnimation : MonoBehaviour
{

    private Animator Swarm08_Animator;

    // Start is called before the first frame update
    void Start()
    {
        Swarm08_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider collision)
    {
        //teleport
        gameObject.transform.Translate(10,0,0);
        collision.gameObject.transform.Translate(10,0,0);
    }
    

    private void OnTriggerStay(Collider other)
    {
        if(Swarm08_Animator != null)
        {
            if(Input.GetKey(KeyCode.O))
            {
                Swarm08_Animator.SetTrigger("GetHit");
            }
            if(Input.GetKey(KeyCode.I))
            {
                Swarm08_Animator.SetTrigger("Attack");
            }
            if(Input.GetKey(KeyCode.U))
            {
                Swarm08_Animator.SetTrigger("Die");
            }
        }
    }
}

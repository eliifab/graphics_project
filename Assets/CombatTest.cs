using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTest : MonoBehaviour
{
    private Animation ani;

    void Start()
    {
        ani = gameObject.GetComponent<Animation>();
        ani.Play("Swarm08_Idle");
    }

    // Update is called once per frame
    public void OnInteract()
    {
        ani.Play("Swarm08_Hit");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnim : MonoBehaviour
{
    public Animator ChestAnim;
    private Animator CharAnim;
    // Start is called before the first frame update
    void Start()
    {
        CharAnim = GetComponent<Animator>();
        CharAnim.SetTrigger("Win");
        ChestAnim.SetTrigger("Open");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIpatrol : MonoBehaviour
{
    private bool move;
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;

    //patrol
    Vector3 DestPoint;
    bool WalkpointSet;
    [SerializeField] float range, vision;
    [SerializeField] bool chase;
    bool playerSeen;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("PlayerObj");
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            playerSeen = chase && Physics.CheckSphere(transform.position, vision, playerLayer);
            
            if (!playerSeen) Patrol();
            if (playerSeen) Chase();
        }
   
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    void Patrol()
    {
        if (!WalkpointSet) SearchForDest();
        if (WalkpointSet) agent.SetDestination(DestPoint);
        if (Vector3.Distance(transform.position, DestPoint) < 10) WalkpointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        DestPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if(Physics.Raycast(DestPoint, Vector3.down, groundLayer))
        {
            WalkpointSet = true;
        }
    }

    
    public void SwitchMove(Transform trans)
    {
        move = !move;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
        gameObject.transform.position = trans.position;
        agent.enabled = true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class birdAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public LayerMask ground, playerLayer;
    public Transform enemy;
    public Animator animator;

    //wander variables (there will be no range for this)
    public float wanderSpeed;
    public float wanderVariance;
    public float minRadius;
    public float maxRadius;

    //chase variables
    public float chaseSpeed;
    public float chaseRange;
    public bool retreating;
    // no variance because it makes a beeline for the player

    public Vector3 walkPoint; //where it will walk
    bool walkPointSet; //does the walk point exist
    public float walkPointRange;

    //States
    public float sightRange;
    public bool playerInSightRange, playerInLurkRange, playerInChaseRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInChaseRange = Physics.CheckSphere(transform.position, chaseRange, playerLayer);
        if (!playerInChaseRange) Wander();
        if (playerInChaseRange) Chase();
        //if (playerInChaseRange && player.position.y > 10f) Retreat();
        //Debug.Log(playerInChaseRange);
        animator.SetFloat("agent.Speed", agent.speed);
    }

    private void Wander()
    {
        if (!walkPointSet) wanderPoint();

        if (walkPointSet)
            //print(retreating);
            agent.SetDestination(walkPoint);
            if (retreating){
                agent.speed = chaseSpeed;
            }
            else if (!retreating){
                agent.speed = wanderSpeed;
            }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
            retreating = false;
            }
    }


    private void Chase()
    {
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
    }
    private void Retreat()
    {
        if (!walkPointSet) retreatPoint();

        if (walkPointSet)
            //print(retreating);
            agent.speed = chaseSpeed;
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
            retreating = false;
        }
    }

    private void retreatPoint()
    {
        retreating = true;
        float radius = Random.Range(chaseRange*5f, chaseRange*5.5f); // corresponds to render distance
        float retreatx = Random.Range(0.01f, radius*0.99f);
        float retreatz = Mathf.Sqrt((Mathf.Pow(radius,2))-(Mathf.Pow(retreatx,2)));
        int quad = Random.Range(1,4);
        
        if (quad == 2){ 
            retreatx = -retreatx;
        }
        else if (quad==3){
            retreatx = -retreatx;
            retreatz = -retreatz;
        }
        else if (quad==4){
            retreatz = -retreatz;
        }

        walkPoint = new Vector3(transform.position.x + retreatx, transform.position.y, transform.position.z + retreatz);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            walkPointSet = true;
    }

    private void wanderPoint()
    {
         // vector from bird to player
        float bpx = ((player.position.x) - (enemy.position.x));
        //Debug.Log(bpx);
        float bpz = ((player.position.z) - (enemy.position.z));
        //Debug.Log(bpz);

        float radius = Random.Range(minRadius, maxRadius);
        float walkx = Random.Range(radius*0.5f, radius*0.999f);
        float walkz = Mathf.Sqrt((Mathf.Pow(radius,2))-(Mathf.Pow(walkx,2)));
        
        if ((bpx) < 0){ 
            walkx = -walkx;
        }

        if ((bpz) < 0){ 
            walkz = -walkz;
        }


        walkPoint = new Vector3(transform.position.x + walkx, transform.position.y, transform.position.z + walkz);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            walkPointSet = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class jayhawkAI : MonoBehaviour
{
    public TreeTrigger treeTrigger;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player; // this is what he will be tracking!
    public LayerMask ground, playerLayer;
    public Transform enemy;
    public Animator animator;
    public Camera mainCam;
    public Camera scareCam;
    public GameObject scareScene;

    public float chaseSpeed;
    public float chaseRange;
    public float wanderSpeed;
    public float wanderVariance;
    public float deathRange;
    public bool inTree;

    public AudioSource walkSource;
    public AudioSource runSource;
    public AudioSource hissSource;



    private Vector3 wanderPoint()
    {
        float x = Random.Range(-wanderVariance, wanderVariance);
        float z = Random.Range(-wanderVariance, wanderVariance);
        Vector3 dest = new Vector3(player.position.x + x, transform.position.y, player.position.z + z);
        //print("wanderpoint");
        return dest;
    }

    private Vector3 chasePoint()
    {
        Vector3 dest = new Vector3(player.position.x, player.position.y, player.position.z);
        //print("chasepoint");
        return (dest);
    }

    private Vector3 retreatPoint()
    {
        //Vector3 dest = new Vector3(player.position.x, player.position.y, player.position.z);

        float radius = Random.Range(chaseRange * 3f, chaseRange * 3.5f); // corresponds to render dist
        float retreatx = Random.Range(0.01f, radius * 0.99f);
        float retreatz = Mathf.Sqrt((Mathf.Pow(radius, 2)) - (Mathf.Pow(retreatx, 2)));
        int quad = Random.Range(1, 4);

        if (quad == 2)
        {
            retreatx = -retreatx;
        }
        else if (quad == 3)
        {
            retreatx = -retreatx;
            retreatz = -retreatz;
        }
        else if (quad == 4)
        {
            retreatz = -retreatz;
        }
        Vector3 dest = new Vector3(transform.position.x + retreatx, transform.position.y, transform.position.z + retreatz);
        //print("retreating");
        return (dest);
    }

    IEnumerator rec_pathfind()
    {
        bool playerInChaseRange = Physics.CheckSphere(transform.position, chaseRange, playerLayer);
        Vector3 walkPoint = new Vector3(0, 0, 0);
        float wait = 5f;
        if (playerInChaseRange && !inTree)
        {
            walkPoint = chasePoint();
            walkSource.volume = 0f;
            hissSource.volume = 1f;
            yield return new WaitForSeconds(0.75f);
            runSource.volume = 1f;
            agent.speed = chaseSpeed;
            wait = 0.033f;
        }
        else if (!playerInChaseRange)
        {
            walkPoint = wanderPoint();
            runSource.volume = 0f;
            walkSource.volume = 1f;
            agent.speed = wanderSpeed;
        }
        else if (playerInChaseRange && inTree)
        { //retreat
            agent.speed = 0; // SCREAM ANIMATION HERE
            runSource.volume = 0f;
            walkSource.volume = 0f;
            hissSource.volume = 1f;

            yield return new WaitForSeconds(2.8f);
            hissSource.volume = 0f;
            runSource.volume = 1f;
            walkPoint = retreatPoint();
            agent.speed = chaseSpeed;
        }


        agent.SetDestination(walkPoint);
        yield return new WaitForSeconds(wait);
        StartCoroutine("rec_pathfind");
    }

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("rec_pathfind");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inTree);
        animator.SetFloat("agent.speed", agent.speed);
        bool playerInDeathRange = Physics.CheckSphere(transform.position, deathRange, playerLayer);
        if (playerInDeathRange)
        {
            //print("DEATH");
            StopCoroutine("rec_pathfind");

            //CHANGE CAMERAS HERE
            mainCam.enabled = false;
            scareCam.enabled = true;
            //animator.SetFloat("jumpscare", 1f);

        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    private Animator Animator;
    private float StartKillTime;
    private int NextWaypoint = 0;

    public List<Transform> Checkpoints;

    private GameObject player;

    private float HealthPoints = 100;

    public AudioSource wingFlap;

    // Use this for initialization
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthPoints > 0)
        {
            NavMeshAgent.SetDestination(Checkpoints[NextWaypoint].position);

            if (Vector3.Distance(transform.position, NavMeshAgent.destination) <= NavMeshAgent.stoppingDistance)
            {
                ++NextWaypoint;
                if (NextWaypoint >= Checkpoints.Count)
                {
                    NextWaypoint = 0;
                }
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            if (Vector3.Distance(transform.position, player.transform.position) <= 20.0f)
            {
                NavMeshAgent.SetDestination(player.transform.position);
            }
        }
        else
        {
            Animator.SetTrigger("Kill");
            wingFlap.mute = true;
        }
    }

    public void Damaged()
    {
        HealthPoints = HealthPoints - 10;
        Debug.Log(HealthPoints);
        //StartKillTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("spike"))
        {
            //Debug.Log("SPIKE!");
            HealthPoints = HealthPoints - 20;
        }
    }

    public void StartSinking()
    {

    }
}

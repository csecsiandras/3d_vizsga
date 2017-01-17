using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    private Animator Animator;
    private float StartKillTime;
    private int NextWaypoint = 0;
    private bool stop = false;

    public List<Transform> Checkpoints;

    private GameObject player;

    private float HealthPoints = 100;

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
        if (!stop)
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
            if (Vector3.Distance(transform.position, player.transform.position) <= 13.0f)
            {
                NavMeshAgent.SetDestination(player.transform.position);
            }
        }
    }

    public void Damaged()
    {
        HealthPoints = HealthPoints - 5;
        Debug.Log(HealthPoints);
        if (HealthPoints <= 0)
        {
            Animator.SetTrigger("Kill");
            stop = true;
        }
        //StartKillTime = Time.time;
    }

    public void StartSinking()
    {

    }
}

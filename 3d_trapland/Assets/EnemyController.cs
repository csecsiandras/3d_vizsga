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
        if (NavMeshAgent.enabled == true)
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

            if (Vector3.Distance(transform.position, player.transform.position) <= NavMeshAgent.stoppingDistance)
            {
                player.GetComponent<PlayerController>().Damaged();
            }
        }
        else
        {
            if (Time.time - StartKillTime > 4.0f)
            {
                GameObject.Destroy(transform.gameObject);
            }
        }
    }

    public void Damaged()
    {
        NavMeshAgent.enabled = false;
        Animator.SetTrigger("Kill");
        StartKillTime = Time.time;
    }

    public void StartSinking()
    {

    }
}

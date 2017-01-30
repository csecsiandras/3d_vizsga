using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    private Animator Animator;
    private float StartKillTime;
    private int NextWaypoint = 0;

    public List<Transform> Checkpoints;

    private GameObject player;

    public float HealthPoints = 400;

    public AudioSource wingFlap;
    public AudioSource death;

    bool isDead = false;
    int timer = 0;
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
            death.mute = false;
            death.Play();
            Animator.SetTrigger("Kill");
            wingFlap.mute = true;
            isDead = true;
            
        }

        if (isDead)
        {
            timer++;
            if (timer > 120)
            {
                SceneManager.LoadScene("victory");
            }
        }
    }

    public void Damaged()
    {
        HealthPoints = HealthPoints - 4;
        Debug.Log(HealthPoints);
        //StartKillTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("spike"))
        {
            //Debug.Log("SPIKE!");
            HealthPoints = HealthPoints - 6;
        }
    }

    public void StartSinking()
    {

    }
}

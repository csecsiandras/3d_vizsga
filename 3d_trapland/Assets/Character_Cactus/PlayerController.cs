using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    private float playerX;
    private float playerZ;
    private float distToGround;
    private Vector3 curPos;
    private Vector3 lastPos;
    private bool moving = false;
    public Animator Animator;

    private bool isAttack;
    private GameObject spike;
    private GameObject prefab;

    private GameObject enemy;
    private float AttackRadius = 10;
    private float enemyDistance;

    public Slider HealthBar;

    public int count = 0;
    public Text countText;
    private int attackCounter = 0;
    public int attackTime = 10;

    private float HealthPoints = 100;
    private float MaxHealthPoints = 100;
    public Text HPText;

    public AudioSource footsteps;
    public AudioSource hit;
    public AudioSource buffpickup;

    // Use this for initialization
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        Animator = GetComponent<Animator>();
        spike = GameObject.FindGameObjectWithTag("spike");
        prefab = Resources.Load("Spike") as GameObject;
        SetCountText();
        SetHpText();
        
        
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    public int getCount() { return count; }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Player HP: " + HealthPoints);
        attackCounter++;
        
        if (HealthPoints > 0)
        {
            playerX = Input.GetAxis("Vertical") * speed;
            playerZ = Input.GetAxis("Horizontal") * speed;
            playerX *= Time.deltaTime;
            playerZ *= Time.deltaTime;

            transform.Translate(playerZ, 0, playerX);
            
            curPos = transform.position;
            if (curPos == lastPos)
            {
                Animator.SetBool("moving", false);
                footsteps.mute = true;
            }
            else
            {
                Animator.SetBool("moving", true);
                footsteps.mute = false;
            }
            lastPos = curPos;

            if (Input.GetKeyDown("escape"))
                Cursor.lockState = CursorLockMode.None;

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }

            //spike attack
            if (Input.GetMouseButtonDown(1))
            {
                spike = Instantiate(prefab) as GameObject;
                spike.transform.position = transform.position + new Vector3(0,4.5f,0) + Camera.main.transform.forward * 2;
                spike.transform.rotation = transform.rotation;
                spike.transform.Rotate(Vector3.right * 90);
                Rigidbody rb = spike.GetComponent<Rigidbody>();
                rb.velocity = Camera.main.transform.forward * 40;
            }

            //melee attack
            enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (Input.GetMouseButtonDown(0))
            {
                Animator.SetBool("attack", true);
                if (enemyDistance < AttackRadius)
                {
                    enemy.GetComponent<EnemyController>().Damaged();
                    
                    Debug.Log("Enemy damaged");
                }
            }
            else
            {
                Animator.SetBool("attack", false);
            }
            if (enemyDistance < AttackRadius && (attackCounter % attackTime == 0))
            {
               // Damaged();
            }
        }
        else
        {
            Animator.SetTrigger("Die");
            SceneManager.LoadScene("GameOver");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("buff"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            MaxHealthPoints = MaxHealthPoints + 50;
            HealthPoints = MaxHealthPoints;
            speed = speed + 2.0f;
            SetCountText();
            SetHpText();
            buffpickup.Play();
        }
        if (other.gameObject.CompareTag("trap"))
        {
            HealthPoints = HealthPoints - 10;
            hit.Play();
            SetHpText();
            Animator.SetBool("gethit", true);
            
        }
       
        if (other.gameObject.CompareTag("Enemy"))
        {
            {
                HealthPoints = HealthPoints - 8 * (7 - count);
                hit.Play();
                SetHpText();
            }           
        }
    }   

    void SetCountText()
    {
        countText.text = "Buff: " + count.ToString() + "/7";
    }
    void SetHpText()
    {
        if (HealthPoints > 0)
            HPText.text = "HP: " + HealthPoints.ToString() + "/" + MaxHealthPoints.ToString();
        else
            HPText.text = "HP: 0/100";
    }

   /* public void Damaged()
    {
        if (count < 7)
        {
            HealthPoints = HealthPoints - 20;
        }
        else
        {
            HealthPoints = HealthPoints - 2;
        }
    }*/

    void LateUpdate()
    {
        Animator.SetBool("gethit", false);
    }
}

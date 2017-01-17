using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 8.0f;
    private float playerX;
    private float playerZ;
    private float distToGround;
    private Vector3 curPos;
    private Vector3 lastPos;
    private bool moving = false;
    private Animator Animator;

    private bool isAttack;    

    private GameObject enemy;
    private float AttackRadius = 10;
    private float enemyDistance;

    public Slider HealthBar;

    private int count = 0;
    public Text countText;
    private int attackCounter = 0;
    public int attackTime = 10;

    private float HealthPoints = 100;
    public Text HPText;    
    

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        Animator = GetComponent<Animator>();

        SetCountText();

        SetHpText();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Player HP: " + HealthPoints);
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
                moving = false;
            }
            else
            {
                moving = true;
            }
            lastPos = curPos;

            if (Input.GetKeyDown("escape"))
                Cursor.lockState = CursorLockMode.None;

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Animator.SetTrigger("Spike");
            }

            enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (Input.GetMouseButtonDown(0))
            {
                if (enemyDistance < AttackRadius)
                {
                    enemy.GetComponent<EnemyController>().Damaged();
                    Debug.Log("Enemy damaged");
                }
            }
            if (enemyDistance < AttackRadius && (attackCounter % attackTime == 0))
            {
                Damaged();
            }
        }
        else
        {
            Animator.SetTrigger("Die");
            //SceneManager.LoadScene("GameOver");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("buff"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("trap"))
        {
            HealthPoints = HealthPoints - 10;
            SetHpText();
        }
    }   

    void SetCountText()
    {
        countText.text = "Buff: " + count.ToString() + "/7";
    }
    void SetHpText()
    {
        if (HealthPoints > 0)
            HPText.text = "HP: " + HealthPoints.ToString() + "/100";
        else
            HPText.text = "HP: 0/100";
    }

    public void Damaged()
    {
        if (count < 7)
        {
            HealthPoints = HealthPoints - 20;
        }
        else
        {
            HealthPoints = HealthPoints - 2;
        }
    }
}

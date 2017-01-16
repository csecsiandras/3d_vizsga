using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 8.0f;
    private float playerX;
    private float playerZ;
    private float distToGround;

    private bool isAttack;    

    private GameObject enemy;
    private Vector3 AttackRadius = new Vector3(5.0f, 0.0f, 5.0f);

    public Slider HealthBar;

    private int count;
    public Text countText;

    private float HealthPoints = 100;
    public Text HPText;    

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        enemy = GameObject.FindGameObjectWithTag("Enemy");

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
        playerX = Input.GetAxis("Vertical") * speed;
        playerZ = Input.GetAxis("Horizontal") * speed;
        playerX *= Time.deltaTime;
        playerZ *= Time.deltaTime;

        transform.Translate(playerZ, 0, playerX);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            if ((Mathf.Abs(enemy.transform.position.x - transform.position.x)) < AttackRadius.x && (Mathf.Abs(enemy.transform.position.y - transform.position.y)) < AttackRadius.y)
            {
                enemy.GetComponent<EnemyController>().Damaged();
                Debug.Log("Enemy damaged");
            }
        }
    }

    public void Damaged()
    {
        HealthPoints -= 1;
        HealthBar.value = HealthPoints;
        if (HealthPoints <= 0)
        {
            GameObject.Destroy(gameObject);
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
        HPText.text = "HP: " + HealthPoints.ToString() + "/100";
    }

    
}

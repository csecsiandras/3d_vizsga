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

    private Rigidbody rb;

    public Text HPText;
    private float HealthPoints = 100;

    private GameObject enemy;
    private Vector3 AttackRadius = new Vector3(5.0f, 0.0f, 5.0f);

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();

        HPText.text = HealthPoints.ToString();

        enemy = GameObject.FindGameObjectWithTag("Enemy");
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
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
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
        HPText.text = Mathf.Round(HealthPoints).ToString();
        if (HealthPoints <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}

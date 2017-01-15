using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float speed = 10.0f;
    private float playerX;
    private float playerZ;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        playerX = Input.GetAxis("Vertical") * speed;
        playerZ = Input.GetAxis("Horizontal") * speed;
        playerX *= Time.deltaTime;
        playerZ *= Time.deltaTime;

        transform.Translate(playerZ, 0, playerX);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void replay()
    {
        SceneManager.LoadScene("trapland");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

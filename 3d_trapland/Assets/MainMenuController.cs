using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("trapland");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

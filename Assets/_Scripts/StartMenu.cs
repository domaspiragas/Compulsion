using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		
	}

	public void StartGame()
	{

		Application.LoadLevel(Application.loadedLevel+1);

	}

	public void Quit()
	{
		Application.Quit ();
	}

}

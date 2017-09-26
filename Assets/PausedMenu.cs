using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausedMenu : MonoBehaviour {
	public GameObject PauseMenu;
	// Use this for initialization
	void Start () {
		PauseMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale =0;
			PauseMenu.SetActive (true);
			
		}
		
	}

	public void OnPauseButtonClicked()
	{
		//Only need this code if we're not mapping pause to a button
		/*Time.timeScale =0;
		PauseButton.enabled = false;
		PauseMenu.SetActive (true);*/ 
	}
	public void OnRestartButtonClicked()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
	public void OnResumeButtonClicked()
	{
		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.None;
		Time.timeScale =1;
		PauseMenu.SetActive (false);
	}
	public void OnExitButtonClicked()
	{
		Application.Quit ();
	}
	public void OnMainMenuExitClicked()
	{
		Application.LoadLevel ("PinakScene");
	}
}

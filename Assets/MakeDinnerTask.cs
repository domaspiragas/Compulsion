using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeDinnerTask : MonoBehaviour {
    

	public HoldAndSetPlates HSP;
	public HoldAndSetForksKnives HSF;
	public HoldAndSetCookies CC; // Caden Added for cooking the cookies
	public OCDEffectManager OEM;
	public SinkKnob SK;
	public OvenKnobs OK;
	public HoldAndSetSandwiches HSS;
    public bool OCDTasksActivated = false;
	public bool gg = false;

	private bool isTrue  = false;
	private bool isTrue2 = false;
	// Use this for initialization
	void Start () {
		GameObject h =  GameObject.FindGameObjectWithTag ("KeepThenSet");
		GameObject t = GameObject.Find ("pCylinder26");
		GameObject t1 = GameObject.Find ("SinkKnob");
		GameObject temp = GameObject.FindGameObjectWithTag ("OCDManager");
		GameObject g = GameObject.FindGameObjectWithTag ("KeepThenSet");
		GameObject go = GameObject.FindGameObjectWithTag ("KeepThenSet");
        GameObject go3 = GameObject.FindGameObjectWithTag("KeepThenSet"); // Caden Added for cooking the cookies
        HSP = g.GetComponent<HoldAndSetPlates> ();
		HSF = go.GetComponent<HoldAndSetForksKnives> ();
		SK = t1.GetComponent<SinkKnob> ();
		//OK = t.GetComponent<OvenKnobs> ();
		HSS = h.GetComponent<HoldAndSetSandwiches> ();
		CC = go3.GetComponent<HoldAndSetCookies>(); // Caden Added for cooking the cookies
		OEM = temp.GetComponent<OCDEffectManager>();
    }

	// Update is called once per frame
	void Update () {
		
		if(HSF.FKSet == true && HSP.setPlates == true)
		{
			GameObject.FindGameObjectWithTag ("OCDScrub").GetComponent<Text> ().enabled = false;
			GameObject.Find ("pCylinder26").GetComponent<BoxCollider> ().enabled = true;
			GameObject.Find ("pCylinder27").GetComponent<BoxCollider> ().enabled = true;
			GameObject.Find ("pCylinder28").GetComponent<BoxCollider> ().enabled = true;
			GameObject.Find ("pCylinder29").GetComponent<BoxCollider> ().enabled = true;
			if (isTrue == false) {
			StartCoroutine (MakeDinnerText ());
				}
		}
		if (OvenKnobs.isStoveChecked == true && SK.areHandsWashed == true) {
			GameObject.FindGameObjectWithTag ("OCDWash").GetComponent<Text> ().enabled = true;
			GameObject.FindGameObjectWithTag ("OCDWash").GetComponent<Text> ().text = "Make Dinner!";
			if (isTrue2 == false) {
				GameObject.Find ("UncookedCookies").GetComponent<BoxCollider> ().enabled = true;
				GameObject.Find ("2SandwichesPickUp").GetComponent<BoxCollider> ().enabled = true;
				isTrue2 = true;
			}
		}
		if (CC.areCookiesCooked == true && HSS.areSandwichesPlaced == true) {
			GameObject.FindGameObjectWithTag ("OCDWash").GetComponent<Text> ().enabled = false;
			StartCoroutine (GameOver ());

		}
	}

	public IEnumerator GameOver()
	{
		GameObject.FindGameObjectWithTag ("GameOver").GetComponent<Text> ().enabled = true;
		yield return new WaitForSeconds (4.5f);
		Application.LoadLevel (Application.loadedLevel-1);
	}
	public IEnumerator MakeDinnerText()
	{
		isTrue = true;

		StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text> ()));
		yield return null;
	}


	public IEnumerator FadeTextToFullAlpha(float t, Text i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
		while (i.color.a < 1.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
			yield return null;
		}
		StartCoroutine(WaitForTable());


	}
	public IEnumerator WaitForTable()
	{
		yield return new WaitForSeconds (1.5f);
		StartCoroutine(FadeTextToZeroAlpha (1f, GetComponent<Text> ()));

	}
		
	public IEnumerator FadeTextToZeroAlpha(float t, Text i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
		while (i.color.a > 0.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
			yield return null;
		}
		yield return new WaitForSeconds (1f);
		//Enables the OCD Tasks
		StartCoroutine (OCDTasks ());

	}
	public IEnumerator OCDTasks()
	{
		OEM.StartOCDTimer ();
		OCDTasksActivated = true;
		GameObject.FindGameObjectWithTag ("OCDWash").GetComponent<Text> ().enabled = true;
		GameObject.FindGameObjectWithTag ("OCDKnobs").GetComponent<Text> ().enabled = true;
		yield return null;
	}
}
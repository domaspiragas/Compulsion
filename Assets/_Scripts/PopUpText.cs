using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour {
	bool isTrue = false;
	public bool DisableTableText = false;
	public OCDEffectManager OEM;

	// Use this for initialization
	void Start () {
		
		GameObject g = GameObject.FindGameObjectWithTag ("OCDManager");
		OEM = g.GetComponent<OCDEffectManager> ();
		GameObject.FindGameObjectWithTag ("SetTable").GetComponent<Text> ().enabled = false;
		GameObject.FindGameObjectWithTag ("OCDScrub").GetComponent<Text> ().enabled = false;
        GameObject.Find("2PlatesPickup").GetComponent<BoxCollider>().enabled = false;
		GameObject.Find ("ForkKnifePickup").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("pCylinder26").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("pCylinder27").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("pCylinder28").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("pCylinder29").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("UncookedCookies").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("2SandwichesPickUp").GetComponent<BoxCollider> ().enabled = false;


	}
	
	// Update is called once per frame
	void Update () {
		//If any movement is triggered, we start a coroutine with a timer and then pop up the "Set Table" Text
		if (Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S)
			|| Input.GetKey (KeyCode.D)) {
			if (isTrue == false) {
				StartCoroutine (SetTableText ());
			}
		}
		if (DisableTableText == true) {
			GameObject.FindGameObjectWithTag ("SetTable").GetComponent<Text> ().enabled = false;
		}
	}
	public IEnumerator SetTableText()
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
		GameObject temp = GameObject.FindGameObjectWithTag ("SetTable");
		temp.GetComponent<Text> ().enabled = true;
		StartCoroutine (ScrubTableTask ());

	}
	public IEnumerator ScrubTableTask()
	{
		OEM.StartOCDTimer ();
		GameObject.FindGameObjectWithTag ("OCDScrub").GetComponent<Text> ().enabled = true;
		yield return null;
	}
}
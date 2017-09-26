using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWashHands : MonoBehaviour
{
	private GameObject gameManagerObject;
	private Coroutine cor;
	private Coroutine cor2;
	private FloatingText floatingtext;

	private void Start()
	{
		gameManagerObject = GameObject.Find("GameManager");
		floatingtext = transform.Find("3DText").GetComponent<FloatingText>(); // Gets the child object called 3DText.
	}

	public void Activate()
	{
		cor2 = StartCoroutine(OCDActiveLength());

		cor = StartCoroutine(StartOCDTextPulse());
	}

	public void CleanUp()
	{
		StopCoroutine(cor2); // Stop the OCDActiveLength process

		floatingtext.Deactivate();

		StopCoroutine(cor); // Stop the StartOCDTextPulse process
	}

	private IEnumerator OCDActiveLength()
	{
		while (true)
		{
			yield return new WaitForSeconds(10);

			floatingtext.Increase(); // The text pulse is in sync witht he IncreaseInfluence and therefore with the OCD effects.

			gameManagerObject.SendMessage("IncreaseInfluence", gameObject);
		}
	}

	private IEnumerator StartOCDTextPulse()
	{
		yield return new WaitForSeconds(10f); // Waits 10 seconds before initial start. Match this with starting IncreaseInfluence
		floatingtext.Activate();
	}
}

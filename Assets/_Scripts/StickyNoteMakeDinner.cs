using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will start the Make Dinner everyday task.
/// It will also start the OCD tasks for scrubbing the table and washing hands.
/// </summary>
public class StickyNoteMakeDinner : MonoBehaviour
{
    private GameObject gameManagerObject;
    private GameObject sponge;
    private GameObject kitchenTableCollider;
    private KitchenTableCollider kitchenTableColliderScript;
	private Coroutine cor;
	private Coroutine cor2;

	private FloatingText floatingtext;

    // Use this for initialization
    void Start ()
    {
        gameManagerObject = GameObject.Find("GameManager");
        sponge = GameObject.Find("Sponge");
        kitchenTableCollider = GameObject.Find("KitchenTableCollider");
        kitchenTableColliderScript = GameObject.Find("KitchenTableCollider").GetComponent<KitchenTableCollider>();
		floatingtext = transform.Find("3DText").GetComponent<FloatingText>(); // Gets the child object called 3DText.
    }
	
    public void Activate()
    {
        // ADD SOUND EFFECT FOR RIPPING OFF STICKY NOTE
        // Disable sticky note, but do not destroy it. It's needed by the GameManager.
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;

        // Enable box colliders needed.
        sponge.GetComponent<BoxCollider>().enabled = true;

        gameManagerObject.SendMessage("StartOCD", gameObject);

		cor2 = StartCoroutine(OCDActiveLength());

		cor = StartCoroutine(StartOCDTextPulse());
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

    public void CleanUp()
    {
        kitchenTableColliderScript.MaybeDisableCollider(); // Decrements the static integer

        if (KitchenTableCollider.colliderEnabledCount == 0) // Only disable the box collider if no other task is using it.
        {
            kitchenTableCollider.GetComponent<BoxCollider>().enabled = false;
        }

        gameManagerObject.SendMessage("EndOCD", gameObject);

		StopCoroutine(cor2); // Stop the OCDActiveLength process

		floatingtext.Deactivate();

		StopCoroutine(cor); // Stop the StartOCDTextPulse process
    }
}

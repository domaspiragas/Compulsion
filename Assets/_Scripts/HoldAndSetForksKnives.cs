using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAndSetForksKnives : MonoBehaviour
{
    private static bool forkAndKnife;
    private GameObject forkAndKnifeReference;
	public bool FKSet;

    // Use this for initialization
    void Start ()
    {
		FKSet = false;

        forkAndKnife = false;

        forkAndKnifeReference = GameObject.Find("ForkKnifePickup"); // Need a reference to the original forks and knives.
    }

    public void GrabThenSetDown()
    {
        if (gameObject.name == "ForkKnifePickup" && forkAndKnife == false)
        {
            SendMessage("PickUpAndHold", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        else if (gameObject.name == "KitchenTableCollider" && forkAndKnife == true)
        {
            SendMessage("SetDown", SendMessageOptions.DontRequireReceiver);

            Destroy(forkAndKnifeReference); // Destroy the original forks and knives.

            // Specail cases becasue the children for fork and knife start at index 2
            gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(3).GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(4).GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(5).GetComponent<MeshRenderer>().enabled = true;
            forkAndKnife = false;
			FKSet = true;
        }
    }

    public void SetVarTrue()
    {
        forkAndKnife = true;
    }
}

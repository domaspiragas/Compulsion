using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAndSetCookies : MonoBehaviour
{
    public static bool cookedCookies;
	public bool areCookiesCooked;
    private GameObject cookedCookiesReference;

    // Use this for initialization
    void Start()
    {
        cookedCookies = false;

        cookedCookiesReference = GameObject.Find("CookedCookies"); // Need a reference to the original Cooked Cookies.
    }

    public void GrabThenSetDown()
    {
        if (gameObject.name == "CookedCookies" && cookedCookies == false)
        {
            SendMessage("PickUpAndHold", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        else if (gameObject.name == "KitchenTableCollider" && cookedCookies == true)
		{
            SendMessage("SetDown", SendMessageOptions.DontRequireReceiver);
			areCookiesCooked = true;
            Destroy(cookedCookiesReference); // Destroy the original plates.
            gameObject.transform.GetChild(7).GetComponent<MeshRenderer>().enabled = true;
            cookedCookies = false;
        }
    }

    public void SetVarTrue()
    {
        cookedCookies = true;
    }
}

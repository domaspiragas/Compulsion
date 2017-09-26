using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookCookies : MonoBehaviour
{
    public static bool uncookedCookies;
    private GameObject uncookedCookiesReference;
    private Transform putInOvenPosition;

    public static bool cookedCookies;
    private GameObject cookedCookiesReference;

    public static bool startCookingCookies;

    private OvenDoor ovenDoorScript;

    public AudioClip foodDone;
    public float foodDoneVolume = 0.5f;
    private AudioSource audio;

    public float cookTime = 10.0f;

    // Use this for initialization
    void Start()
    {
        uncookedCookies = false;
        cookedCookies = false;
        startCookingCookies = false;

        uncookedCookiesReference = GameObject.Find("UncookedCookies"); // Need a reference to the original Uncooked Cookies.
        cookedCookiesReference = GameObject.Find("CookedCookies"); // Need a reference to the original Cooked Cookies.
        putInOvenPosition = GameObject.Find("UncookedCookiePlacement").transform; // The position of where to put the uncooked cookies in the oven.

        GameObject ovenDoor = GameObject.Find("OvenDoor");
        ovenDoorScript = ovenDoor.GetComponent<OvenDoor>();

        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (startCookingCookies == true && ovenDoorScript.IsOpen == false) // Wait unitl the cookies have been placed in the oven and the oven door is closed.
        {
            Invoke("CookiesAreDone", cookTime); // Let the cookies cook for 10 seconds.
            
            startCookingCookies = false; // Set to false so the sound doesn't play more than once. The If-Statement does not get executed again.
        }
    }

    public void GrabThenSetDown()
    {
        if (gameObject.name == "UncookedCookies" && uncookedCookies == false)
        {
            SendMessage("PickUpAndHold", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        else if (gameObject.name == "UncookedCookiePlacement" && uncookedCookies == true)
        {
            SendMessage("SetDown", SendMessageOptions.DontRequireReceiver);

            uncookedCookiesReference.transform.position = putInOvenPosition.position + new Vector3(-0.4f, -0.05f, 0f); // New position of the uncooked cookies. (in th oven). Needed an offset added to it.
            uncookedCookiesReference.transform.parent = putInOvenPosition; // Child the uncooked cookies to the collider so the player lets go of it.
            uncookedCookiesReference.name = "PlacedUncookedCookies"; // Rename the object so it cannot be picked up again right away.

            uncookedCookies = false;

            startCookingCookies = true;
        }
    }

    public void SetVarTrue()
    {
        uncookedCookies = true;
    }

    // Replaces the uncooked cookies with the cooked ones and playes the sound.
    private void CookiesAreDone()
    {
        if (ovenDoorScript.IsOpen == false) // To make sure the oven door stays closed in order for the cooking to stop.
        {
            cookedCookiesReference.GetComponent<MeshRenderer>().enabled = true;
            cookedCookiesReference.GetComponent<BoxCollider>().enabled = true;
            audio.PlayOneShot(foodDone, foodDoneVolume);
            //Debug.Log(gameObject.name);
            Destroy(uncookedCookiesReference);
        }
        else
        {
            Invoke("CookiesAreDone", cookTime); // Re-intantiate cooking the cookies for 10 seconds.
        }
    }
}

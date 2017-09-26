using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {

    private GameObject mainCamera;
    private bool carrying;

    public float floatDistance = 3f;
    public float floatSpeed = 3f;
    public float touchRange = 2f;


    public LayerMask interactable;
    public LayerMask activatableMask;
    public GameObject reticle;


    private Ray ray;
    private RaycastHit hit;
    private GameObject objectHeld;
    private Vector2 screencenter;

    // Use this for initialization
    void Start ()
    {
        mainCamera = GameObject.FindWithTag("MainCamera"); // Sets the main camera to variable mainCamera by finding its tag 
        screencenter = new Vector2(Screen.width/2, Screen.height/2);
    }


    private void Update()
    {
        ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screencenter);
        if(Physics.Raycast(ray, out hit, touchRange, interactable))
        {
            reticle.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                // Switch for tags
                switch (hit.transform.tag) // Caden Added
                {
                    case "KeepThenSet": // For picking things up and setting them down
                        // Calls the Grab function in the script attached to this gameObject
                        hit.transform.gameObject.SendMessage("GrabThenSetDown", SendMessageOptions.DontRequireReceiver);
                        // Calls the Scrub function in the script attached to this gameObject
                        hit.transform.gameObject.SendMessage("Scrub", SendMessageOptions.DontRequireReceiver);
                        break;
                    case "Activate": // For activating things. E.g. open drawers.
                        hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                        break;
                }
                
            }
        }
        else
        {
            reticle.SetActive(false);
        }    
    }
}

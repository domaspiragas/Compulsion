using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAndSetBooks : MonoBehaviour
{
    public static bool coffeeTableBooks;
    private GameObject coffeeTableBooksReference;
    private BoxCollider booksCoffeeTablePlaceCollider;
    private StickyNoteWaterPlants stickyNoteWaterPlantsScript;

    // Use this for initialization
    void Start()
    {
        coffeeTableBooks = false;

        coffeeTableBooksReference = GameObject.Find("BooksCoffeeTablePickUp"); // Need a reference to the original coffee table books.
        booksCoffeeTablePlaceCollider = GameObject.Find("BooksCoffeeTablePlace").GetComponent<BoxCollider>();
        stickyNoteWaterPlantsScript = GameObject.Find("StickyNoteWaterPlants").GetComponent<StickyNoteWaterPlants>();
    }

    public void GrabThenSetDown()
    {
        if (gameObject.name == "BooksCoffeeTablePickUp" && coffeeTableBooks == false)
        {
            SendMessage("PickUpAndHold", gameObject, SendMessageOptions.DontRequireReceiver);

            booksCoffeeTablePlaceCollider.enabled = true; // Enable the placement box collider.
            //Debug.Log ("Here!");
        }
        else if (gameObject.name == "BooksCoffeeTablePlace" && coffeeTableBooks == true)
        {
            //Debug.Log ("Here!!!");
            SendMessage("SetDown", SendMessageOptions.DontRequireReceiver);

            Destroy(coffeeTableBooksReference); // Destroy the original coffe table books.

            gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(3).GetComponent<MeshRenderer>().enabled = true;
            coffeeTableBooks = false;

            booksCoffeeTablePlaceCollider.enabled = false; // Disable the placement box collider.

            stickyNoteWaterPlantsScript.CleanUp();
        }
    }

    public void SetVarTrue()
    {
        coffeeTableBooks = true;
    }
}

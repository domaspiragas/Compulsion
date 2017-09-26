using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampOnAndOff : MonoBehaviour
{
    public int numberOfTimesNeeded = 3; // Number needed to stop OCD

    private static int timesTurnedOnAndOff;
    private static bool isOn;

    private GameObject lampString;

    // Use this for initialization
    void Start()
    {
        timesTurnedOnAndOff = 0;

        isOn = false;

        lampString = gameObject.transform.GetChild(2).gameObject;
    }


    public void Activate()
    {
        if (isOn == false)
        {
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
            timesTurnedOnAndOff++;
            isOn = true;
        }
        else
        {
            gameObject.transform.GetChild(3).gameObject.SetActive(false);
            timesTurnedOnAndOff++;
            isOn = false;
        }

        if (timesTurnedOnAndOff == (numberOfTimesNeeded * 2))
        {
            timesTurnedOnAndOff = 0;
            Debug.Log("Complete");

            // STOP OCD EFFECTS HERE (SEND MESSAGE BACK TO CAMERON TO STOP IT??)
        }
    }
}

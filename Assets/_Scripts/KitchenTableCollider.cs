using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenTableCollider : MonoBehaviour
{
    [HideInInspector]
    public static int colliderEnabledCount;

	// Use this for initialization
	void Start ()
    {
        colliderEnabledCount = 0;
    }

    /// <summary>
    /// Always enable the box collider when needed.
    /// </summary>
    public void EnableCollider()
    {
        colliderEnabledCount++;

        if (gameObject.GetComponent<BoxCollider>().enabled == false) // Only enable if it isn't already
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    /// <summary>
    /// This function is always called when the collider might need to be disabled.
    /// Only disable the collider if no other task is using it.
    /// The colliderEnabledCount static variable is checked in other classes, 
    ///     if the integer is equal to 0 it means no other task is using it,  
    ///     the collider can then be disabled from that class.
    /// </summary>
    public void MaybeDisableCollider()
    {
        colliderEnabledCount--;
    }
}

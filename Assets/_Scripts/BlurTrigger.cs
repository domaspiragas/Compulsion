using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurTrigger : MonoBehaviour {

    public GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        StartCoroutine(mainCamera.GetComponent<Blur>().StartBlurTimer(1));
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
			//StartCoroutine(mainCamera.GetComponent<Blur>().ProgressiveBlur());
        }
    }
}

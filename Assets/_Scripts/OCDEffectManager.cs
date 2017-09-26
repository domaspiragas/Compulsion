using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OCDEffectManager : MonoBehaviour {

    public GameObject playerCamera;
    public GameObject playerAudio;
	public bool performOCDEffects = false;
    private float timeSinceLastOCDAttack = 0;
    private float timeUntilIncreasedEffects = 60;
    private bool activeAttack = false;
    private float timeUntilEffect = 10f;
    private float timeSinceLastEffect = 0;

    void Start ()
    {
        playerCamera = GameObject.FindWithTag("MainCamera");
        //Remember to add this tag
        playerAudio = GameObject.FindWithTag("PlayerAudio");

    }
	// Update is called once per frame
	void Update ()
    {
        // bool that controls if OCD effects are active
        if (performOCDEffects)
        {
            // will start an OCD attack every timeUntilIncreasedEffects seconds if one is not actively occurring.
            timeSinceLastOCDAttack += Time.deltaTime;
            timeSinceLastEffect += Time.deltaTime;
			//Debug.Log (timeSinceLastOCDAttack);

            if (!activeAttack)
            {
                if (timeSinceLastEffect >= timeUntilEffect)
                {
					Debug.Log ("OCD attack starting");
                    StartOCDAttack();
                    timeSinceLastEffect = 0;
                }
                activeAttack = true;
                timeSinceLastOCDAttack = 0;
            }
            // if timeUntilIncreasedEffects seconds pass and player hasn't satisfied OCD urge, increase effect rate
            else if (timeSinceLastOCDAttack >= timeUntilIncreasedEffects && activeAttack)
            {
                if (timeUntilEffect >= 4)
                {
                    timeUntilEffect /= 2;
                }
                if (timeSinceLastEffect >= timeUntilEffect)
                {
                    StartOCDAttack();
                    timeSinceLastEffect = 0;
                }
                timeSinceLastOCDAttack = 0;
            }
            else if (timeSinceLastEffect >= timeUntilEffect && activeAttack)
            {
                StartOCDAttack();
                timeSinceLastEffect = 0;
            }
        }
	}

    private void StartOCDAttack()
    {
        StartCoroutine(playerCamera.GetComponent<Blur>().BlurPulse());
        playerAudio.GetComponent<PlayerSounds>().PlayBreathingSound();
    }
    private void ResetEffects()
    {
        timeUntilEffect = 10;
        activeAttack = false;
        timeSinceLastOCDAttack = 0;
        timeSinceLastEffect = 0;
    }
    public void StartOCDTimer()
    {
        performOCDEffects = true;
    }
    public void StopOCDTimer()
    {
        performOCDEffects = false;
        ResetEffects();
    }
}

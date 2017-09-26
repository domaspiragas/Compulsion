using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashHands : MonoBehaviour
{
    private SinkKnob sinkKnobScript;
    private bool stopWashingHands;

    private OCDEffectManager blur;

    public AudioClip washingHands;
    public float washingHandsVolume = 0.5f;
    private AudioSource audio;

	private RandomWashHands randomWashHandsScript;

    // Use this for initialization
    void Start ()
    {
        GameObject sinkKnob = GameObject.Find("SinkKnob");
        sinkKnobScript = sinkKnob.GetComponent<SinkKnob>();
        stopWashingHands = false;
        audio = GetComponent<AudioSource>();
		randomWashHandsScript = GameObject.Find("RandomWashHands").GetComponent<RandomWashHands>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (stopWashingHands == false && sinkKnobScript.IsOpen == true)
        {
            audio.PlayOneShot(washingHands, washingHandsVolume);

            stopWashingHands = true;

			randomWashHandsScript.CleanUp();
        }
	}
}

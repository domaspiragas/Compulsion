using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    private AudioSource source;
    public AudioClip[] breathing = new AudioClip[4];
	// Use this for initialization
	void Start ()
    {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void PlayBreathingSound()
    {
        source.pitch = Random.Range(.95f, 1.05f);
        source.PlayOneShot(breathing[Random.Range(0, 3)], .7f);
    }
}

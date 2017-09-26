using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinkKnob : MonoBehaviour {

    public float speed = 90f;
	//public HoldAndSetPlates HSP;
	//public HoldAndSetForksKnives HSF;
	//public OCDEffectManager OEM;
	//public MakeDinnerTask MDT;
	public bool areHandsWashed = false;   // Can't comment out due to dependency issues
	//public OvenKnobs OK;

    private bool isOpen;
    private bool ocdActive;
    private Quaternion closed;
    private Quaternion open;
    private Quaternion target;
    private Coroutine cor;
    private ParticleSystem water;
    private Coroutine cor2;
    private GameObject gameManager;
    private FloatingText floatingtext;


    // isOPen getter for Caden's CookCookies script.
    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
    }

    // Use this for initialization
    public void Start()
    {
		areHandsWashed = false;
		//GameObject temp = GameObject.FindGameObjectWithTag ("Activate");
		//OK = temp.GetComponent<OvenKnobs> ();
		//GameObject g = GameObject.FindGameObjectWithTag ("OCDManager");
		//OEM = g.GetComponent<OCDEffectManager> ();
		//GameObject go = GameObject.FindGameObjectWithTag ("Task2");
		//MDT = go.GetComponent<MakeDinnerTask> ();
        isOpen = false;
        ocdActive = true;
        water = GameObject.Find("WaterParticleEffect").GetComponent<ParticleSystem>();
        water.Stop();
        closed = transform.rotation;
        open = Quaternion.AngleAxis(180f, Vector3.forward) * transform.rotation;
        target = closed;
        gameManager = GameObject.Find("GameManager");
        floatingtext = transform.Find("3DText").GetComponent<FloatingText>();
    }

    public void Activate()
    {
        if (cor != null)
            StopCoroutine(cor);

        if (isOpen)
        {
            target = closed;
            StopCoroutine(cor2);
            if (ocdActive)
            {
                gameManager.SendMessage("EndOCD", gameObject);
                ocdActive = false;
                floatingtext.Deactivate();
            }
        }
        else
        {
            target = open;
            cor2 = StartCoroutine(OCDTimer());
        }

        cor = StartCoroutine("Swing");
        isOpen = !isOpen;
    }

    public IEnumerator Swing()
    {
        Debug.Log(Quaternion.Angle(transform.rotation, target));
        while (Quaternion.Angle(transform.rotation, target) != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * speed);
            yield return null;
        }

		if (isOpen) {
			water.Play ();
		}
		else {
			water.Stop ();
			//if (MDT.OCDTasksActivated == true) {
			//	Debug.Log ("Hands have been washed, delete this shitty text");
			//	GameObject.FindGameObjectWithTag ("OCDWash").GetComponent<Text> ().enabled = false;
			//	areHandsWashed = true;
			//} else
			//	areHandsWashed = false;
		}	
    }

    private IEnumerator OCDTimer()
    {
        yield return new WaitForSeconds(10f);
        if (isOpen)
        {
            ocdActive = true;
            gameManager.SendMessage("StartOCD", gameObject);
            floatingtext.Activate();
        }

        yield return new WaitForSeconds(15f);
        while (isOpen)
        {
            gameManager.SendMessage("IncreaseInfluence", gameObject);
            floatingtext.Increase();
            yield return new WaitForSeconds(15f);

        }
    }
}

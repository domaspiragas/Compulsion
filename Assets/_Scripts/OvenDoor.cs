using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour {

    public float speed = 60f;

    private bool isOpen;
    private bool ocdActive;
    private Quaternion closed;
    private Quaternion open;
    private Quaternion target;
    private Coroutine cor;
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
    private void Start()
    {
        isOpen = false;
        ocdActive = true;
        closed = transform.rotation;
        open = Quaternion.AngleAxis(45f, Vector3.forward) * transform.rotation;
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

    private IEnumerator Swing()
    {
        Debug.Log(Quaternion.Angle(transform.rotation, target));
        while (Quaternion.Angle(transform.rotation, target) != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * speed);
            yield return null;
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

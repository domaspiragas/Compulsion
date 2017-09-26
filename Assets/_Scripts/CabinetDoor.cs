using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetDoor : MonoBehaviour {

    public float speed = 60f;
    public bool swingClockwise;

    private bool isOpen;
    private bool ocdActive;
    private Transform parent;
    private Quaternion closed;
    private Quaternion open;
    private Quaternion target;
    private Coroutine cor;
    private Coroutine cor2;
    private GameObject gameManager;
    private FloatingText floatingtext;

    // Use this for initialization
    private void Start()
    {
        isOpen = false;
        ocdActive = true;
        parent = transform.parent;
        closed = parent.rotation;
        if(swingClockwise)
            open = Quaternion.AngleAxis(80f, Vector3.up) * parent.rotation;
        else
            open = Quaternion.AngleAxis(-80f, Vector3.up) * parent.rotation;

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
        while(Quaternion.Angle(transform.rotation, target) != 0)
        {
            parent.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * speed);
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

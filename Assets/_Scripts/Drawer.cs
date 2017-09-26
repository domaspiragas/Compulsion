using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour {

	public float speed = 1f;
	public bool moveX = true;

	private bool isOpen;
    private bool ocdActive;
	private Vector3 closed;
	private Vector3 open;
	private Vector3 target;
	private Coroutine cor;
    private Coroutine cor2;
    private GameObject gameManager;
    private FloatingText floatingtext;

	// Use this for initialization
	private void Start()
	{
		isOpen = false;
        ocdActive = true;
		closed = transform.position;
		if (moveX)
			open = transform.position + (Vector3.left * 0.5f);
		else
			open = transform.position + (Vector3.forward * 0.5f);
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

        cor = StartCoroutine("Move");
        isOpen = !isOpen;
    }

	private IEnumerator Move()
	{
		while (Vector3.Distance(transform.position, target) != 0)
		{
			if (Vector3.Distance (transform.position, target) > Time.deltaTime * speed)
				transform.Translate ((target - transform.position).normalized * Time.deltaTime * speed);
			else
				transform.position = target;
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

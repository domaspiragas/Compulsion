using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    [SerializeField]
    private float pulseSpeed;

    private Transform self;
    private Transform player;
    private MeshRenderer mesh;
    private Vector3 originalScale;
    private Vector3 minSizeScale;
    private Vector3 maxSizeScale;
    private float currentSpeed;

    // Use this for initialization
    private void Start()
    {
        self = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
        originalScale = self.localScale;
        maxSizeScale = originalScale * 1.5f;
        minSizeScale = originalScale;
        currentSpeed = pulseSpeed;
    }

    private void Update()
    {
        if (mesh.enabled)
        {
            self.LookAt(player);
        }
    }

    public void Activate()
    {
        if (!mesh.enabled)
        {
            currentSpeed = pulseSpeed;
            self.localScale = originalScale;
            minSizeScale = originalScale;
            maxSizeScale = originalScale * 1.5f;
            mesh.enabled = true;
            StartCoroutine(Pulse());
        }
    }

    public void Increase()
    {
        if (minSizeScale.x < (originalScale.x * 2.0f))
        {
            minSizeScale += 0.2f * originalScale;
            maxSizeScale += 0.2f * originalScale;
            currentSpeed *= 1.5f;
        }

        
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        mesh.enabled = false;
    }

    private IEnumerator Pulse()
    {
        bool isGrowing = true;
        bool tickTock = false;

        while (true)
        {
            if (isGrowing)
            {
                self.localScale = Vector3.Slerp(self.localScale, maxSizeScale*1.05f, currentSpeed * Time.deltaTime);
                if (self.localScale.x >= maxSizeScale.x)
                {
                    isGrowing = false;
                }
            }
            else
            {
                self.localScale = Vector3.Slerp(self.localScale, originalScale*0.95f, currentSpeed * Time.deltaTime);
                if (self.localScale.x <= originalScale.x)
                {
                    isGrowing = true;
                    //if(tickTock)
                    //    yield return new WaitForSeconds(0.75f);
                    //tickTock = !tickTock;
                }
            }

            

            yield return null;
        }
    }
}

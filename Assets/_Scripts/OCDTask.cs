using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OCDTask : MonoBehaviour{

    public int Influence { get; set; }
    public GameObject Owner { get; private set; }

    public OCDTask(GameObject owner)
    {
        Influence = 1;
        Owner = owner;
    }
}

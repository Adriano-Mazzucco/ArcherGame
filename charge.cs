using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charge : MonoBehaviour
{
    private float size = 100;

    void Update()
    {
        float charge = (GameObject.Find("erika_archer_bow_arrow").GetComponent<archerMove>()).charge;
        
        this.transform.localScale = new Vector3(charge/30, 0.3f, 7);

    }
}

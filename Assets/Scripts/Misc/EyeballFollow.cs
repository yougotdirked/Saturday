using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballFollow : MonoBehaviour {

    GameObject target;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("MainCamera");

        if (target == null)
        {
            Debug.Log("The eyes of the world have nothing to look at");
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target.transform);
	}
}

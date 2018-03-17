using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= -5f)
        {
            Destroy(gameObject);
        }
	}
}

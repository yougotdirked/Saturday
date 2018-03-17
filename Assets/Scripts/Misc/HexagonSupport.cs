using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonSupport : MonoBehaviour {

    public GameObject supportBlock;

    bool supported = false;
    // Update is called once per frame

    private void Start()
    {
        supported = false;
    }

    void Update () {
		if (transform.position.y > 0 && !supported)
        {
            GameObject support = GameObject.Instantiate(supportBlock, transform);
            support.transform.position = transform.position;
            support.transform.Translate(Vector3.down * transform.lossyScale.y * 2);

            supported = true;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public float score;
    public float currentPower;
    public float maxPower;
    public ItemPlacement selectedItem;

    public int platformcount;

    Vector3 spawn;
    Vector3 playerPosition;

	// Use this for initialization
	void Start () {
        spawn = transform.position;
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - spawn).magnitude >= score)
        {
            score = (transform.position - spawn).magnitude;
        }
	}

    public void setPlayerPosition(Vector3 pos)
    {
        playerPosition = pos;
    }
}

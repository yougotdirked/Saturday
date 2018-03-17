using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playtest_Killsript : MonoBehaviour {
    public bool UnlimitedLives;

    Vector3 respawnLocation;

	// Use this for initialization
	void Start () {
        respawnLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= -3)
        {
            respawn();
        }
	}

    void respawn()
    {
        transform.position = respawnLocation;
    }
}

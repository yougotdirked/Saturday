using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    bool isTriggered;
    public float floatingHeight = 1;
    
	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        //Just player triggers the pickups.
        if (other.gameObject.tag == "Player")
        {
            triggered(other.gameObject);
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void triggered(GameObject player)
    {
        //trigger logic in subclass
    }
}

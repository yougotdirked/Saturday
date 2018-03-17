using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalController : MonoBehaviour {

    public GameObject player;
    public Text tripDistance, powerLevel, maxPower, currentItem;

    PlayerData playerdata;

	// Use this for initialization
	void Start () {
        playerdata = player.GetComponent<PlayerData>();
	}
	
	// Update is called once per frame
	void Update () {
        tripDistance.text = "Trip Distance: " + (int) playerdata.score;
        powerLevel.text = "Power: " + (int) playerdata.currentPower;
        maxPower.text = "Power Limit: " + (int) playerdata.maxPower;
        currentItem.text = "Current Item: " + playerdata.selectedItem.itemName;
	}
}

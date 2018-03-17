using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOrb : Pickup {

    public float powerAmount;

    PlayerData playerdata;

    public override void triggered(GameObject player)
    {
        playerdata = player.GetComponent<PlayerData>();
        playerdata.currentPower += powerAmount;

        if (playerdata.currentPower > playerdata.maxPower)
        {
            playerdata.maxPower += playerdata.currentPower - playerdata.maxPower;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 100);
    }
}

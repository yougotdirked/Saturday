using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlacement : ItemPlacement {

    public override void placeItem(Vector3 origin)
    {
        Debug.Log("create platform");
        GameObject newPlatform = GameObject.Instantiate(gameObject);
        newPlatform.transform.position = origin - (Vector3.up * 1f);

        Platform platformscript = newPlatform.GetComponent<Platform>();
        //platformscript.moveSpeed = 2f;
        platformscript.direction = Camera.main.transform.forward;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlacement : ItemPlacement {

    public override void placeItem(Vector3 origin)
    {
        Debug.Log("create box");
        GameObject newBox = GameObject.Instantiate(gameObject);

        Vector3 offset = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * 2;

        newBox.transform.position = origin + offset + Vector3.up;
    }
}

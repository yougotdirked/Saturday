using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : MonoBehaviour {

    public int itemCost;
    public string itemName;

    //virtual placing algorithm
    public virtual void placeItem(Vector3 origin)
    {

    }
}

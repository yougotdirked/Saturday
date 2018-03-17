using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreate : MonoBehaviour {

    public float platformCost = 15f;

    public GameObject[] itemlist;

    PlayerData playerdata;
    Camera aimCamera;
    int itemIterator;

    Rigidbody rigidbody_;

	// Use this for initialization
	void Start () {
        playerdata = GetComponent<PlayerData>();
        rigidbody_ = GetComponent<Rigidbody>();
        aimCamera = Camera.main;
        itemIterator = 0;
        playerdata.selectedItem = itemlist[itemIterator].GetComponent<ItemPlacement>();
	}

    public void createSelected()
    {
        Debug.Log("creating placable");
        ItemPlacement itemplacer = itemlist[itemIterator].GetComponent<ItemPlacement>();
        if (itemplacer != null && playerdata.currentPower >= itemplacer.itemCost)
        {
            itemplacer.placeItem(transform.position);
            playerdata.currentPower -= itemplacer.itemCost;
        }
    }

    public void nextItem()
    {
        if (itemIterator == itemlist.Length - 1)
        {
            itemIterator = 0;
        }
        else
            itemIterator++;

        playerdata.selectedItem = itemlist[itemIterator].GetComponent<ItemPlacement>();
    }

    public void prevItem()
    {
        if (itemIterator == 0)
            itemIterator = itemlist.Length - 1;
        else
            itemIterator--;

        playerdata.selectedItem = itemlist[itemIterator].GetComponent<ItemPlacement>();
    }
}

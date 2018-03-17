using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGeneration : MonoBehaviour {

    public int size;
    public float minDistance = 1;
    public float distancejitter = 2;
    public float blockSize = 1;
    public float sizeJitter = 0;

    [HideInInspector ]public Island island;

    public Vector3 spawnDirection;

    public virtual void Start()
    {
        island = GetComponent<Island>();
    }

    public virtual void formBody(GameObject buildingblock, GameObject[] alreadyPlaced)
    {

    }
}

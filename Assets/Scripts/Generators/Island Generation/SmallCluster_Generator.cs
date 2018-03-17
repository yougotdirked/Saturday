using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCluster_Generator : IslandGeneration {
    // Use this for initializations
    public float directionJitter;
    public float heightJitter = 3;

    public override void Start() {
        base.Start();

        //size = Random.Range(2, 5);
    }

    // Update is called once per frame
    void Update() {

    }

    public override void formBody(GameObject buildingblock, GameObject[] alreadyPlaced)
    {
        for (int i = 0; i < size; i++)
        {
            setNewDirection();
            float tempDistance = newDist();
            island.body[i] = GameObject.Instantiate(buildingblock, gameObject.transform);
            if (i == 0)
            {
                //basecase
                //set correct values in newObject
                island.body[i].transform.position = island.transform.position + (Vector3.up * island.height);
                island.body[i].GetComponent<BuildingBlock>().setEndHeight(island.height);
                island.body[i].GetComponent<BuildingBlock>().setIsland(island);
                island.body[i].transform.localScale = new Vector3(1, 1, 1) * blockSize;
            }
            else if (i > 0)
            {
                //place new building blocks based on previous objects.
                Debug.Log(island.body[i]);
                Debug.Log(i); 
                island.body[i].transform.position = island.body[i - 1].transform.position + (spawnDirection * tempDistance) + (Vector3.up * Random.Range(0, heightJitter));
                island.body[i].GetComponent<BuildingBlock>().setEndHeight(island.height + Random.Range(-3, 3));
                island.body[i].GetComponent<BuildingBlock>().setIsland(island);
                island.body[i].transform.localScale = new Vector3(1, 1, 1) * (blockSize + Random.Range(-sizeJitter, sizeJitter));
            }
        }
    }

    float newDist()
    {
        return minDistance + Random.Range(-distancejitter, distancejitter);
    }

    void setNewDirection()
    {
        spawnDirection = new Vector3(Random.Range(-directionJitter, directionJitter), 0, Random.Range(-directionJitter, directionJitter)).normalized;
    }
}

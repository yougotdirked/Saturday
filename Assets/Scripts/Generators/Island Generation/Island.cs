using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour {

/* Islands form clusters of building blocks
 * They determine how big the clusters should be and what blocks they should consist of.
 * They should contain the algorithm of placing the individual blocks
 * 
 * OPEN QUESTIONS
 * When should a new island be created
 * When should a new buildig block be created?
 * Where should the new buidling block be created?
 * 
 * maybe divide further with an island which has an island generation script (which will determine the shape for example)
 */

    public float height;
    public float radius = 10; //Radius in which new building blocks can be placed.
    public float spawnRadius = 50; //Radius in which new islands can be placed.
    public float newIslandThreshhold = 20;
    public GameObject nextIsland;

    public GameObject mainBlock; //The block that the island's 'body' will be formed out
    [HideInInspector] public IslandGeneration islandgen; //the specefics of how the island should be formed and how it should create new ones

    public GameObject[] body; //collection of blocks that form its body.

    bool formedBody = false;
    bool newIslandCreated = false;
    [HideInInspector] public bool blockIsGrowing = false;

    float islandGrowth = 0; //variable to determine if new islands should be placed.
    GameObject player;

	// Use this for initialization
	void Start () {
        //temp solution for duplicates?
        deleteBody();

        player = GameObject.Find("Player");
        islandgen = GetComponent<IslandGeneration>();
        body = new GameObject[islandgen.size];
        transform.Translate(Vector3.down * transform.position.y); //Set it to water level, height of the island should not matter.

        islandgen.formBody(mainBlock, body);
	}
	
	// Update is called once per frame
	void Update () {
        if (blockIsGrowing)
        {
            growAll();
            if (islandGrowth < newIslandThreshhold)
            {
                islandGrowth += Time.deltaTime;
            }

            else if (!newIslandCreated && islandGrowth >= newIslandThreshhold)
            {
                createNewIsland();
                newIslandCreated = true;
            }
        }
	}

    void deleteBody()
    {
        foreach (GameObject g in body)
        {
            Destroy(g);
        }
    }

    private void LateUpdate()
    {
        blockIsGrowing = false;
    }

    void growAll()
    {
        foreach (GameObject g in body)
        {
            g.GetComponent<BuildingBlock>().islandIsGrowing = true;
        }
    }

    void createNewIsland()
    {
        Vector3 creationDirection = (player.transform.position - transform.position).normalized;
        Vector3 creationPos = transform.position + (creationDirection * spawnRadius);

        GameObject.Instantiate(nextIsland, creationPos, Quaternion.identity);

        Debug.Log("creating new island at: " + creationPos);
    }
}
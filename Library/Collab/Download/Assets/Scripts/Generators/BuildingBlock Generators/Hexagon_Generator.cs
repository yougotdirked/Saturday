using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon_Generator : Generation
{
    GameObject[] children = new GameObject[6];

    public GameObject pickupPrefab;
    public int relativeIndex, parentIndex;
    public float heightVariation;
    public float distanceVariation;
    public float sizeVariation;
    public float moveSpeed = 1f;

    GameObject pickup;
    Vector3 targetpos;
    bool pickupGenerated = false;

    public void Start()
    {
        //play start thingey (make hexagons rise from the water)
        rise();
    }

    public void Update()
    {
        if (transform.position.y < targetpos.y)
        {
                moveToTarget(targetpos);
        }
        else if (pickup == null && !pickupGenerated)
        {
            pickup = GameObject.Instantiate(pickupPrefab);
            pickup.transform.Translate(transform.position + (Vector3.up * transform.localScale.y) + Vector3.up);
            pickupGenerated = true;
        }
    }

    void rise()
    {
        targetpos = transform.position;
        transform.position += (Vector3.down * (transform.position.y + 10));
    }

    void moveToTarget(Vector3 t)
    {
        Vector3 dir = targetpos - transform.position;

        if (dir.magnitude < 0.2f)
        {
            dir.Normalize();
            dir *= 0.2f;
        }
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    public override void newChild(Vector3 inputLoc, int c)
    {
        int next = c + 1;
        if (next == 6)
            next = 0;

        int prev = c - 1;
        if (prev == -1)
            prev = 5;

        if (c < maxComplexity)
        {
            //TODO: rebuild with raycasts.

            children[c] = GameObject.Instantiate(child);
            float distance = Random.Range(5, distanceVariation);
            float size = Random.Range(1, sizeVariation);
            int angle = (30 + (c * 60));

            children[c].GetComponent<Transform>().Rotate(Vector3.up, angle);
            children[c].GetComponent<Transform>().Translate(Vector3.forward * 3f * size);
            children[c].GetComponent<BuildingBlock>().isGenerating = false;

            children[c].GetComponent<Transform>().Rotate(Vector3.up, -angle);
            children[c].GetComponent<Transform>().Translate(Vector3.up * Random.Range(0, (heightVariation) + heightMod(size)));

            //make sure it doesn't generate under water
            if (children[c].GetComponent<Transform>().position.y < 0)
            {
                Vector3 newPos = children[c].GetComponent<Transform>().position;
                newPos.y = 0;
                children[c].GetComponent<Transform>().position = newPos;
            }
            children[c].GetComponent<Transform>().localScale = (new Vector3(1, 1, 1));
            children[c].GetComponent<Transform>().localScale *= size;

            //delete underlying objects
            Collider[] closeObjects = Physics.OverlapCapsule(children[c].transform.position, children[c].transform.position + Vector3.up * 2, 1);
            Collider childCollider = children[c].GetComponent<Collider>();

            for (int x = 0; x < closeObjects.Length; x++)
            {
                if (closeObjects[x] != childCollider)
                {
                    GameObject closeobject = closeObjects[x].gameObject;
                    if (childCollider.bounds.Intersects(closeObjects[x].bounds))
                    {
                        if (closeobject.GetComponent<Hexagon_Generator>() != null || closeobject.tag == "Player")
                        {
                            //fix removal now that they rise from the water..
                            Destroy(children[c]);
                            GetComponent<BuildingBlock>().failedCreation();
                            GetComponent<BuildingBlock>().stopGenerating();
                        }

                        if (closeobject.GetComponent<PowerOrb>() != null)
                        {
                            Destroy(closeobject);
                        }
                    }
                }
            }

            //Create power pickups on top.
        }
        else
            GetComponent<BuildingBlock>().isGenerating = false;
    }

    float heightMod(float size)
    {
        if (size > 3)
        {
            return 20;
        }
        else
            return 0;
    }

    //Generation of extra objects to make the world more interesting ( after they have already generated their own children)
    public override void extraStuff(float growthrate, float extraThreshhold)
    {
        //hard coded shit.
        float g = extraGrow + growthrate;

        if (extraGrow >= extraThreshhold && !hasExtra)
        {
            Vector3 offset = transform.position + (Vector3.up * transform.localScale.y / 2.5f);
            GameObject extraChild = GameObject.Instantiate(extra, offset, new Quaternion(0, 0, 0, 0));
            extraChild.transform.Rotate(Vector3.up, Random.Range(-180, 180));
            extraChild.transform.Rotate(Vector3.right, Random.Range(0, 15));
            extraChild.transform.localScale = transform.localScale / 2;
            Vine_Generator loop = extraChild.GetComponent<Vine_Generator>();
            if (loop != null)
            {
                Debug.Log("new loop");
                loop.radius = Random.Range(5, 20);
                loop.maxComplexity = Random.Range(10, 30);
                loop.isRoot = false;
            }
            hasExtra = true;
        }
        else
            extraGrow = g;
    }

}
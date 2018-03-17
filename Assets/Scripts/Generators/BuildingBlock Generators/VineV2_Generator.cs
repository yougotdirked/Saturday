using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineV2_Generator : Generation {

    public Vector3 direction; //direction of the overall vine -> use to place next child;

    //public Vector3 growthDirection; //direction the individual piece will move

    public float growthrate;
    public bool isRoot = false;
    public GameObject next, parent;
    public Vector3 targetPos;
    public float size;
    public float maxSize = 3f;
    public float spawnSize;

    Vector3 newSize;

	// Use this for initialization
	void Start () {
        transform.localScale *= size;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (transform.localScale.magnitude < newSize.magnitude)
        {
            transform.localScale += new Vector3(1,1,1) * Time.deltaTime * growthrate;
        }

        if (!isRoot)
            targetPos = parent.transform.position + (1.7f * parent.GetComponent<VineV2_Generator>().direction * size);

        if ((targetPos - transform.position).magnitude > 0.2f)
        {
            if (!isRoot)
            {
                Debug.DrawLine(transform.position, targetPos, Color.blue);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
            }
        }
	}

    public void grow()
    {
        size += growthrate;
        newSize = new Vector3(1, 1, 1) * size;

        if (next == null && size >= spawnSize)
        {
            {
                Debug.Log("vine2 generating");
                GameObject g = GameObject.Instantiate(child);
                VineV2_Generator gen = g.GetComponent<VineV2_Generator>();

                g.transform.position = last().transform.position;

                gen.parent = gameObject;
                gen.isRoot = false;
                gen.size = 0.1f;
                //gen.targetPos = last().transform.position + last().GetComponent<VineV2_Generator>().direction;

                gen.direction = newDirection(direction);
                gen.parent = gameObject;
                
                g.GetComponent<BuildingBlock>().hasGrown = true;
                g.GetComponent<BuildingBlock>().isGrowing = false;
                gen.maxComplexity = maxComplexity - 1;

                last().GetComponent<VineV2_Generator>().next = g;
            }
        }
    }

    Vector3 newDirection(Vector3 dir)
    {
        Vector3 result;

        float newx = Random.Range(-1, 1);
        float newy = dir.y + .2f;
        float newz = Random.Range(-1, 1);

        Vector3 offset = new Vector3(newx, newy, newz);
        result = dir + offset;

        result.Normalize();
        return result;
    }

    public void growChildren()
    {
        if (size < maxSize)
        {
            grow();
        }

        if (next != null)
        {
            next.GetComponent<VineV2_Generator>().growChildren();
        }
    }

    public GameObject last()
    {
        if (next != null)
            return next.GetComponent<VineV2_Generator>().last();
        else
            return this.gameObject;
    }

    public GameObject root()
    {
        if (parent != null)
            return parent.GetComponent<VineV2_Generator>().root();
        else
            return this.gameObject;
    }

    public override void newChild(Vector3 inputLoc, int c)
    {
        Debug.Log("VineV2 newChild");
        root().GetComponent<VineV2_Generator>().growChildren();
    }
}

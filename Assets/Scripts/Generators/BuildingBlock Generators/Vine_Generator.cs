using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine_Generator : Generation {
    public Vine_Generator next, parent;
    public float radius = 5f;
    public bool generateToRight;
    public float growthRate = 0.1f;

    public float sizeMultiplier = 0.1f;
    
    public bool isRoot;

    bool rootIsGenerating;

	// Use this for initialization
	public void Start () {
        transform.localScale *= sizeMultiplier;
        isRoot = true;
	}

    public void setParent(GameObject p)
    {
        parent = p.GetComponent<Vine_Generator>();
        isRoot = false;
    }

    public void growSelf()
    {
        sizeMultiplier += growthRate;
        Vector3 newScale = new Vector3(1, 1, 1) * sizeMultiplier;

        //if (newScale.magnitude <= radius)
            transform.localScale = newScale;
    }

    public void growChildren()
    {
        if (next != null)
        {
            next.growChildren();
            growSelf();
        }
    }

    public Vine_Generator last()
    {
        if (next != null)
            return next.last();
        else
            return this;
    }

    public Vine_Generator root()
    {
        if (parent != null)
            return parent.root();
        else
            return this;
    }

    public override void newChild(Vector3 inputLoc, int c)
    {
        if (next != null)
        {
            next.newChild(inputLoc, c);
        }

        else
        {
            GameObject g = GameObject.Instantiate(child);
            Vine_Generator gen = g.GetComponent<Vine_Generator>();

            gen.setParent(this.gameObject);
            g.transform.position = last().transform.position;
            g.transform.rotation = last().transform.rotation;

            g.transform.Translate(Vector3.up * radius);
            g.transform.Rotate(new Vector3(0, 0, 20));
            g.transform.Translate(Vector3.down * radius);
            g.transform.Translate(Vector3.forward * .8f);

            g.GetComponent<BuildingBlockOLD>().isGenerating = false;
            g.GetComponent<BuildingBlockOLD>().hasGenerated = true;
            gen.maxComplexity = 0;

            last().next = gen;
            root().growChildren();
        }
    }

    public override void extraStuff(float growthrate, float extraThreshold)
    {
        //create leaf platforms
    }

    public void influenceParent()
    {

    }
}

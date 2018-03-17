using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlockOLD : MonoBehaviour {
    //public variables
    public bool isGenerating;
    public bool hasGenerated = false;

    public float spawnThreshhold;
    public float growthRate;
    public float extraThreshhold = 4;

    float currentPower = 0f;

    //private variables
    Generation g;
    //Degeneration d;
    float empowerment = 0f;
    public int childCount = 0;
    int maxChildren;
    bool extrasChecked = false;

    // Use this for initialization
    void Start () {
        g = GetComponent<Generation>();
        //d = GetComponent<Degeneration>();
        maxChildren = g.maxComplexity;
	}
	
	// Update is called once per frame
	void Update () {
        if (isGenerating && !hasGenerated)
        {
            if (currentPower < spawnThreshhold)
            {
                currentPower += (growthRate * Time.deltaTime);
            }

            if (currentPower > (spawnThreshhold / g.maxComplexity) * childCount)
            {
                createChild();
            }
        }

        if (hasGenerated)
        {
            //create extra stuff
            if (!extrasChecked)
            {
                g.createNewChance();
                extrasChecked = true;
            }
            if (g.creatingExtras)
                g.extraStuff(growthRate, extraThreshhold);
        }

        if (isGenerating)
        {
            //d.restoreIntegrity(growthRate);
        }
	}

    private void LateUpdate()
    {
        isGenerating = false;
    }

    void createChild()
    {
        g.newChild(Vector3.zero, childCount);
        if (childCount >= maxChildren)
            hasGenerated = true;
        else childCount++;
    }

    public void failedCreation()
    {
        if (childCount > 0)
            childCount--;
    }

    public void stopGenerating()
    {
        hasGenerated = true;
    }

    public void pauseGeneration()
    {
        isGenerating = false;
    }
}

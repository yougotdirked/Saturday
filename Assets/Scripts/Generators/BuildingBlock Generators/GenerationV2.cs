using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationV2 : MonoBehaviour {
    //This Building Block class should govern the state of individual blocks.
    //The old one governs how it reproduces, but that should not happen yet.
    //Building block "reproduction" should be governed by the island generation.
    //The Island generation should also govern the extra's generated around individual blocks.
    //The code in this class is centered on hexagons.

    public float currentGrowth; //value between 0-1, 0 being almost gone, 1 being fully charged
    public float growthRate = 1f;
    public float shrinkRate = .5f;
    public bool hasSupport;
    public float size;
    public float startHeight = -5f;
    public float phaseTime;
    public GameObject supportObject, pickupObject;

    enum State {Growing, Stable, Deteriorating};

    GameObject pickup;
    Vector3 targetPos, endPosition;
    State state;
    bool pickupGenerated = false;
    bool spawning;
    float phaseTimer = 0;
    float shrinkTarget = -1;

	// Use this for initialization
	void Start () {
        endPosition = transform.position;
        state = State.Growing;
        spawning = true;
        setToStart();
	}
	
	// Update is called once per frame
    private void FixedUpdate()
    {
        if (spawning && state == State.Growing)
        {
            currentGrowth += growthRate * Time.deltaTime * 2;

            if (currentGrowth >= 1)
            {
                spawning = false;
                currentGrowth = 1;

                if (pickup == null && !pickupGenerated)
                {
                    pickup = GameObject.Instantiate(pickupObject);
                    pickup.transform.Translate(transform.position + (Vector3.up * transform.localScale.y) + Vector3.up);
                    pickupGenerated = true;
                }
            }

            else if (hasSupport)
            {
                createSupport();
            }
        }

        calculateNewTarget();

        switch (state)
        {
            case State.Growing:
                if (!spawning)
                    currentGrowth += growthRate * Time.deltaTime;
                moveToTarget();
                break;

            case State.Stable:
                phaseTimer += Time.deltaTime;
                if (phaseTimer >= phaseTime)
                {
                    state = State.Deteriorating;
                    shrinkTarget = currentGrowth;
                    phaseTimer = 0;
                }
                currentGrowth -= Time.deltaTime * shrinkRate;
                break;

            case State.Deteriorating:
                if (currentGrowth > shrinkTarget)
                {
                    moveToTarget();
                }

                else
                {
                    state = State.Stable;
                    shrinkTarget = - 1;
                }
                currentGrowth -= Time.deltaTime * shrinkRate;
                break;
        }
    }
    void createSupport()
    {
        //copied code
        GameObject support;
        int supportCount = Mathf.CeilToInt(endPosition.y / (transform.lossyScale.y * 2));
        for (int i = 1; i <= supportCount; i++)
        {
            support = GameObject.Instantiate(supportObject, transform);
            support.transform.position = transform.position;
            support.transform.Translate(i * (Vector3.down * transform.lossyScale.y * 2));
        }
    }

    void calculateNewTarget()
    {
        float newHeight = (1 / endPosition.y) * currentGrowth;
        targetPos = new Vector3(transform.position.x, startHeight + newHeight, transform.position.z);
    }

    public void setToStart()
    {
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
    }

    public void setTarget(Vector3 target)
    {
        targetPos = target;
    }

    void moveToTarget()
    {
        
    }
}

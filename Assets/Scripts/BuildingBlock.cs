using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    //This Building Block class should govern the state of individual blocks.
    //The old one governs how it reproduces, but that should not happen yet.
    //Building block "reproduction" should be governed by the island generation.
    //The Island generation should also govern the extra's generated around individual blocks.
    //The code in this class is centered on hexagons.

    public float targetGrowth; //value between 0-1, 0 being almost gone, 1 being fully charged
    public float currentGrowth = 0;
    public float shrinkRate = .5f;

    public float startHeight = -5f;

    public GameObject supportObject, pickupObject;
    public float growthRate = 1f;
    public float phaseTime;

    public bool hasSupport;
    public float size;

    public bool isGrowing;
    [HideInInspector] public bool islandIsGrowing;
    public bool hasGrown = false;

    enum State { Growing, Stable, Deteriorating };

    Island island;
    GameObject pickup;
    public Vector3 targetPos, endPosition;
    State state;
    bool pickupGenerated = false;
    bool spawning;
    float phaseTimer = 0;

    // Use this for initialization
    void Start()
    {
        endPosition = transform.position;
        spawning = true;
        setToStart();
        targetGrowth = 1;
        createSupport();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!pickupGenerated && currentGrowth >= 1)
        {
            createPickup();
            pickupGenerated = true;
        }

        if (islandIsGrowing)
        {
            if (targetGrowth < 1)
                targetGrowth += Time.deltaTime * growthRate * .5f; //temporary modifier
        }

        if (isGrowing)
        {
            if (targetGrowth < 1)
                targetGrowth += Time.deltaTime * growthRate;

            island.blockIsGrowing = true;
        }

        if ((int) currentGrowth == (int) targetGrowth)
        {
            state = State.Stable;
        }

        else if (currentGrowth < targetGrowth)
        {
            state = State.Growing;
        }

        else if (currentGrowth > targetGrowth)
        {
            state = State.Deteriorating;
        }

        switch (state)
        {
            case State.Growing:
                if (currentGrowth < targetGrowth && isGrowing)
                    currentGrowth += (growthRate * Time.deltaTime);

                if (currentGrowth < targetGrowth && islandIsGrowing)
                {
                    currentGrowth += (growthRate * Time.deltaTime * .5f);
                }

                break;

            case State.Stable:
                //Set state to deteriorating after a set amount of time.

                phaseTimer += Time.deltaTime;
                if (phaseTimer >= phaseTime)
                {
                    targetGrowth = targetGrowth / 2;
                    //targetGrowth = currentGrowth;
                    phaseTimer = 0;
                }

                break;

            case State.Deteriorating:

                if (currentGrowth > targetGrowth)
                    currentGrowth -= Time.deltaTime * shrinkRate;

                //calculateNewTarget();

                if (currentGrowth <=0)
                {
                    //delete logic
                }
                break;
        }

        if ((targetPos - transform.position).magnitude - 0.1f < 0 || (targetPos - transform.position).magnitude + 0.1f > 0)
        {
            calculateNewTarget();
            moveToTarget();
        }

        checkGrowthrate();

        isGrowing = false;
        islandIsGrowing = false;
    }

    public void createPickup()
    {
        GameObject pickup = GameObject.Instantiate(pickupObject, transform);
        pickup.transform.localScale = pickup.transform.localScale * (1 / pickup.transform.lossyScale.magnitude);
        pickup.transform.Translate(transform.up * ((transform.lossyScale.magnitude / 2) + pickupObject.GetComponent<Pickup>().floatingHeight));
    }

    public void setIsland(Island i)
    {
        island = i;
    }

    public void setEndHeight(float newEnd)
    {
        endPosition = new Vector3(transform.position.x, newEnd, transform.position.z);
    }

    void createSupport()
    {
        GameObject support;
        int supportCount = Mathf.RoundToInt(endPosition.y / (transform.lossyScale.y * 2) + 10);
        for (int i = 1; i <= supportCount; i++)
        {
            support = GameObject.Instantiate(supportObject, transform);
            support.transform.position = transform.position;
            support.transform.Translate(i * (Vector3.down * transform.lossyScale.y * 2));
        }
    }

    void checkGrowthrate()
    {
        if (currentGrowth > targetGrowth)
            currentGrowth = targetGrowth;
    }

    void calculateNewTarget()
    {
        //calculate y value according to targetGrowth variable
        //currently only for up and down
        float newY = currentGrowth * ((endPosition.y) - startHeight);
        targetPos = new Vector3(transform.position.x, startHeight + newY, transform.position.z);
    }

    public void setToStart()
    {
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
    }

    void setTarget(Vector3 target)
    {
        targetPos = target;
    }

    void moveToTarget()
    {
        Vector3 dir = (targetPos - transform.position);
        transform.Translate(dir * Time.deltaTime);
    }

    public void stopGenerating()
    {
        hasGrown = true;
    }

    public void pauseGeneration()
    {
        isGrowing = false;
    }
}


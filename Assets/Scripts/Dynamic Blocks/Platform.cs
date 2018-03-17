using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public Vector3 direction, velocity;
    public float maxDistance;
    public bool isMoving = true;
    public float maxSpeed;
    public float size = 1f;
    public float snapThreshhold = .1f;
    public float waitingTime;
    public float acceleration;

    bool foundPath = false;
    bool movingForward = true;
    float distanceTravelled = 0f;
    float waitedTime = 0f;
    float currentMaxSpeed;

    Vector3 startPos, endPos, currentGoal;

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(size, transform.localScale.y, size);
        transform.name = "MovingPlatform";
        waitingTime = 2f;

        startPos = transform.position;

        endPos = transform.position + (direction * maxDistance);

        currentGoal = endPos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        velocity = currentGoal - transform.position;

        if (velocity.magnitude > currentMaxSpeed)
        {
            velocity = velocity.normalized * currentMaxSpeed;
        }

        if (!foundPath)
        {
            checkBounce();
        }

        if (maxDistance < 2.5f || direction.magnitude < .5f)
        {
            GetComponentInChildren<Transform>().SetParent(null);
            Destroy(gameObject);
        }

        if (isMoving)
        {
            transform.Translate(velocity * Time.deltaTime);
            distanceTravelled += (velocity.magnitude * Time.deltaTime);
            if ((currentGoal - transform.position).magnitude <= snapThreshhold)
            {
                //transform.position = currentGoal;
                reverseDirection();
                isMoving = false;
                currentMaxSpeed = 0;
            }
            if (currentMaxSpeed < maxSpeed)
            {
                currentMaxSpeed += acceleration * Time.deltaTime;
            }
            else
            {
                currentMaxSpeed = maxSpeed;
            }
        }
        if (!isMoving)
        {
            waitedTime += Time.deltaTime;

            if (waitedTime >= waitingTime)
            {
                isMoving = true;
            }
        }

        Debug.DrawRay(transform.position, velocity, Color.red);
    }
    void reverseDirection()
    {
        if (currentGoal == endPos)
            currentGoal = startPos;
        else if (currentGoal == startPos)
            currentGoal = endPos;
    }

    void checkBounce()
    {
        RaycastHit ray;

        if (Physics.Raycast(transform.position + (Vector3.up * .5f), velocity.normalized, out ray, size * 2))
        {
            if (ray.transform.tag != "Player")
            {
                endPos = transform.position;
                currentGoal = transform.position;
                maxDistance = distanceTravelled;
                foundPath = true;
                reverseDirection();
            }
        }

        if (distanceTravelled >= maxDistance)
        {
            foundPath = true;
            reverseDirection();
        }
    }

    public void startMoving()
    {
        isMoving = true;
    }

    public void endMoving()
    {
        isMoving = false;
    }
}

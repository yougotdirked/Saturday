using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaantjeFloat : MonoBehaviour
{
    [SerializeField] float maxSpeed = 1;
    [SerializeField] float distance = 3;
    [SerializeField] float threshHold = .5f;

    Vector3 locA;
    Vector3 locB;
    Vector3 target;
    bool movingUp = false;
    float currentSpeed;

    // Use this for initialization
    void Start()
    {
        locA = transform.position;
        locB = transform.position + Vector3.up * distance;
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingUp)
            target = locA;
        else
            target = locB;

        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * currentSpeed);

        if ((transform.position - target).magnitude <= threshHold)
        {
            movingUp = !movingUp;
            currentSpeed = 0;
        }
    }
}

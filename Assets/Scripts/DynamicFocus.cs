using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

/*
 * Use this script to customize camera focus and stuff.
 * */

public class DynamicFocus : MonoBehaviour {
    public bool adjustFocalLength = true;

    [SerializeField] LayerMask mask;
    [SerializeField] float maxDistance = 1000;
    [SerializeField] float minimalDistance = .5f;
    [SerializeField] float forwardAdjustment = 1; //use forward and backward adjustment to control speed of transitioning between different fields of focus
    [SerializeField] float backwardAdjustment = 2;

    //if the camera is focussing on something nearby, the focal length should decrease
    [SerializeField] float minFocalLength = 50;
    [SerializeField] float focalStrength = 1;

    Camera mainCam;
    PostProcessingBehaviour ppb;
    PostProcessingProfile ppp;
    GameObject targetObject;
    float targetDistance;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        ppb = GetComponent<PostProcessingBehaviour>();
        ppp = ppb.profile;
	}
	

	// Update is called once per frame
	void FixedUpdate () {
        RaycastHit hit;
        DepthOfFieldModel.Settings temp_DOFModel = ppp.depthOfField.settings;
        float currentDistance = ppp.depthOfField.settings.focusDistance;
        Vector3 rayOrigin = transform.position + transform.forward * minimalDistance;
        float adjustmentMultiplier;
        float currentFocalDistance = ppp.depthOfField.settings.focalLength;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, maxDistance, mask))
        {
            targetDistance = hit.distance;
        }

        //set adjustmentMultiplier to correct one:
        if (targetDistance > currentDistance)
            adjustmentMultiplier = forwardAdjustment;
        else
            adjustmentMultiplier = backwardAdjustment;

        //calculate difference:
        float difference = Mathf.Abs(targetDistance - currentDistance);

        difference = Mathf.Abs(targetDistance - currentDistance);

        //focal length adjustments:
        if(adjustFocalLength)
        {
            float targetFocal = minFocalLength + (focalStrength * targetDistance);
            temp_DOFModel.focalLength = Mathf.Lerp(currentFocalDistance, targetFocal, Time.deltaTime * focalStrength);
        }
        
        //Lerp the focus distance between the old value and the new ray distance
        temp_DOFModel.focusDistance = Mathf.Lerp(currentDistance, targetDistance, (Time.deltaTime / difference) * adjustmentMultiplier);

        //set the post processing profile:
        ppp.depthOfField.settings = temp_DOFModel;
    }
}

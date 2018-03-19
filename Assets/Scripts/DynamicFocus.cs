using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

/*
 * Use this script to customize camera focus and stuff.
 * */

public class DynamicFocus : MonoBehaviour {
    public bool adjustFocalLength = true;
    public bool smooth = true;

    [SerializeField] LayerMask mask;
    [SerializeField] float minimalDistance = .5f;
    [SerializeField] float forwardAdjustment = 30f; //use forward and backward adjustment to control speed of transitioning between different fields of focus
    [SerializeField] float backwardAdjustment = 60f;
    [SerializeField] float maximumDistance = 1000f; //distance the ray travels for sampling

    //if the camera is focussing on something nearby, the focal length should decrease
    [SerializeField] float minFocalLength = 50;
    [SerializeField] float focalStrength = 1;

    //[SerializeField] float drag; //value for smooth focus transitioning.

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
	void Update () {
        RaycastHit hit;
        DepthOfFieldModel.Settings temp_DOFModel = ppp.depthOfField.settings;
        float currentDistance = ppp.depthOfField.settings.focusDistance;
        Vector3 rayOrigin = transform.position + transform.forward * minimalDistance;
        float adjustmentMultiplier;
        
        float currentFocalDistance = ppp.depthOfField.settings.focalLength;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, maximumDistance))
            targetDistance = hit.distance;

        float difference = Mathf.Abs(targetDistance - currentDistance);

        if (targetDistance > currentDistance)
            adjustmentMultiplier = forwardAdjustment;
        else
            adjustmentMultiplier = backwardAdjustment;
        

        if (difference > 200)
        {
            difference = 200;
        }

        //focal length adjustments:
        if(adjustFocalLength)
        {
            float targetFocal = minFocalLength + (focalStrength * targetDistance);
            temp_DOFModel.focalLength = Mathf.Lerp(currentFocalDistance, targetFocal, Time.deltaTime * focalStrength);
        }

        //instead of focussing on the current center, make the focus point drag behind the camera:
        if (smooth)
        {
            //smooth camera focus logic here
        }

        //Lerp the focus distance between the old value and the new ray distance
        temp_DOFModel.focusDistance = Mathf.Lerp(currentDistance, targetDistance, (Time.deltaTime / difference) * adjustmentMultiplier);

        ppp.depthOfField.settings = temp_DOFModel;
    }
}

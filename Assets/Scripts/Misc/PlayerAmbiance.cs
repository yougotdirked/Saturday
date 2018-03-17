using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmbiance : MonoBehaviour {

    // Use this for initialization
    public Light sun;
    //public Skybox skybox;

    public float maxExposure = 1.5f;
    public float maxIntensity = 1.5f;

    float sunStart;
    float skyboxStart;

    GameObject player;
    Camera mainCamera;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        float newIntensity = player.transform.position.y / 25;
        float newExposure = player.transform.position.y/75;

        if (newExposure >= maxExposure)
        {
            newExposure = maxExposure;
        }

        if (newIntensity >= maxIntensity)
        {
            newIntensity = maxIntensity;
        }
        
        RenderSettings.skybox.SetFloat("_Exposure", newExposure);
        sun.intensity = newIntensity;
	}
}

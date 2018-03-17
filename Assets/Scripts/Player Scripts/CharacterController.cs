using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterBehaviour))]

public class CharacterController : MonoBehaviour {

    //public variables
    public float moveSpeed;
    public float gravity;
    CharacterBehaviour character;
    Vector3 move;
    Transform cam;
    private Vector3 camForward;
    public bool doubleJump = false;

    public bool isJumping = false;
    float jump;
    bool reachedMaxTurning = false;

    //private variables    
    PlayerGrow growth;
    PlayerCreate creation;
    
	void Start () {
        growth = GetComponent<PlayerGrow>();
        creation = GetComponent<PlayerCreate>();
        character = GetComponent<CharacterBehaviour>();
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("No main Camera!");
        }
	}
	
	// Update is called once per frame
	void Update () {
        mouseInput();

        doubleJump = Input.GetKeyDown(KeyCode.Space);

        isJumping = Input.GetButton("Jump");

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (cam != null)
        {
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = vertical * camForward + horizontal * cam.right;
        }

        else
        {
            move = vertical * Vector3.forward + horizontal * Vector3.right;
        }

        //pass parameters to character control
        character.Move(move, isJumping, doubleJump);
        character.stopDoubleJump();
        isJumping = false;

    }

    private void FixedUpdate()
    {

    }


    void mouseInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            creation.createSelected();
        }

        if (Input.GetAxis("Fire2") > 0)
        {
            growth.startGrowing();
        }

        else
            growth.endGrowing();

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            creation.nextItem();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            creation.prevItem();
        }
    }
}

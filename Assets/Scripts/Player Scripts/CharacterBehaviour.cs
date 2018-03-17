using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {
    [SerializeField] float movingTurnSpeed = 360;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] float stationaryTurnSpeed = 180;
    [SerializeField] float jumpPower = 12f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] float floatingGravity = 1f; //can not be higher than the gravity
    [SerializeField] float stepHeight = .25f;
    //[SerializeField] float frontCheckDistance = 0.1f;

    float originalGravity;
    Rigidbody rigidbody_;
    //CapsuleCollider capsulecollider;
    //BoxCollider boxCollider;
    Animator animator;
    float playerHeight;
    public bool isGrounded;
    float originalGroundCheckDistance;
    Vector3 groundNormal;
    bool hasDoubleJumped;
    bool doubleJump;
    Vector3 m;
    Vector3 oldM;
    bool isFloating;
    bool hasFloated;
    RaycastHit hitinfo;

    bool feetColliding;

    float turnAmount;
    float forwardAmount;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rigidbody_ = GetComponent<Rigidbody>();
        //boxCollider = GetComponent<BoxCollider>();
        //capsulecollider = GetComponent<CapsuleCollider>();
        //playerHeight = capsulecollider.height;

        rigidbody_.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        originalGroundCheckDistance = groundCheckDistance;
        originalGravity = gravityMultiplier;
    }

    void Update()
    {
        feetColliding = true;
    }

    public void Move(Vector3 move, bool jump, bool dj)
    {
        if (move.magnitude > 1f)
            move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGround();
        move = Vector3.ProjectOnPlane(move, groundNormal);

        forwardAmount = move.z;
        turnAmount = Mathf.Atan2(move.x, move.z);
        
        m = move;

        ApplyExtraTurnRotation(move.x);

        if (isGrounded)
        {
            HandleGroundMovement(jump, move);
        }
        else
        {
            if (dj)
                doubleJump = true;

            HandleAirborneMovement();
        }

        if (hasDoubleJumped && jump && rigidbody_.velocity.y < 0 && !hasFloated)
        {
            isFloating = true;
        }

        else
        {
            isFloating = false;
        }

        //check stepheight
        if (Physics.Raycast(transform.position, transform.forward, .6f))
            checkStep(m);

        if (m.y == 0 && !isGrounded)
        {
            checkBehind();
        }
    }

    void checkBehind()
    {
        Vector3 pos = transform.position - transform.forward;
        RaycastHit behindinfo;

        if (Physics.Raycast(pos + (Vector3.up * 0.1f), Vector3.down, out behindinfo, groundCheckDistance))
            isGrounded = true;

    }

    void checkStep(Vector3 move)
    {
        Vector3 pos = transform.position + transform.forward + (Vector3.up * stepHeight);

        RaycastHit stepinfo;

        Debug.DrawRay(pos, Vector3.down, Color.red, Time.deltaTime);

        if (Physics.Raycast(pos, Vector3.down, out stepinfo, stepHeight + .1f) && move.z != 0)
        {
            if (stepinfo.transform.gameObject.tag == "Building Block")
            {
                transform.position += Vector3.up * (stepHeight - stepinfo.distance) * 15 * Time.deltaTime;
                isGrounded = true;
            }
        }
    }

    public void doublejump()
    {
        print("doublejump");
        doubleJump = true;
    }

    public void stopDoubleJump()
    {
        doubleJump = false;
    }

    public void OnAnimatorMove()
    {
        if (isGrounded && Time.deltaTime > 0)
        {
            Vector3 v = ((transform.forward * moveSpeed * m.z) / Time.deltaTime )/ 10;
            
            v.y = rigidbody_.velocity.y;

            rigidbody_.velocity = v;

        }

        //floating

        else if (Time.deltaTime > 0 && isFloating)
        {
            Vector3 v = ((transform.forward * moveSpeed * m.z) / Time.deltaTime) / 10;

            if (!isFloating)
            {
                v.y = rigidbody_.velocity.y;
            }
            else
            {
                v.y = floatingGravity;
            }

            rigidbody_.velocity = v;
        }

    }

    void HandleAirborneMovement()
    { 
        if (doubleJump && !hasDoubleJumped && !isGrounded)
        {
            rigidbody_.velocity = new Vector3(rigidbody_.velocity.x, jumpPower, rigidbody_.velocity.z);
            isGrounded = false;
            hasDoubleJumped = true;
            animator.applyRootMotion = false;
            groundCheckDistance = 0.1f;
        }

        else
        {
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rigidbody_.AddForce(extraGravityForce);

            groundCheckDistance = rigidbody_.velocity.y < 0 ? originalGroundCheckDistance : 0.1f;
        }
    }

    void HandleGroundMovement(bool jump, Vector3 m)
    {
        if (jump && isGrounded)
        {
            rigidbody_.velocity = new Vector3(rigidbody_.velocity.x, jumpPower, rigidbody_.velocity.z);
            isGrounded = false;
            animator.applyRootMotion = false;
            groundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation(float isturning)
    {
        if (isturning != 0)
        {
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }

    void CheckGround()
    {
        //gather data
        //, backinfo, frontinfo, rightinfo, leftinfo;
        bool centerhit = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitinfo, groundCheckDistance);
        //bool backhit = Physics.Raycast(transform.position + (transform.worldToLocalMatrix.MultiplyVector(Vector3.back * 2)) + (Vector3.up * 0.1f), Vector3.down, out backinfo, groundCheckDistance);
        //bool fronthit = Physics.Raycast(transform.position + (transform.worldToLocalMatrix.MultiplyVector(Vector3.forward * 2)) + (Vector3.up * 0.1f), Vector3.down, out frontinfo, groundCheckDistance);
        
        if (centerhit)
        {
            hasDoubleJumped = false;
            groundNormal = hitinfo.normal;
            isGrounded = true;
            animator.applyRootMotion = true;
        }
        else
        {
            //print(hitinfo);
            isGrounded = false;
            groundNormal = Vector3.up;
            animator.applyRootMotion = false;
        }
        if (isGrounded && hitinfo.transform.name == "MovingPlatform")
            gameObject.transform.SetParent(hitinfo.transform);

        else
            gameObject.transform.SetParent(null);
    }

    private void OnTriggerStay(Collider other)
    {
        feetColliding = true;
    }
}

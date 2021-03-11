using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonMovement : MonoBehaviour
{
    public Vector3 playerVelocity;

    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float jumpForce = 20f;
    public float gravityValue = -9.81f;

    private Vector3 floorNorm = Vector3.zero;
    float turnSmoothVelocity;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        SprintingCommanded();
        EscapeCommanded();
        Vector3 moveDir = CameraRelativeMovement();
        Vector3 playerXZMovement = moveDir.normalized * speed;
        Vector3 playerYMovement = new Vector3(0f, playerVelocity.y, 0f); // = proj  //
        Vector3 gravityMovement = new Vector3(0f, gravityValue*Time.deltaTime, 0f);  //


        float floorAngle = Vector3.Angle(floorNorm, Vector3.up);

        if(controller.isGrounded)   //
        {

            if(floorAngle > 40)
            {
                Debug.Log("steep");
                playerYMovement += .1f * Vector3.ProjectOnPlane(playerYMovement, floorNorm);
                gravityMovement = .1f * Vector3.ProjectOnPlane(gravityMovement, floorNorm); // MAX OUT SLIDE VELOCITY

                if (playerYMovement.magnitude > 100f)
                {
                    Debug.Log("too fast");
                    playerYMovement = 100*playerYMovement.normalized;
                }
            }
            else
            {
                Debug.Log("zeroed");
                playerYMovement = Vector3.zero;
            }
            
            if (Input.GetButtonDown("Jump"))
                playerYMovement = new Vector3(0, jumpForce, 0);


        }





        Vector3 moveProjected = sameMagProjected(playerXZMovement, floorNorm); //plane is floor

        // if(floorAngle > 60 && floorAngle < 89)
        // {
        //     playerXZMovement = Vector3.zero;
        //     gravityMovement = 50 * floorNorm; //-10*sameMagProjected(gravityMovement,moveProjected); //plane normal to floor
        //     //Debug.Log(gravityMovement);
        // }

        if (moveProjected.y < 0 && floorAngle < 60) //moving downhill
        {
            playerXZMovement = moveProjected;
        }

        playerVelocity = playerXZMovement + playerYMovement + gravityMovement;
        Vector3 displacementVector = playerVelocity *Time.deltaTime;
        //displacementVector = sameMagProjected(displacementVector, floorNorm);
        floorNorm = Vector3.zero;  //will update in Move() when/if OnControllerColliderHit is called
        controller.Move(displacementVector);

    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 norm = hit.normal;
        floorNorm = norm;
    }

    Vector3 sameMagProjected(Vector3 v, Vector3 norm)
    {
        return v.magnitude * Vector3.ProjectOnPlane(v, norm).normalized;
    }

    bool SprintingCommanded()
    {
        bool run = Input.GetKey(KeyCode.LeftShift);
        if(run)
        {
            speed = 18f;
            jumpForce = 25f;
        }

        else
        {
            speed = 6f;
            jumpForce = 20f;
        }
        return run;
    }

    bool EscapeCommanded()
    {
        bool escape = Input.GetButtonDown("Cancel");
        if(escape)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
        return escape;
    }

    Vector3 CameraRelativeMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir;
        if(direction.magnitude >= .1f)   //movement relative to camera
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDir = Vector3.zero;
        }

        return moveDir;
    }

    Vector3 JumpCommanded()
    {
        //Debug.Log("hello");
        bool jump = Input.GetButtonDown("Jump");
        if(jump)
        {
            Debug.Log(playerVelocity);
            Vector3 jumpVelocity = new Vector3(0, jumpForce, 0);
            return jumpVelocity;
        }
        return Vector3.zero;
    }

    bool stableFooting()
    {
        return true;
    }

}

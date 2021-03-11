using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Cap_character_Movement:MonoBehaviour
{
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 10.0f;
    public float jumpVel = 5.0f;
    public float gravityValue = 9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            Debug.Log("Ground");
            playerVelocity.y = -gravityValue * Time.deltaTime;;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.Log(forward);
        float curSpeed = playerSpeed * Input.GetAxis("Vertical");
        controller.Move(forward * curSpeed * playerSpeed);

        // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // controller.Move(move * Time.deltaTime * playerSpeed);

        // if (move != Vector3.zero)
        // {
        //     gameObject.transform.forward = move;
        // }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            if(groundedPlayer)
            {
                playerVelocity.y += jumpVel;
            }
            else
                Debug.Log("Not Grounded");
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        //Transform target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //controller.rotation = target.transform.localRotation;
    }
}











// using UnityEngine;
// using System.Collections;
//
// [RequireComponent(typeof(CharacterController))]
// public class Cap_character_Movement : MonoBehaviour
// {
//     public float speed = 100.0F;
//     public float rotateSpeed = 100.0F;
//     public CharacterController controller;
//
//     void Start()
//     {
//         controller = GetComponent<CharacterController>();
//     }
//
//     void Update()
//     {
//         // Rotate around y - axis
//         transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
//
//         // Move forward / backward
//         Vector3 forward = transform.TransformDirection(Vector3.forward);
//         float curSpeed = speed * Input.GetAxis("Vertical");
//         controller.SimpleMove(forward * curSpeed);
//     }
// }

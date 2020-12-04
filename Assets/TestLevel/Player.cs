using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject feet_obj;

    private const float SPEED_X = 0.5f;
    private const float JUMP_VEL = 10f;
    private const float DEAD_ZONE = 0.1f;

    private float vx_max = 10f;
    private bool grounded = false;

    private Rigidbody rb;

    void Start() {
        this.rb = GetComponent<Rigidbody>();
    }

    void Update() {
    }

    void FixedUpdate() {
        float ax_hor = Input.GetAxis("Horizontal");
        Vector3 vel = this.rb.velocity;

        // Handle horizontal velocity with this section.
        if (Mathf.Abs(ax_hor) > DEAD_ZONE) {
            vel.x += SPEED_X * Mathf.Sign(ax_hor);

            // Cap horizontal velocity to avoid moving too fast.
            if (Mathf.Abs(vel.x) > vx_max) {
                vel.x = vx_max * Mathf.Sign(vel.x);
            }
        } else {
            vel.x *= 0.95f;
        }

        // Handle vertical velocity with this section
        this.CheckGrounded();
        if (this.grounded && Input.GetButton("Jump")) {
            vel.y = JUMP_VEL;
        }
        
        this.rb.velocity = vel;
    }

    private void CheckGrounded() {
        this.grounded = Physics.Raycast(this.feet_obj.transform.position, Vector3.down, 0.25f);
    }
}

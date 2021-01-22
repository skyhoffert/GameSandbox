using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Transform feet;

    private float ground_speed = 40f;
    private float air_speed = 20f;
    private float ground_dist = 0.3f;

    private bool is_grounded = false; // Calculated with raycast.
    private bool is_airborne = false; // Set by this code.

    private float jump_velocity = 8f;
    private float jump_cd_max = 0.15f;
    private float jump_cd = 0f;
    private float jump_air_cd_max = 0.1f; // time after leaving platform to jump
    private float jump_air_cd = 0f;

    private string debuglog = "";
    private float debugtimermax = 2f;
    private float debugtimer = 2f;

    private LayerMask phys_mask;

    private Rigidbody rb;

    private void Jump() {
        this.rb.velocity += Vector3.up * this.jump_velocity;
        this.is_airborne = true;
        this.jump_cd = this.jump_cd_max;
    }

    void Start() {
        phys_mask.value = ~(1 << 2);

        this.rb = GetComponent<Rigidbody>();
    }

    void Update() {
        // Calculate movement.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Calculate gravity/feet collision.
        Debug.DrawLine(this.feet.position, this.feet.position + -Vector3.up * this.ground_dist, Color.blue);
        bool was_grounded = this.is_grounded;
        this.is_grounded = Physics.Raycast(this.feet.position, -Vector3.up, this.ground_dist, this.phys_mask);
        if (this.is_grounded) {
            this.debuglog += "_";
            this.is_airborne = false;
        } else {
            this.debuglog += ".";
            this.is_airborne = true;
            if (was_grounded) {
                this.jump_air_cd = this.jump_air_cd_max;
            }
        }

        // Jumping stuff here.
        if (this.jump_cd > 0f) {
            this.jump_cd -= Time.deltaTime;
        }
        if (this.jump_air_cd > 0f) {
            this.jump_air_cd -= Time.deltaTime;
        }
        if (Input.GetButton("Jump")) {
            if (this.is_grounded && this.jump_cd <= 0f) {
                this.Jump();
            } else if (this.is_airborne && this.jump_cd <= 0f && this.jump_air_cd >= 0f) {
                Debug.Log("Jumped after leaving platform.");
                this.Jump();
            }
        }
        
        // Add movements here.
        float spd = 0;
        if (this.is_grounded) {
            spd = this.ground_speed;
        } else {
            spd = this.air_speed;
        }
        rb.AddForce(move * spd);

        // DEBUG
        this.debugtimer -= Time.deltaTime;
        if (this.debugtimer <= 0) {
            this.debugtimer = this.debugtimermax;
            //Debug.Log(this.debuglog);
            this.debuglog = "";
        }
    }
}

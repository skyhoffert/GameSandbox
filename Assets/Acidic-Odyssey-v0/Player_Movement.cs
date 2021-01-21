using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Transform feet;

    private float ground_speed = 12f;
    private float air_speed = 6f;
    private float ground_dist = 0.2f;
    private Vector3 velocity;
    private float accel = 2f;
    private bool is_grounded = false;
    private float jump_velocity = 4f;
    private float jump_cooldown = 0f;

    private string debuglog = "";
    private float debugtimermax = 2f;
    private float debugtimer = 2f;

    private LayerMask phys_mask;

    private Rigidbody rb;

    void Start() {
        this.velocity = new Vector3(0f,0f,0f);
        phys_mask.value = ~(1 << 2);

        this.rb = GetComponent<Rigidbody>();
    }

    void Update() {
        // Calculate movement.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Calculate gravity/feet collision.
        this.is_grounded = Physics.Raycast(this.feet.position, -Vector3.up, this.ground_dist, this.phys_mask);
        if (this.is_grounded) {
            this.velocity.y = 0f;
            this.debuglog += "_";
        } else {
            this.velocity.y -= this.accel * Time.deltaTime;
            this.debuglog += ".";
        }

        if (this.jump_cooldown > 0f) {
            this.jump_cooldown -= Time.deltaTime;
        }
        if (this.is_grounded && Input.GetButton("Jump")) {
            if (this.jump_cooldown <= 0f) {
                this.velocity.y = this.jump_velocity;
                this.jump_cooldown = 0.1f;
            }
        }
        
        // Add movements here.
        float spd = 0;
        if (this.is_grounded) {
            spd = this.ground_speed;
        } else {
            spd = this.air_speed;
        }
        this.velocity += move * spd * Time.deltaTime;
        rb.velocity = this.velocity;

        // DEBUG
        this.debugtimer -= Time.deltaTime;
        if (this.debugtimer <= 0) {
            this.debugtimer = this.debugtimermax;
            //Debug.Log(this.debuglog);
            this.debuglog = "";
        }

        if (Input.GetButton("Ctrl")) {
        }
    }
}

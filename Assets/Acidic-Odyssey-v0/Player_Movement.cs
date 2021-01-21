using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform feet;

    private float ground_speed = 12f;
    private float air_speed = 6f;
    private float ground_dist = 0.2f;
    private Vector3 velocity;
    private float accel = 2f;
    private bool is_grounded = false;
    private float jump_velocity = 1.5f;
    private float jump_cooldown = 0f;

    void Start() {
        this.velocity = new Vector3(0f,0f,0f);
    }

    void Update() {
        // Calculate movement.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Calculate gravity/feet collision.
        this.velocity.y -= this.accel * Time.deltaTime;
        this.is_grounded = Physics.Raycast(this.feet.position, -Vector3.up, this.ground_dist);
        if (this.is_grounded) {
            this.velocity.y = 0f;
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

        move += this.velocity;
        
        // Add movements here.
        float spd = this.is_grounded ? this.ground_speed : this.air_speed;
        this.velocity += move * spd;
        this.controller.Move(this.velocity * Time.deltaTime);
    }
}

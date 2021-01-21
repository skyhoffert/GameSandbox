using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Look : MonoBehaviour
{
    public Transform playerTransform;

    private float mouse_sense_horizont = 400f;
    private float mouse_sense_vertical = 300f;

    private float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * this.mouse_sense_horizont * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * this.mouse_sense_horizont * Time.deltaTime;

        this.xRotation -= mouseY * this.mouse_sense_vertical * Time.deltaTime;
        this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(this.xRotation, 0f, 0f);
        this.playerTransform.Rotate(Vector3.up * mouseX);
    }
}

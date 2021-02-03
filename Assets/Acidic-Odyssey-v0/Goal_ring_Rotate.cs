using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal_ring_Rotate : MonoBehaviour
{
    private float rotation_rate = 10.0f;

    private AudioSource yay;

    private bool triggered = false;

    void Start() {
        this.yay = GetComponent<AudioSource>();
    }

    void Update() {
        this.transform.Rotate(new Vector3(0f,0f,this.rotation_rate * Time.deltaTime));

        if (this.triggered && this.yay.isPlaying == false) {
            SceneManager.LoadScene("Acidic-Odyssey-v0");
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (this.triggered == false) {
            this.yay.Play();
            Debug.Log("Player reached goal.");
            this.triggered = true;
        }
    }
}

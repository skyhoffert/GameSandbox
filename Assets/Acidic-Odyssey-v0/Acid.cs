using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Acid : MonoBehaviour
{
    private AudioSource sizzle;

    private bool triggered = false;

    void Start() {
        this.sizzle = GetComponent<AudioSource>();    
    }

    void Update() {
        if (this.triggered && this.sizzle.isPlaying == false) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (this.triggered == false) {
            this.sizzle.Play();
            Debug.Log("Player fell into acid.");
            this.triggered = true;
        }
    }
}

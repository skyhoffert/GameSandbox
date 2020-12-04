using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject start_hover;
    public GameObject exit_hover;

    private const int nBUTTONS = 2;
    private const float DEAD_ZONE = 0.1f;
    
    private int selected = 0;
    private bool bounce_catch = false; // Allows only one button move on keypress

    // Start is called before the first frame update
    void Start() {
        this.selected = 0;
        this.UpdateSelection();
    }

    // Update is called once per frame
    void Update() {
        // Unity takes this from "w" and "s" keys.
        float vert_ax = Input.GetAxis("Vertical");

        // bounce_catch to avoid holding button to move. vert_ax has a dead zone
        if (this.bounce_catch == false && Mathf.Abs(vert_ax) > DEAD_ZONE) {
            this.bounce_catch = true;

            if (vert_ax < 0) { // DOWN
                this.selected = (this.selected + 1) % nBUTTONS;
            } else if (vert_ax > 0) { // UP
                this.selected = (this.selected + nBUTTONS-1) % nBUTTONS;
            }

            this.UpdateSelection();
        } else {
            // Here we verify that the player has let go of the keys or joystick.
            if (Mathf.Abs(vert_ax) < DEAD_ZONE) {
                this.bounce_catch = false;
            }
        }

        if (Input.GetButton("Submit")) {
            if (this.selected == 0) {
                SceneManager.LoadScene("TestLevel");
            } else if (this.selected == 1) {
                // Need this weird "#if" block to check if in editor or in a build.
                #if UNITY_EDITOR
                    // Application.Quit() does not work in the editor so
                    // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }
    }

    private void UpdateSelection() {
        this.start_hover.SetActive(false);
        this.exit_hover.SetActive(false);

        if (selected == 0) {
            this.start_hover.SetActive(true);
        } else if (selected == 1) {
            this.exit_hover.SetActive(true);
        }
    }
}

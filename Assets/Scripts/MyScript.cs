using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    private Vector3 home;
    private bool trav_to_target = true;
    private int frames = 0;
    private float freq = 1;

    public Vector3 target = Vector3.zero;
    public float dur = 1; // seconds

    // Start is called before the first frame update
    void Start()
    {
        home = transform.position;
        freq = 2*Mathf.PI / (dur*60f);
    }

    // Update is called once per frame
    void Update()
    {
        // float ratio = ((float)frames) / ((float)frame_count);
        // float v = 0.5f*Mathf.Sin(ratio*Mathf.PI/2) + 1;
        float v = 0.5f*Mathf.Sin(freq*((float)frames)) + 0.5f;
        
        transform.position = Vector3.Lerp(home, target, v);

        if (trav_to_target == true) {
        } else {
            //transform.position = Vector3.Lerp(target, home, v);
        }
        
        frames++;
        /*
        if (frames > 4*frame_count) {
            frames = 0;
            trav_to_target = !trav_to_target;
        }
        */

        transform.Rotate(1f,1f,Random.value,Space.Self);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyant : MonoBehaviour
{
    public Transform[] buoyant_pts;
    public float water_level;

    private Rigidbody rb;
    private float force_mult = 60f;

    void Start() {
        this.rb = GetComponent<Rigidbody>();
    }

    void Update() {
        for (int i = 0; i < this.buoyant_pts.Length; ++i) {
            Transform bp = this.buoyant_pts[i];
            float depth = water_level - bp.position.y;

            if (depth > 0) {
                Vector3 end = bp.position + new Vector3(0f,depth*2,0f);
                Debug.DrawLine(bp.position, end, Color.white);

                Vector3 fv = new Vector3(0f, Mathf.Pow(depth,1) * this.force_mult, 0f);
                rb.AddForceAtPosition(fv, bp.position, ForceMode.Force);
            }
        }
    }
}

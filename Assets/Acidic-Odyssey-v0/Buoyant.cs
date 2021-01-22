using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyant : MonoBehaviour
{
    public float water_level;

    private Rigidbody rb;

    public int buoyant_mode = 1; // 0=pts, 1=objs

    public float force_mult_0 = 60f;
    public Transform[] buoyant_pts;

    public float force_mult_1 = 1.6f;
    public GameObject[] buoyant_objs;

    // These variables are used for buoyant_objs caching.
    private float[] radii;
    private float[] volumes;

    // Converts a linear percentage (between top and bottom) to factor that can be multiplied by
    // volume to approximate volume below surface.
    // @param pc: linear percentage of sphere that is submerged
    // @return float: "sinusoidal" percentage of submerged volume approximating a sphere.
    private float VolumePCFromLinearPC(float pc) {
        if (pc > 0) {
            if (pc < 1) {
                return (Mathf.Sin(-Mathf.PI/2 + pc*Mathf.PI) + 1)/2;
            }
            return 1 + (pc - 1)/2f;
        }
        return 0;
    }

    void Start() {
        this.rb = GetComponent<Rigidbody>();

        this.radii = new float[this.buoyant_objs.Length];
        this.volumes = new float[this.buoyant_objs.Length];
        for (int i = 0; i < this.buoyant_objs.Length; ++i) {
            this.radii[i] = this.buoyant_objs[i].GetComponent<Buoyant_Point>().radius;
            this.volumes[i] = 4/3 * Mathf.PI * Mathf.Pow(this.radii[i],3);
        }
    }

    void Update() {
        if (this.buoyant_mode == 0) {
            for (int i = 0; i < this.buoyant_pts.Length; ++i) {
                Transform bp = this.buoyant_pts[i];
                float depth = this.water_level - bp.position.y;

                if (depth > 0) {
                    Vector3 end = bp.position + new Vector3(0f,depth*2,0f);
                    Debug.DrawLine(bp.position, end, Color.white);

                    Vector3 fv = new Vector3(0f, Mathf.Pow(depth,1) * this.force_mult_0, 0f);
                    rb.AddForceAtPosition(fv, bp.position, ForceMode.Force);
                }
            }
        } else if (this.buoyant_mode == 1) {
            for (int i = 0; i < this.buoyant_objs.Length; ++i) {
                Vector3 center = this.buoyant_objs[i].transform.position;
                float radius = this.radii[i];
                Vector3 end = center + Vector3.up * radius;
                Debug.DrawLine(center, end, Color.white);

                float top = center.y + radius;
                float bottom = center.y - radius;
                float pc = (this.water_level - bottom) / (top - bottom);
                float vpc = VolumePCFromLinearPC(pc);
                float volume = this.volumes[i];
                float submerged_volume = volume * vpc;

                Vector3 fv = new Vector3(0f, submerged_volume * this.force_mult_1, 0f);
                rb.AddForceAtPosition(fv, center, ForceMode.Force);

                // Debug.DrawLine(center, center + Vector3.up * submerged_volume/10f, Color.blue);

                // TODO: ideally here, drag would be calculated
            }
        }
    }
}

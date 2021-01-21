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

    void Start() {
        this.rb = GetComponent<Rigidbody>();
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
                float radius = this.buoyant_objs[i].GetComponent<Buoyant_Point>().radius;
                Vector3 end = center + Vector3.up * radius;
                Debug.DrawLine(center, end, Color.white);

                float d = this.water_level - center.y;
                float volume = 4/3 * Mathf.PI * Mathf.Pow(radius,3);

                // Case 1: fully submerged (d > r && d >= 0)
                // Case 2: mostly submerged (d < r && d >= 0)
                // Case 3: slightly submerged (d > -r && d < 0)
                // Case 4: airborne (d < -r && d < 0)

                float submerged_volume = 0;
                Color color = Color.white;
                if (d >= 0) {
                    if (d > radius) { // fully submerged
                        submerged_volume = volume; // TODO: consider depth?
                        color = Color.blue;

                        // TODO: why is correction factor needed??
                        submerged_volume += 66;
                    } else { // mostly submerged
                        float h = radius - d;
                        float cap = Mathf.PI * Mathf.Pow(h,2) / 3 * (3*radius - h);
                        submerged_volume = volume - cap;
                        color = Color.red;
                        // Debug.Log("red,cap=" + cap);

                        // TODO: why is correction factor needed??
                        submerged_volume += 66;
                    }
                } else { // d < 0
                    if (d > -radius) { // slightly submerged
                        float h = radius + d;
                        submerged_volume = Mathf.PI * Mathf.Pow(h,2) / 3 * (3*radius - h);
                        color = Color.green;
                        // Debug.Log("green,cap=" + submerged_volume);
                    } else { // Nothing. Airborne.
                    }
                }
                
                Vector3 fv = new Vector3(0f, submerged_volume * this.force_mult_1, 0f);
                rb.AddForceAtPosition(fv, center, ForceMode.Force);

                Debug.DrawLine(center, center + Vector3.up * submerged_volume, color);
            }
        }
    }
}

using System;//.Collections;
//using System.Collections.Generic;
using UnityEngine;


public class ColorChangeJ : MonoBehaviour
{

    public float forceMultiplier;

    public void Start()
    {

    }

    public void Update()
    {
      Vector3 inputMovementV = new Vector3(0,1,0) * Input.GetAxisRaw("Vertical");
      Vector3 inputMovementH = new Vector3(0,0,1)  * Input.GetAxisRaw("Horizontal");
      gameObject.GetComponent<Rigidbody>().AddForce((inputMovementV+inputMovementH) * forceMultiplier);
      //Debug.Log((inputMovementV+inputMovementH) * forceMultiplier);

    }

    private void OnCollisionEnter(Collision cold_)
    {
      Color randomColor = GetRandomColor();
      GetComponent<Renderer>().material.color = randomColor;
      Debug.Log(cold_.collider);
    }


    private Color GetRandomColor()
    {
      return new Color(
        r:UnityEngine.Random.Range(0f,1f),
        g:UnityEngine.Random.Range(0f,1f),
        b:UnityEngine.Random.Range(0f,1f));
    }

    private void OnTriggerEnter(Collider col_)
    {
      //col_.gameObject.GetComponent<Renderer>().material.color = GetRandomColor();
      //gameObject.GetComponent<Rigidbody>().AddForce(0,1000,0);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boosterPadForceAdder : MonoBehaviour
{
    public float forceMultiplier = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
      other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * forceMultiplier);
    }
}

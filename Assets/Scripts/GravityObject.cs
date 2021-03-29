using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public float mass;
    public float radius;
    public Vector3 velocity;
    public Vector3 acceleration;

    // Start is called before the first frame update
    void Start() {
        if (radius == 0) {
            radius = gameObject.transform.localScale.magnitude;
        } else {
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

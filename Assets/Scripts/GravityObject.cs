using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour
{
    public float mass;
    public float radius;
    public Vector3 velocity;
    public Vector3 acceleration;
    Transform transform;


    // Start is called before the first frame update
    void Start() {
        transform = gameObject.transform;
        if (radius == 0) {
            radius = gameObject.transform.localScale.magnitude;
        } else {
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
        }
        if (mass == 0) {
            mass = 0.01f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(radius, radius, radius);
    }
}

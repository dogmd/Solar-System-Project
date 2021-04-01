using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour
{
    public double mass;
    public float radius;
    public Vector3d position;
    [HideInInspector]
    public Vector3d velocity;
    public Vector3d initialVelocity;


    // Start is called before the first frame update
    void Start() {
        position = new Vector3d(transform.position);
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
        if (!Application.isPlaying) {
            position = new Vector3d(transform.localPosition);
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
        }
    }
}

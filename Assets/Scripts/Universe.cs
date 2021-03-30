using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public GravityObject[] objects;
    public float gravitationalConstant = 1.93f;
    public float timeStep = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = timeStep;
        objects = new GravityObject[transform.childCount];

        int i = 0;
        foreach (Transform t in transform) {
            objects[i++] = t.GetComponent<GravityObject>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GravityObject obj in objects) {
            Vector3 force = new Vector2(0, 0);
            Vector3 position = obj.transform.position;
            foreach (GravityObject other in objects) {
                if (other != obj) {
                    Vector3 heading = (position - other.transform.position);
                    float forceMag = CalcGravity(obj.mass, other.mass, heading.magnitude);
                    Vector3 forceDirection = heading / heading.magnitude;
                    force += forceDirection * -forceMag;
                }
            }
            obj.acceleration = (force / obj.mass);

            obj.velocity += obj.acceleration * timeStep;
            obj.gameObject.transform.position += obj.velocity * timeStep;
        }
    }

    public float CalcGravity(float m1, float m2, float r) {
        if (r < 0.01) {
            r = 0.01f;
        }
        return gravitationalConstant * m1 * m2 / (r * r);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    private GravityObject[] objects;
    public double gravitationalConstant = 1.93d;
    public float timeStep = 0.01f;
    public double runSpeedFactor = 1d;

    void Start()
    {
        Time.fixedDeltaTime = timeStep;
        objects = new GravityObject[transform.childCount];

        int i = 0;
        foreach (Transform t in transform) {
            objects[i] = t.GetComponent<GravityObject>();
            objects[i].velocity = objects[i].initialVelocity * runSpeedFactor;
            i++;
        }
    }

    void FixedUpdate()
    {
        foreach (GravityObject obj in objects) {
            Vector3d force = new Vector3d(Vector3.zero);
            Vector3d position = obj.position;
            foreach (GravityObject other in objects) {
                if (other != obj) {
                    Vector3d heading = (position - other.position);
                    double forceMag = CalcGravity(obj.mass, other.mass, heading.magnitude);
                    Vector3d forceDirection = heading / heading.magnitude;
                    force += forceDirection * -forceMag;
                }
            }
            Vector3d acceleration = (force / obj.mass);

            obj.velocity += acceleration * timeStep;
            obj.position += obj.velocity * timeStep;
            obj.transform.localPosition = transform.TransformPoint(Mathd.GetFloatVector3(obj.position));
        }
    }

    public double CalcGravity(double m1, double m2, double r) {
        if (r < 0.01) {
            r = 0.01f;
        }
        return gravitationalConstant * runSpeedFactor * runSpeedFactor * m1 * m2 / (r * r);
    }
}

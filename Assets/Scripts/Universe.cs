using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public static readonly double AU = 149597870.7;
    public static readonly double DAY = 86400.0;
    public static readonly double SCALE = 1000 / AU;
    public static readonly double VEL_SCALE = AU / DAY;

    private GravityObject[] objects;
    public double gravitationalConstant = 6674.07989501953;
    public float timeStep = 0.01f;
    public float distanceScale = 1f;
    public float sizeScale = 1f;
    public double runSpeedFactor = 1d;

    void Start() {
        Time.fixedDeltaTime = timeStep;
        objects = new GravityObject[transform.childCount];

        int i = 0;
        foreach (Transform t in transform) {
            objects[i] = t.GetComponent<GravityObject>();
            objects[i].velocity = objects[i].Velocity * Universe.VEL_SCALE * runSpeedFactor;
            objects[i].position = objects[i].Position * Universe.AU;
            i++;
        }
    }

    void FixedUpdate() {
        foreach (GravityObject obj in objects) {
            Vector3d force = new Vector3d(Vector3.zero);
            foreach (GravityObject other in objects) {
                if (other != obj) {
                    Vector3d heading = (obj.position - other.position);
                    double forceMag = CalcGravity(obj.mass, other.mass, heading.magnitude);
                    Vector3d forceDirection = heading / heading.magnitude;
                    force += forceDirection * -forceMag;
                }
            }
            Vector3d acceleration = (force / obj.mass);

            obj.velocity += acceleration * timeStep;
            obj.position += obj.velocity * timeStep;
            obj.transform.localPosition = transform.TransformPoint(Mathd.GetDisplayVector3(obj.position, obj));
        }
    }

    public double CalcGravity(double m1, double m2, double r) {
        if (r < 0.01) {
            r = 0.01f;
        }
        return gravitationalConstant * runSpeedFactor * runSpeedFactor * m1 * m2 / (r * r);
    }
}

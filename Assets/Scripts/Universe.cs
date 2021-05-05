using System;
using UnityEngine;

[RequireComponent(typeof(PredictedOrbitDisplay))]
public class Universe : MonoBehaviour {
    public static readonly double AU = 149597870.7;
    public static readonly double DAY = 86400.0;
    public static readonly double SCALE = 10000 / AU;
    public static readonly double VEL_SCALE = AU / DAY;

    [HideInInspector]
    public GravityObject[] gravityObjects;
    public double gravitationalConstant = 6674.07989501953;
    public float timeStep = 0.01f;
    public double distanceScale = 1d;
    public double sizeScale = 1d;
    public double runSpeedFactor = 1d;
    public Vector3d worldOffset = Vector3d.zero;
    public UniverseSettings activeSettings;
    public bool toScale = false;

    void Start() {
        Init();
    }

    public void Init() {
        Time.fixedDeltaTime = timeStep;
        gravityObjects = GetComponentsInChildren<GravityObject>();

        foreach (GravityObject obj in gravityObjects) {
            obj.velocity = obj.initVel * Universe.VEL_SCALE * runSpeedFactor;
            obj.position = obj.initPos * Universe.AU;
        }
    }

    void FixedUpdate() {
        foreach (GravityObject obj in gravityObjects) {
            Vector3d force = new Vector3d(Vector3.zero);
            foreach (GravityObject other in gravityObjects) {
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
        }
    }

    void Update() { }

    public double CalcGravity(double m1, double m2, double r) {
        if (r < 0.01) {
            r = 0.01f;
        }
        return gravitationalConstant * runSpeedFactor * runSpeedFactor * m1 * m2 / (r * r);
    }

    public void OffsetTrails(Vector3 offset) {
        Vector3 test = ExtensionMethods.Round(offset);
        if (test != Vector3.zero) {
            foreach (GravityObject obj in gravityObjects) {
                TrailRenderer tr = obj.tr;
                tr.AddPosition(obj.transform.position);
                Vector3[] positions = new Vector3[tr.positionCount];
                tr.GetPositions(positions);

                for (int i = 0; i < tr.positionCount; i++) {
                    positions[i] += offset;
                }
                tr.SetPositions(positions);
            }
        }
    }
}

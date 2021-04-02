using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Universe))]
public class PredictedOrbitDisplay : MonoBehaviour {
    public int numSteps = 1024;
    public float timeStep = 0.1f;
    public bool useUniverseTime = false;
    public bool relativeToObj;
    public GravityObject centralBody;
    public bool enableSimulation;
    Universe universe;

    void Start() {
        universe = gameObject.transform.GetComponent<Universe>();
        if (Application.isPlaying) {
            enableSimulation = false;
        }
    }

    void Update() {
        if (!Application.isPlaying && enableSimulation) {
            DrawOrbits();
        }
    }

    void DrawOrbits() {
        if (useUniverseTime) {
            timeStep = universe.timeStep;
        }
        GravityObject[] gravObjs = GetComponentsInChildren<GravityObject>();
        OrbitSimObject[] simObjs = new OrbitSimObject[gravObjs.Length];
        Vector3 initalRefPos = Vector3.zero;
        int refIndex = 0;
        var orbitPoints = new Vector3[gravObjs.Length][];

        // initialize arrays and simulation objects
        for (int i = 0; i < gravObjs.Length; i++) {
            simObjs[i] = new OrbitSimObject(gravObjs[i], universe.runSpeedFactor);
            orbitPoints[i] = new Vector3[numSteps];

            if (gravObjs[i] == centralBody) {
                refIndex = i;
                initalRefPos = transform.TransformPoint(Mathd.GetDisplayVector3(simObjs[i].position, gravObjs[i]));
            }
        }

        // simulate
        for (int i = 0; i < numSteps; i++) {
            Vector3 newRefPos = transform.TransformPoint(Mathd.GetDisplayVector3(simObjs[refIndex].position, gravObjs[refIndex]));
            for (int j = 0; j < simObjs.Length; j++) {
                OrbitSimObject obj = simObjs[j];
                Vector3d force = CalcForce(obj, simObjs);
                Vector3d acceleration = (force / obj.mass);

                obj.velocity += acceleration * timeStep;
                obj.position += obj.velocity * timeStep;

                Vector3 drawPoint = transform.TransformPoint(Mathd.GetDisplayVector3(obj.position, gravObjs[j]));
                if (relativeToObj) {
                    drawPoint -= (newRefPos - initalRefPos);
                }
                if (relativeToObj && j == refIndex) {
                    drawPoint = initalRefPos;
                }
                orbitPoints[j][i] = drawPoint;
            }
        }

        // render
        for (int i = 0; i < orbitPoints.Length; i++) {
            for (int j = 0; j < orbitPoints[i].Length - 1; j++) {
                Debug.DrawLine(orbitPoints[i][j], orbitPoints[i][j + 1], gravObjs[i].gameObject.GetComponent<MeshRenderer>().sharedMaterial.color);
            }
        }
    }

    Vector3d CalcForce(OrbitSimObject obj, OrbitSimObject[] others) {
        Vector3d force = Vector3d.zero;
        foreach (OrbitSimObject other in others) {
            if (other != obj) {
                Vector3d heading = (obj.position - other.position);
                double forceMag = universe.CalcGravity(obj.mass, other.mass, heading.magnitude);
                Vector3d forceDirection = heading / heading.magnitude;                
                force += forceDirection * -forceMag;
            }
        }
        return force;
    }

    // use this class so actual objects aren't affected
    class OrbitSimObject {
        public Vector3d position;
        public Vector3d velocity;
        public double mass;
        public string name;

        public OrbitSimObject(GravityObject obj, double runSpeedFactor) {
            position = obj.position;
            velocity = obj.velocity * runSpeedFactor;
            mass = obj.mass;
            name = obj.gameObject.name;
        }
    }
}

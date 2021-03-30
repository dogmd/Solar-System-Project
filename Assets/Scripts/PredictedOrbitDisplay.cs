using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
            simObjs[i] = new OrbitSimObject(gravObjs[i]);
            orbitPoints[i] = new Vector3[numSteps];

            if (gravObjs[i] == centralBody) {
                refIndex = i;
                initalRefPos = simObjs[i].position;
            }
        }

        // simulate
        for (int i = 0; i < numSteps; i++) {
            Vector3 newRefPos = simObjs[refIndex].position;
            for (int j = 0; j < simObjs.Length; j++) {
                OrbitSimObject obj = simObjs[j];
                Vector3 force = CalcForce(obj, simObjs);
                Vector3 acceleration = (force / obj.mass);
                obj.velocity += acceleration * timeStep;
                obj.position += obj.velocity * timeStep;

                Vector3 drawPoint = obj.position;
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
                Debug.DrawLine(orbitPoints[i][j], orbitPoints[i][j + 1], Color.white);
            }
        }
    }

    Vector3 CalcForce(OrbitSimObject obj, OrbitSimObject[] others) {
        Vector3 force = Vector3.zero;
        foreach (OrbitSimObject other in others) {
            if (other != obj) {
                Vector3 heading = (obj.position - other.position);
                float forceMag = universe.CalcGravity(obj.mass, other.mass, heading.magnitude);
                Vector3 forceDirection = heading / heading.magnitude;
                force += forceDirection * -forceMag;
            }
        }
        return force;
    }

    // use this class so actual objects aren't affected
    class OrbitSimObject {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public OrbitSimObject(GravityObject obj) {
            position = obj.transform.position;
            velocity = obj.velocity;
            mass = obj.mass;
        }
    }
}

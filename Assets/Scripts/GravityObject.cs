using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour {
    // Values used in simulation
    public double mass;
    private float _GameWorldRadius;
    private double oldRadius;
    private double oldSizeScale;
    public double radius;
    [HideInInspector]
    public Vector3d position;
    [HideInInspector]
    public Vector3d velocity;
    
    // Values used for input
    public Vector3d initPos;
    public Vector3d initVel;
    [HideInInspector]
    public Universe universe;
    public bool useCustomSizeScale = false;
    public bool useCustomDistanceScale = false;
    public double sizeScale = 1d;
    public double distanceScale = 1d;
    public int trailLength = -1;
    private Vector3d[] trail;
    private int trailInd;
    [HideInInspector]
    public Color color;

    void SetUniverse() {
        Transform curr = transform;
        while (!(universe = curr.parent.GetComponent<Universe>())) {
            curr = curr.parent;
        }
    }
    
    void Start() {
        color = Random.ColorHSV();
        SetUniverse();
        if (radius == 0) {
            radius = gameObject.transform.localScale.magnitude;
        }
        SetScale();

        if (trailLength == -1) {
            PredictedOrbitDisplay orbitSim = universe.GetComponent<PredictedOrbitDisplay>();
            trailLength = (int)(orbitSim.numSteps * universe.timeStep / orbitSim.timeStep);
        }
        trail = new Vector3d[trailLength];

        if (mass == 0) {
            mass = 0.01d;
        }
    }

    void SetScale() {
        Transform transform = this.gameObject.transform;
        bool multiTransfom = false;
        if (name == "Saturn") {
            foreach (Transform t in transform.GetComponentInChildren<Transform>()) {
                if (t.gameObject.name == name) {
                    transform = t;
                    multiTransfom = true;
                    break;
                }
            }
        }

        double rescale = -1, maxSize = -1;
        for (int i = 0; i < 3; i++) {
            double size = transform.GetComponent<MeshRenderer>().bounds.size[i];
            if (size == 0) {
                size = 0.000001d;
            }
            if (maxSize < size) {
                maxSize = size;
                rescale = transform.localScale[i];
            }
        }
        rescale = 2 * (radius * Universe.SCALE * sizeScale) * rescale / maxSize;
        if (rescale == 0) {
            rescale = 0.000001d;
        }
        if (multiTransfom) {
            foreach (Transform t in transform.parent.GetComponentInChildren<Transform>()) {
                t.localScale = new Vector3((float)rescale, (float)rescale, (float)rescale);
            }
        } else {
            transform.localScale = new Vector3((float)rescale, (float)rescale, (float)rescale);
        }

        Light light = transform.GetComponent<Light>();
        if (light) {
            light.range = (float)(radius * distanceScale);
        }
    }

    void Update() {
        SetUniverse();
        SetScale();
        if (!Application.isPlaying) {
            this.position = initPos * Universe.AU;
            this.velocity = initVel * Universe.VEL_SCALE;
        } else {
            // handle trail
            trail[trailInd] = position;
            // for (int i = 0; i < trailLength - 1; i++) {
            //     int ind = (trailInd - i + trailLength) % trailLength;
            //     if (ind - 1 > 0 && trail[ind] != null && trail[ind - 1] != null) {
            //         Vector3 a = Mathd.GetDisplayVector3(trail[ind], this) + universe.worldOffset;
            //         Vector3 b = Mathd.GetDisplayVector3(trail[ind - 1], this) + universe.worldOffset;
            //         Debug.DrawLine(a, b, gameObject.GetComponent<MeshRenderer>().sharedMaterial.color);
            //     }
            // }
            trailInd++;
            trailInd %= trailLength;
        }

        if (!useCustomDistanceScale) {
            distanceScale = universe.distanceScale;
        }
        if (!useCustomSizeScale) {
            sizeScale = universe.sizeScale;
        }

        if (oldRadius != radius || oldSizeScale != sizeScale) {
            _GameWorldRadius = (float)(radius * Universe.SCALE * sizeScale);
        }
        oldRadius = radius;
        oldSizeScale = sizeScale;
    }
        
    public Vector3d GameWorldPos {
        get {
            return ScaledPos + universe.worldOffset;
        }
    }

    public Vector3d ScaledPos {
        get {
            return position * this.distanceScale * Universe.SCALE;
        }
    }

    public float GameWorldRadius {
        get {
            return _GameWorldRadius;
        }
    }

    void TeleportTo(Camera cam) {
        //cam.transform.position.x = position * Universe.SCALE * distanceScale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TrailRenderer))]
public class GravityObject : MonoBehaviour {
    // Values used in simulation
    public double mass;
    public double radius;
    public double siderealPeriod = 0;
    public double obliquity = 0;
    public Vector3d position;
    public Vector3d velocity;
    public Vector3 axis;
    public double rotation;
    
    // Values used for input
    public Vector3d initPos;
    public Vector3d initVel;
    public Universe universe;
    public bool useCustomSizeScale = false;
    public bool useCustomDistanceScale = false;
    public double sizeScale = 1d;
    public double distanceScale = 1d;
    private int trailInd;
    public Color color;
    public TrailRenderer tr;

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

        if (mass == 0) {
            mass = 0.01d;
        }

        tr = GetComponentInChildren<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = color;
        tr.endColor = color;
        tr.widthCurve = AnimationCurve.Linear(0, 1, 1, 0);
        tr.emitting = true;
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

        // undo rotation
        Vector3 eAngs = transform.eulerAngles;
        transform.eulerAngles = Vector3.zero;

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
        rescale = 2 * (radius * Universe.SCALE * SizeScale) * rescale / maxSize;
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
            light.range = (float)(radius * DistanceScale);
        }

        // redo rotation
        transform.eulerAngles = eAngs;
    }

    void SetAxis() {
        Vector3 vel = Mathd.GetFloatVector3(velocity).normalized;
        axis = Quaternion.AngleAxis((float)obliquity, vel) * Vector3.Cross(vel, Vector3.up);//Vector3.Cross(vel, Vector3.up + new Vector3((float)obliquity, 0, 0));
        if (name == "Saturn") {
            foreach(Transform t in transform.GetComponentInChildren<Transform>()) {
                t.up = axis;
            }
        } else { 
            transform.up = axis;
        }

        // Draw debug lines
        if (this == transform.root.GetComponentInChildren<FollowCamera>().referenceBody) {
            float r = (float)radius;
            Vector3 orthogonal = Vector3.Cross(vel, Vector3.forward);
            Debug.DrawLine(transform.position, transform.position + axis * (float)SizeScale * r, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + vel * (float)SizeScale* r, Color.white);
            Debug.DrawLine(transform.position, transform.position + orthogonal * (float)SizeScale* r, Color.red);
        }
    }

    void Rotate() {
        if (siderealPeriod == 0) {
            return;
        }

        double degOff = universe.timeStep * universe.runSpeedFactor / (siderealPeriod * 24 * 60 * 60) * 360;
        if (Application.isPlaying) {
            rotation += degOff;
            //rotation %= 360;

            if (name == "Saturn") {
                foreach(Transform t in transform.GetComponentInChildren<Transform>()) {
                    t.up = axis;
                    t.RotateAround(t.position, axis, (float)rotation);
                }
            } else {
                transform.up = axis;
                transform.RotateAround(transform.position, axis, (float)rotation);
            }
        }
    }

    void Update() {
        tr.time = 1 * (float)(100000 / universe.runSpeedFactor * DistanceScale);
        tr.widthMultiplier = GameWorldRadius;

        SetUniverse();
        SetScale();
        if (axis == Vector3.zero || !Application.isPlaying) {
            SetAxis();
        }
        Rotate();
        
        if (!Application.isPlaying) {
            this.position = initPos * Universe.AU;
            this.velocity = initVel * Universe.VEL_SCALE;
        }

        if (Time.frameCount < 3) {
            tr.Clear();
        }
    }
        
    public Vector3d GameWorldPos {
        get {
            return ScaledPos + universe.worldOffset;
        }
    }

    public Vector3d ScaledPos {
        get {
            return position * this.DistanceScale * Universe.SCALE;
        }
    }

    public float GameWorldRadius {
        get {
            return (float)(radius * Universe.SCALE * SizeScale);
        }
    }

    public double SizeScale {
        get {
            if (useCustomSizeScale) {
                return sizeScale;
            }
            return universe.sizeScale;
        }
    }

    public double DistanceScale {
        get {
            if (useCustomDistanceScale) {
                return distanceScale;
            }
            return universe.distanceScale;
        }
    }
}

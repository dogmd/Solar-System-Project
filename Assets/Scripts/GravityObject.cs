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
    public GravityObject parent;

    void SetUniverse() {
        Transform curr = transform;
        while (!(universe = curr.parent.GetComponent<Universe>())) {
            curr = curr.parent;
        }
    }

    void Start() {
        SetUniverse();
        if (radius == 0) {
            radius = gameObject.transform.localScale.magnitude;
        }
        SetScale();

        if (mass == 0) {
            mass = 0.01d;
        }

        SetTrail();
        if (transform.parent != universe.transform) {
            GravityObject[] siblings = transform.parent.GetComponentsInChildren<GravityObject>();
            foreach (GravityObject sibling in siblings) {
                if (sibling.name == transform.parent.gameObject.name) {
                    parent = sibling;
                    break;
                }
            }
        }

        FindObjectOfType<CosmicCamera>().queueTrailReset = true;
    }



    public float GetScale(Transform transform, double desiredSize) {
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
        rescale = desiredSize * rescale / maxSize;

        // redo rotation
        transform.eulerAngles = eAngs;

        return (float)rescale;
    }

    void SetTrail() {
        tr = GetComponentInChildren<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = color;
        tr.endColor = color;
        tr.widthCurve = AnimationCurve.Linear(0, 1, 1, 0);
        tr.emitting = name != "Sun";
        tr.minVertexDistance = (float)DistanceScale / 100;

        if (!tr.emitting) {
            tr.Clear();
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

        float rescale = GetScale(transform, 2 * (radius * Universe.SCALE * SizeScale));
        if (rescale == 0) {
            rescale = 0.00001f;
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
    }

    void SetAxis() {
        Vector3 vel = velocity.normalized.ToFloat();
        axis = Quaternion.AngleAxis((float)obliquity, vel) * Vector3.Cross(vel, Vector3.up);
        if (name == "Saturn") {
            foreach (Transform t in transform.GetComponentInChildren<Transform>()) {
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
            Debug.DrawLine(transform.position, transform.position + vel * (float)SizeScale * r, Color.white);
            Debug.DrawLine(transform.position, transform.position + orthogonal * (float)SizeScale * r, Color.red);
        }
    }

    void Rotate() {
        if (siderealPeriod == 0) {
            return;
        }

        double degOff = Time.deltaTime * universe.runSpeedFactor / (siderealPeriod * 24 * 60 * 60) * 360;
        if (Application.isPlaying) {
            rotation += degOff;
            rotation %= 360;

            if (name == "Saturn") {
                foreach (Transform t in transform.GetComponentInChildren<Transform>()) {
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
        tr.time = 3500000f / (float)(universe.runSpeedFactor);
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
    }

    public Vector3d GameWorldPos {
        get {
            return ScaledPos + universe.worldOffset;
        }
    }

    public Vector3d ScaledPos {
        get {
            if (parent && parent != this) {
                Vector3d offset = (parent.position - position) * Universe.SCALE;
                return parent.ScaledPos + offset * DistanceScale;
            } else {
                return position * Universe.SCALE * DistanceScale;
            }
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

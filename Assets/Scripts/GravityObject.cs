using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour {
    // Values used in simulation
    public double mass;
    public float radius;
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
    public double sizeScale = 1f;
    public double distanceScale = 1f;

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
        gameObject.transform.localScale = new Vector3(GameWorldRadius, GameWorldRadius, GameWorldRadius);


        if (mass == 0) {
            mass = 0.01d;
        }

    }

    void Update() {
        if (!Application.isPlaying) {
            transform.localPosition = Mathd.GetFloatVector3(initPos * Universe.AU * Universe.SCALE * distanceScale);

            this.position = initPos * Universe.AU;
            this.velocity = initVel * Universe.VEL_SCALE;            
        }

        gameObject.transform.localScale = new Vector3(GameWorldRadius, GameWorldRadius, GameWorldRadius);
        if (!useCustomDistanceScale) {
            distanceScale = universe.distanceScale;
        }
        if (!useCustomSizeScale) {
            sizeScale = universe.sizeScale;
        }
    }

    public Vector3 GameWorldPos {
        get {
            return Mathd.GetFloatVector3(position * Universe.SCALE * distanceScale);
        }
    }

    public float GameWorldRadius {
        get {
            return (float)(radius * Universe.SCALE * sizeScale);
        }
    }

    void TeleportTo(Camera cam) {
        //cam.transform.position.x = position * Universe.SCALE * distanceScale;
    }
}

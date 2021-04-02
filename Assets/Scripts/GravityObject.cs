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
        gameObject.transform.localScale = new Vector3(GameWorldRadius * 2, GameWorldRadius * 2, GameWorldRadius * 2);


        if (mass == 0) {
            mass = 0.01d;
        }

    }

    void Update() {
        if (!Application.isPlaying) {
            this.position = initPos * Universe.AU;
            this.velocity = initVel * Universe.VEL_SCALE;            
        }

        gameObject.transform.localScale = new Vector3(GameWorldRadius * 2, GameWorldRadius * 2, GameWorldRadius * 2);
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
        
    public Vector3 GameWorldPos {
        get {
            return ScaledPos + universe.worldOffset;
        }
    }

    public Vector3 ScaledPos {
        get {
            return Mathd.GetDisplayVector3(position, this);
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

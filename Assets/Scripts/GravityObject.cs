using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour
{
    // Values used in simulation
    public double mass;
    public float radius;
    [HideInInspector]
    public Vector3d position;
    [HideInInspector]
    public Vector3d velocity;
    
    // Values used for input
    public Vector3d Position;
    public Vector3d Velocity;
    private Universe universe;
    public bool useUniverseSizeScale = true;
    public bool useUniverseDistanceScale = true;
    public double sizeScale = 1f;
    public double distanceScale = 1f;

    void Start() {
        universe = transform.parent.gameObject.transform.GetComponent<Universe>();

        if (radius == 0) {
            radius = gameObject.transform.localScale.magnitude;
        }
        float r = (float)(radius * Universe.SCALE * sizeScale);
        gameObject.transform.localScale = new Vector3(r, r, r);

        if (mass == 0) {
            mass = 0.01d;
        }

    }

    void Update() {
        if (!Application.isPlaying) {
            if (useUniverseDistanceScale) {
                distanceScale = universe.distanceScale;
            }
            if (useUniverseSizeScale) {
                sizeScale = universe.sizeScale;
            }

            transform.localPosition = Mathd.GetFloatVector3(Position * Universe.AU * Universe.SCALE * distanceScale);
            this.position = Position * Universe.AU;
            this.velocity = Velocity * Universe.VEL_SCALE;
            
            float r = (float)(radius * Universe.SCALE * sizeScale);
            gameObject.transform.localScale = new Vector3(r, r, r);
        }
    }
}

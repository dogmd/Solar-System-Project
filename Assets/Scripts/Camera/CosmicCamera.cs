using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyCamera))]
[RequireComponent(typeof(FollowCamera))]
[RequireComponent(typeof(CameraControls))]

[ExecuteInEditMode]
public class CosmicCamera : MonoBehaviour {
    public FlyCamera flyCam;
    public FollowCamera followCam;
    public CameraControls camControls;
    public Vector3 position;
    public Vector3 rotation;
    public bool queueTrailReset = false;

    void Start() {
        flyCam = gameObject.transform.GetComponent<FlyCamera>();
        followCam = gameObject.transform.GetComponent<FollowCamera>();
        camControls = gameObject.transform.GetComponent<CameraControls>();

        flyCam.active = false;
        followCam.active = true;
    }

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            flyCam.active = !flyCam.active;
            followCam.active = !followCam.active;
        }

        foreach (GravityObject obj in transform.parent.GetComponentsInChildren<GravityObject>()) {
            obj.transform.localPosition = obj.GameWorldPos.ToFloat();

            if (queueTrailReset) {
                obj.tr.Clear();
            }
        }
        queueTrailReset = false;
    }
}

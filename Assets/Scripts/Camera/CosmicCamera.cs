using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyCamera))]
[RequireComponent(typeof(FollowCamera))]
[RequireComponent(typeof(CameraControls))]

[ExecuteInEditMode]
public class CosmicCamera : MonoBehaviour {
    FlyCamera flyCam;
    FollowCamera followCam;
    CameraControls camControls;
    public Vector3 position;
    public Vector3 rotation;

    void Start() {
        flyCam = gameObject.transform.GetComponent<FlyCamera>();
        followCam = gameObject.transform.GetComponent<FollowCamera>();
        camControls = gameObject.transform.GetComponent<CameraControls>();

        flyCam.active = false;
        followCam.active = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            flyCam.active = !flyCam.active;
            followCam.active = !followCam.active;
        }

        foreach (GravityObject obj in transform.parent.GetComponentsInChildren<GravityObject>()) {
            obj.transform.localPosition = Mathd.GetFloatVector3(obj.GameWorldPos);
        }
    }
}

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
    Universe universe;
    public Vector3 position;
    public Vector3 rotation;

    void Start() {
        flyCam = gameObject.transform.GetComponent<FlyCamera>();
        followCam = gameObject.transform.GetComponent<FollowCamera>();
        camControls = gameObject.transform.GetComponent<CameraControls>();
        universe = transform.parent.GetComponent<Universe>();

        flyCam.enabled = false;
        followCam.enabled = true;
    }

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            flyCam.enabled = !flyCam.enabled;
            followCam.enabled = !followCam.enabled;
        }

        foreach (GravityObject obj in universe.objects) {
            obj.transform.localPosition = obj.GameWorldPos;
        }

        //gameObject.transform.position = Vector3.zero;
        // gameObject.transform.eulerAngles = Vector3.zero;
        // universe.worldOffset = -position;
        // universe.worldRotation = -rotation;
    }
}

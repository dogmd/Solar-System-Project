using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyCamera))]
[RequireComponent(typeof(FollowCamera))]
[RequireComponent(typeof(CameraControls))]
public class CosmicCamera : MonoBehaviour {
    FlyCamera flyCam;
    FollowCamera followCam;
    CameraControls camControls;

    void Start() {
        flyCam = gameObject.transform.GetComponent<FlyCamera>();
        followCam = gameObject.transform.GetComponent<FollowCamera>();
        camControls = gameObject.transform.GetComponent<CameraControls>();

        flyCam.enabled = false;
        followCam.enabled = true;
    }

    void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            flyCam.enabled = !flyCam.enabled;
            followCam.enabled = !followCam.enabled;
        }
    }
}

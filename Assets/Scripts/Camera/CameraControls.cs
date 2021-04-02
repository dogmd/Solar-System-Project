using UnityEngine;

public class CameraControls : MonoBehaviour {
    public Vector3 eulerOffset = new Vector3(0, 0, 0);
    public bool tracking = true;
    public float sens = 1f;
    public bool lockMouse = true;

    void Start() {}

    void Update() {
        if (lockMouse) {
            if (Input.GetKey(KeyCode.Tab)) {
                Cursor.lockState = CursorLockMode.None;
                tracking = false;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                tracking = true;
            }
        } else if (Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void UpdateCamera() {
        if (tracking) {
            Vector3 mouseOff = new Vector3(-Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
            eulerOffset += mouseOff;
            gameObject.transform.eulerAngles = eulerOffset;
        }
    }
}

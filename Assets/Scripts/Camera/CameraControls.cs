using UnityEngine;

public class CameraControls : MonoBehaviour {
    public Vector3 eulerOffset = Vector3.zero;
    public bool tracking = true;
    public float sens = 1f;
    public bool lockMouse = true;

    void Start() {}

    void Update() {
        if (lockMouse) {
            if (Input.GetKey(KeyCode.Tab)) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        } else if (Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void UpdateCamera() {
        Vector3 mouseOff = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        eulerOffset += mouseOff;
        gameObject.transform.eulerAngles = eulerOffset;
    }
}

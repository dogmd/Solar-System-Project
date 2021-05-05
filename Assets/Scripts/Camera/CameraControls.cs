using UnityEngine;

[ExecuteInEditMode]
public class CameraControls : MonoBehaviour {
    public Vector3 eulerOffset = new Vector3(0, 0, 0);
    public bool tracking = true;
    public float sens = 1f;
    public bool lockMouse = true;
    private Canvas canvas;
    void Start() {
        canvas = FindObjectOfType<Canvas>();
    }

    void Update() {
        if (lockMouse) {
            if (Input.GetKey(KeyCode.Tab)) {
                Cursor.lockState = CursorLockMode.None;
                tracking = false;
                canvas.enabled = true;
            } else if (Application.isPlaying) {
                Cursor.lockState = CursorLockMode.Locked;
                tracking = true;
                canvas.enabled = false;
            }
            eulerOffset = new Vector3(eulerOffset.x % 360, eulerOffset.y % 360, eulerOffset.z % 360);
        } else if (Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void UpdateCamera() {
        UpdateCamera(1, 1, 1);
    }

    public void UpdateCamera(float xSens, float ySens, float zSens) {
        if (tracking) {
            float xOff = Input.GetAxis("Mouse X");
            float yOff = Input.GetAxis("Mouse Y");
            Vector3 mouseOff = Vector3.zero;

            if (Input.GetMouseButton(1)) {
                mouseOff = new Vector3(0, 0, xOff * xSens);
            } else if (Input.GetMouseButton(0)) {
                mouseOff = Quaternion.AngleAxis(-eulerOffset.z, Vector3.forward) * new Vector3(yOff * ySens, xOff * xSens, 0);
            }

            eulerOffset += mouseOff;
            gameObject.transform.eulerAngles = eulerOffset;
        }
    }
}

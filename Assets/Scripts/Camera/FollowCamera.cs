using UnityEngine;

[RequireComponent(typeof(CameraControls))]
[RequireComponent(typeof(CosmicCamera))]
[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour {
    public GravityObject referenceBody;
    private GravityObject prevReferenceBody;
    private float zoomSpeed = 150f;
    public float zoom = 0;
    private CameraControls camControls;
    private CosmicCamera cosmicCam;
    public bool active;

    void Start() {
        camControls = transform.GetComponent<CameraControls>();
        cosmicCam = gameObject.transform.GetComponent<CosmicCamera>();
    }

    void Update() {
        if (active) {
            if (referenceBody) {
                zoomSpeed = referenceBody.GameWorldRadius * 10;

                Vector3 newPosition = transform.position;
                camControls.UpdateCamera();


                // Modified from http://answers.unity.com/answers/1536663/view.html
                float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");      
                zoom += ScrollWheelChange;
                float R = zoom * zoomSpeed - 0.01f;
                float PosX = transform.eulerAngles.x + 90; // Get up and down
                float PosY = -1 * (transform.eulerAngles.y - 90); // Get left to right
                
                // Convert from degrees to radians
                PosX = PosX / 180 * Mathf.PI;
                PosY = PosY / 180 * Mathf.PI;                   

                // Calculate new coords
                float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);
                float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);
                float Y = R * Mathf.Cos(PosX);               

                // Get current camera postition for the offset
                Vector3 offset = new Vector3(X, Y, Z);
                referenceBody.universe.worldOffset = -referenceBody.ScaledPos + newPosition - offset;
            }
            if (referenceBody != prevReferenceBody) {
                zoom = -1;
            }
            prevReferenceBody = referenceBody;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(CameraControls))]
[RequireComponent(typeof(CosmicCamera))]
[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour {
    public GravityObject referenceBody;
    private GravityObject prevReferenceBody;
    private double zoomSpeed = 150f;
    public double zoom = 0;
    private CameraControls camControls;
    private CosmicCamera cosmicCam;
    public bool active;
    public bool followRotation = true;

    void Start() {
        camControls = transform.GetComponent<CameraControls>();
        cosmicCam = gameObject.transform.GetComponent<CosmicCamera>();
    }

    void Update() {
        if (active) {
            if (referenceBody) {
                zoomSpeed = referenceBody.GameWorldRadius * 10;

                Vector3d newPosition = new Vector3d(transform.position);
                camControls.UpdateCamera();
                transform.up = referenceBody.axis;
                float zAng = Vector3.Angle(Vector3.forward, Mathd.GetFloatVector3(referenceBody.velocity).normalized);
                transform.up = Quaternion.AngleAxis(-zAng + camControls.eulerOffset.z, Vector3.forward) * transform.up;
                float zOff = 0;
                if (followRotation) {
                    zOff = (float)referenceBody.rotation;
                }

                float xAng = Vector3.Angle(transform.right, referenceBody.axis);
                float yAng = Vector3.Angle(transform.up, referenceBody.axis);
                transform.Rotate(new Vector3(-xAng + camControls.eulerOffset.x, zOff, -yAng + camControls.eulerOffset.y));

                // Modified from http://answers.unity.com/answers/1536663/view.html
                double ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");      
                zoom += ScrollWheelChange;
                double R = zoom * zoomSpeed - 0.05f;
                double PosX = transform.eulerAngles.x + 90; // Get up and down
                double PosY = -1 * (transform.eulerAngles.y - 90); // Get left to right
                
                // Convert from degrees to radians
                PosX = PosX / 180 * Mathd.PI;
                PosY = PosY / 180 * Mathd.PI;                   

                // Calculate new coords
                double X = R * Mathd.Sin(PosX) * Mathd.Cos(PosY);
                double Z = R * Mathd.Sin(PosX) * Mathd.Sin(PosY);
                double Y = R * Mathd.Cos(PosX);               

                // Get current camera postition for the offset
                Vector3d offset = new Vector3d(X, Y, Z);
                referenceBody.universe.worldOffset = -referenceBody.position * referenceBody.distanceScale * Universe.SCALE + newPosition - offset;
            }
            if (referenceBody != prevReferenceBody) {
                zoom = -1;
            }
            prevReferenceBody = referenceBody;
        }
    }
}

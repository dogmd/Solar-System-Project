using UnityEngine;
using System.Collections;
 
// From https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996, modified to be minecraft fly controls
[RequireComponent(typeof(Camera))]
public class FlyCamera : MonoBehaviour {  
    public float mainSpeed = 25.0f; //regular speed
    float ctrlMult = 12f;
    float ctrlAdd; //multiplied by how long ctrl is held.  Basically running
    float maxCtrl = 1000.0f; //Maximum speed when holding ctrl
    private float totalRun = 1.0f;
    private CameraControls camControls;

    void Start() {
        camControls = transform.GetComponent<CameraControls>();
    }

    void Update () {
        if (Application.isFocused && IsMouseOverGameWindow) {
            ctrlAdd = ctrlMult * mainSpeed;
            camControls.UpdateCamera();
        
            //Keyboard commands
            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftControl)) {
                totalRun += Time.deltaTime;
                p  = p * totalRun * ctrlAdd;
            } else {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }
        
            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;

            // x and z        
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
            
            // y
            Vector3 yOff = Vector3.zero;
            if (Input.GetKey(KeyCode.LeftShift)){ 
                yOff = new Vector3(0, -1, 0);
            }
            if (Input.GetKey(KeyCode.Space)){ 
                yOff = new Vector3(0, 1, 0);
            }
            if (Input.GetKey (KeyCode.LeftControl)){
                yOff = yOff * totalRun * ctrlAdd* Time.deltaTime;
                yOff.y = Mathf.Clamp(yOff.y, -maxCtrl, maxCtrl);
            } else {
                yOff *= mainSpeed * Time.deltaTime;
            }
            transform.Translate(yOff);
        }
    }
     
    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S)){
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.A)){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    bool IsMouseOverGameWindow { 
        get {
            return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); 
        } 
    }
}
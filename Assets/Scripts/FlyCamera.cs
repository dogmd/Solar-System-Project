using UnityEngine;
using System.Collections;
 
// From https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996, modified to be minecraft fly controls
public class FlyCamera : MonoBehaviour {
 
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    ctrl : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
     
     
    float mainSpeed = 100.0f; //regular speed
    float ctrlAdd = 250.0f; //multiplied by how long ctrl is held.  Basically running
    float maxCtrl = 1000.0f; //Maximum speed when holdin gctrl
    float camSens = 1f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun= 1.0f;
     
            
     bool IsMouseOverGameWindow { 
        get {
            return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); 
        } 
    }
    void Update () {
        if (Cursor.lockState == CursorLockMode.Locked) {
            if (Input.GetKey(KeyCode.Tab)) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }

            lastMouse = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse =  new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            //Mouse  camera angle done.  
        
            //Keyboard commands
            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftControl)) {
                totalRun += Time.deltaTime;
                p  = p * totalRun * ctrlAdd;
                p.x = Mathf.Clamp(p.x, -maxCtrl, maxCtrl);
                p.y = Mathf.Clamp(p.y, -maxCtrl, maxCtrl);
                p.z = Mathf.Clamp(p.z, -maxCtrl, maxCtrl);
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
        } else if (IsMouseOverGameWindow && Application.isFocused && !Input.GetKey(KeyCode.Tab)) {
            Cursor.lockState = CursorLockMode.Locked;
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
}
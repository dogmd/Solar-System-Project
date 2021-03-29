using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public GravityObject[] objects;
    public float gravitationalConstant = 1.93f;

    // Start is called before the first frame update
    void Start()
    {
        objects = new GravityObject[transform.childCount];

        int i = 0;
        foreach (Transform t in transform) {
            objects[i++] = t.GetComponent<GravityObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GravityObject obj in objects) {
            Vector3 force = new Vector2(0, 0);
            Vector3 position = obj.gameObject.transform.position;
            foreach (GravityObject other in objects) {
                if (other != obj) {
                    Vector3 heading = (position - other.gameObject.transform.position);
                    float forceMag = calcGravity(obj.mass, other.mass, heading.magnitude);
                    Vector3 forceDirection = heading / heading.magnitude;
                    force += forceDirection * -forceMag;
                }
            }
            obj.acceleration = force / obj.mass;

            obj.velocity += obj.acceleration;
            obj.gameObject.transform.position += obj.velocity;
        }
    }

    float calcGravity(float m1, float m2, float r) {
        if (r < 0.01) {
            r = 0.01f;
        }
        return gravitationalConstant * m1 * m2 / (r * r);
    }
}

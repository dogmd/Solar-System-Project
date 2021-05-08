using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[ExecuteInEditMode]
public class Label : MonoBehaviour {
    GravityObject obj;
    TMP_Text label;
    void Start() {
        SetLabel();
    }

    // Update is called once per frame
    void Update() {
        SetLabel();
    }

    void SetObj() {
        Transform curr = transform.parent;
        while (curr != null && !(obj = curr.GetComponent<GravityObject>())) { curr = curr.parent; }
    }

    void SetLabel() {
        label = transform.GetComponent<TMP_Text>();
        SetObj();

        if (label) {
            label.text = obj.name;
            label.autoSizeTextContainer = true;
            label.fontSize = (int)(obj.SizeScale * 10);

            label.rectTransform.localScale = new Vector3(1, 1, 1);
            float textHeight = (float)(obj.GameWorldRadius * 10 * 0.5d) * 30f;
            label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
            label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textHeight * 8);

            float scale = Mathf.Abs(GetScale(label.transform, obj.GameWorldRadius / 4, 1));
            label.rectTransform.localScale = new Vector3(scale, scale, scale);

            Transform transform = label.transform.parent;
            Vector3 eAngs = transform.localEulerAngles;
            transform.localEulerAngles = Vector3.zero;
            float height = transform.GetComponent<MeshFilter>().sharedMesh.bounds.size.y;
            label.rectTransform.localPosition = new Vector3(0, height, 0);
            transform.localEulerAngles = eAngs;

            CosmicCamera cam = obj.universe.GetComponentInChildren<CosmicCamera>();
            float rot = cam.camControls.eulerOffset.y;
            if (cam.followCam.active && !cam.followCam.followRotation) {
                rot -= (float)obj.rotation;
            }
        }
    }

    public float GetScale(Transform transform, double desiredSize, int axis) {
        // undo rotation
        Vector3 eAngs = transform.eulerAngles;
        transform.eulerAngles = Vector3.zero;

        double rescale = -1;
        double size = transform.GetComponent<MeshRenderer>().bounds.extents[axis];
        if (size == 0) {
            size = 0.000001d;
        }
        rescale = desiredSize * rescale / size;

        // redo rotation
        transform.eulerAngles = eAngs;

        return (float)rescale;
    }
}

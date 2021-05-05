using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UniverseSettings : MonoBehaviour {
    public double[] sizeScales;
    public double[] distanceScales;
    public Universe universe;
    public GravityObject referenceBody;


    void Start() {
        universe = FindObjectOfType<Universe>();
        Text text = GetComponentInChildren<Text>();
        name = referenceBody.name + " Button";
        text.text = referenceBody.name;
        text.color = Color.white;
        GetComponent<Button>().onClick.AddListener(WriteSettings);
        Color color;
        if (referenceBody.parent) {
            color = referenceBody.parent.color;
        } else {
            color = referenceBody.color;
        }
        GetComponent<Image>().color = color;
    }

    void Update() {
        if (!universe) {
            universe = FindObjectOfType<Universe>();
        }
    }

    public void WriteSettings() {
        double minScale = 99999999;
        foreach (double sizeScale in sizeScales) {
            if (sizeScale < minScale) {
                minScale = sizeScale;
            }
        }

        Debug.Log(referenceBody.tag);
        if (referenceBody.tag == "Small") {
            minScale /= 4000 / referenceBody.radius;
        }

        for (int i = 0; i < sizeScales.Length; i++) {
            GravityObject obj = universe.gravityObjects[i];
            obj.useCustomDistanceScale = true;
            obj.useCustomSizeScale = true;
            obj.sizeScale = sizeScales[i] / minScale;
            if (universe.toScale) {
                obj.distanceScale = obj.sizeScale;
            } else {
                obj.distanceScale = distanceScales[i] / minScale;
            }
            obj.tr.Clear();
        }
        universe.GetComponentInChildren<FollowCamera>().referenceBody = referenceBody;
        universe.activeSettings = this;
    }

    public void SaveSettings() {
        universe.Init();
        sizeScales = new double[universe.gravityObjects.Length];
        distanceScales = new double[universe.gravityObjects.Length];

        for (int i = 0; i < sizeScales.Length; i++) {
            GravityObject obj = universe.gravityObjects[i];
            sizeScales[i] = obj.SizeScale;
            distanceScales[i] = obj.DistanceScale;
        }

        referenceBody = universe.GetComponentInChildren<FollowCamera>().referenceBody;
    }
}

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
        if (referenceBody.transform.parent.name != "Asteroids/Dwarfs") {
            if (referenceBody.parent) {
                color = referenceBody.parent.color;
            } else {
                color = referenceBody.color;
            }
            GetComponent<Image>().color = color;
        }
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

        double smallScale = 1;
        if (referenceBody.tag == "Small") {
            smallScale = 3000 / referenceBody.radius;
        }

        for (int i = 0; i < sizeScales.Length; i++) {
            GravityObject obj = universe.gravityObjects[i];
            obj.useCustomDistanceScale = true;
            obj.useCustomSizeScale = true;
            if (universe.toScale) {
                obj.sizeScale = smallScale;
                obj.distanceScale = obj.sizeScale;
            } else {
                obj.sizeScale = sizeScales[i] / minScale * smallScale;
                obj.distanceScale = distanceScales[i] / minScale * smallScale;
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

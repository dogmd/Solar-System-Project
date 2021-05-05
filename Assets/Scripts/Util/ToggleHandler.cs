using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour {
    private Toggle toggle;
    Universe universe;

    void Start() {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        if (name == "Rotation Toggle") {
            toggle.onValueChanged.AddListener(SetFollowRotation);
        } else if (name == "Labels Toggle") {
            toggle.onValueChanged.AddListener(SetLabels);
        } else if (name == "Trails Toggle") {
            toggle.onValueChanged.AddListener(SetTrails);
        } else if (name == "To Scale Toggle") {
            toggle.onValueChanged.AddListener(SetToScale);
        }
        universe = FindObjectOfType<Universe>();
    }

    private void SetToScale(bool isOn) {
        universe.toScale = isOn;
        universe.activeSettings.WriteSettings();
    }

    private void SetTrails(bool isOn) {
        foreach (GravityObject obj in universe.gravityObjects) {
            obj.tr.enabled = isOn;
        }
    }

    private void SetLabels(bool isOn) {
        foreach (Label lbl in FindObjectsOfType<Label>()) {
            lbl.GetComponent<TMPro.TMP_Text>().enabled = isOn;
        }
    }

    private void SetFollowRotation(bool isOn) {
        FindObjectOfType<FollowCamera>().followRotation = isOn;
    }

    private void OnToggleValueChanged(bool isOn) {
        ColorBlock cb = toggle.colors;
        if (isOn) {
            cb.normalColor = Color.gray;
            cb.highlightedColor = Color.gray;
            cb.selectedColor = Color.gray;
        } else {
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
        }
        toggle.colors = cb;
    }
}

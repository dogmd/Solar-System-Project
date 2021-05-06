using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniverseSettingsToggle : MonoBehaviour {
    private Toggle toggle;

    void Start() {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(toggle.isOn);
    }

    public void SetToScale() {
        Universe universe = FindObjectOfType<Universe>();
        universe.toScale = toggle.isOn;
        universe.activeSettings.WriteSettings();
        FindObjectOfType<CosmicCamera>().queueTrailReset = true;
    }

    public void SetTrails() {
        Universe universe = FindObjectOfType<Universe>();
        foreach (GravityObject obj in universe.gravityObjects) {
            obj.tr.enabled = toggle.isOn;
        }
    }

    public void SetLabels() {
        foreach (Label lbl in FindObjectsOfType<Label>()) {
            lbl.GetComponent<TMPro.TMP_Text>().enabled = toggle.isOn;
        }
    }

    public void SetFollowRotation() {
        FindObjectOfType<FollowCamera>().followRotation = toggle.isOn;
    }

    public void OnToggleValueChanged(bool isOn) {
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

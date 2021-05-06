using System.Text.RegularExpressions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UniverseSettingsCustom : MonoBehaviour {
    private Slider slider;
    private Button button;
    private Text text;
    private Universe universe;
    private double lastTimeUpdate;
    private static DateTimeOffset simDateTime;
    private const double MAX_LOSSY_SCALE = 100000;
    private bool resetTime;

    void Start() {
        slider = GetComponent<Slider>();
        button = GetComponent<Button>();
        text = GetComponent<Text>();
        universe = FindObjectOfType<Universe>();
        simDateTime = new DateTimeOffset(new DateTime(2021, 4, 1, 0, 0, 0));
    }

    void Update() {
        if (text) {
            simDateTime = simDateTime.AddSeconds(universe.simulatedSeconds);
            universe.simulatedSeconds = 0;
            string[] dateTime = simDateTime.ToString("s").Split('T');
            text.text = $"Simulated\nDatetime:\n\n{dateTime[0]}\n{dateTime[1]}";
        }
    }

    public void ResetUniverse() {
        universe.Init();
        universe.activeSettings.WriteSettings();
        universe.simulatedSeconds = 0;
        simDateTime = new DateTimeOffset(new DateTime(2021, 4, 1, 0, 0, 0));
        FindObjectOfType<CosmicCamera>().queueTrailReset = true;
    }

    public void SetRunSpeedFactor() {
        if (slider) {
            double newLossyFactor = slider.value < MAX_LOSSY_SCALE ? slider.value : MAX_LOSSY_SCALE;
            double speedRatio = slider.value / universe.runSpeedFactor;
            universe.runSpeedFactor = slider.value;
            foreach (GravityObject obj in universe.gravityObjects) {
                obj.velocity *= speedRatio;
            }

            double timeFactor = newLossyFactor / slider.value;
            universe.timeStep = 0.001f * (float)timeFactor;
            Time.fixedDeltaTime = universe.timeStep;

            if (timeFactor < 0.25f) {
                slider.fillRect.GetComponentInChildren<Image>().color = Color.red;
            } else {
                slider.fillRect.GetComponentInChildren<Image>().color = Color.white;
            }

            double unitBase = 86400;
            string unitName = "Day";
            if (slider.value < 60) {
                unitBase = 1;
                unitName = "Sec";
            } else if (slider.value < 3600) {
                unitBase = 60;
                unitName = "Min";
            } else if (slider.value < 86400) {
                unitBase = 3600;
                unitName = "Hour";
            }
            Text label = transform.parent.GetComponentInChildren<Text>();
            string displayStr = $"[{(slider.value / unitBase):F1} {unitName}/sec]";
            label.text = Regex.Replace(label.text, @"\[.*\]", displayStr);
        }
    }
}

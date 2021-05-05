using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(UniverseSettings))]
[CanEditMultipleObjects]
public class UniverseSettingsEditor : Editor {
    SerializedProperty universe, sizeScales, distanceScales, referenceBody;

    void OnEnable() {
        sizeScales = serializedObject.FindProperty("sizeScales");
        distanceScales = serializedObject.FindProperty("distanceScales");
        universe = serializedObject.FindProperty("universe");
        referenceBody = serializedObject.FindProperty("referenceBody");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(universe);
        EditorGUILayout.PropertyField(referenceBody);
        EditorGUILayout.PropertyField(sizeScales);
        EditorGUILayout.PropertyField(distanceScales);


        Rect r = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r, GUIContent.none)) {
            var value = (UniverseSettings)target;
            value.SaveSettings();
        }
        GUILayout.Label("Save Settings");
        EditorGUILayout.EndHorizontal();

        r = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r, GUIContent.none)) {
            var value = (UniverseSettings)target;
            value.WriteSettings();
        }
        GUILayout.Label("Write Settings");
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}

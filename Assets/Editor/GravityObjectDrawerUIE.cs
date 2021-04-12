using UnityEngine;
using UnityEditor;
using System;


// IngredientDrawerUIE
[CustomEditor(typeof(GravityObject))]
[CanEditMultipleObjects]
public class GravityObjectEditor : Editor {
    SerializedProperty mass, radius, initPos, initVel, useCustSize, useCustDist, sizeScale, distScale, siderealPeriod, obliquity, color; 

    void OnEnable() {
        mass = serializedObject.FindProperty("mass");
        radius = serializedObject.FindProperty("radius");
        initPos = serializedObject.FindProperty("initPos");
        initVel = serializedObject.FindProperty("initVel");
        useCustSize = serializedObject.FindProperty("useCustomSizeScale");
        useCustDist = serializedObject.FindProperty("useCustomDistanceScale");
        sizeScale = serializedObject.FindProperty("sizeScale");
        distScale = serializedObject.FindProperty("distanceScale");
        siderealPeriod = serializedObject.FindProperty("siderealPeriod");
        obliquity = serializedObject.FindProperty("obliquity");
        color = serializedObject.FindProperty("color");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(mass, new GUIContent("Mass x10^23 (kg)"));
        EditorGUILayout.PropertyField(radius, new GUIContent("Radius (km)"));
        EditorGUILayout.PropertyField(siderealPeriod, new GUIContent("Sid. Rot. Period (day)"));
        EditorGUILayout.PropertyField(obliquity, new GUIContent("Obliquity to orbit (deg)"));
        EditorGUILayout.PropertyField(initPos, new GUIContent("Initial Pos (au)"));
        EditorGUILayout.PropertyField(initVel, new GUIContent("Inital Vel (au/day)"));

        useCustSize.boolValue = EditorGUILayout.BeginToggleGroup("Custom Size Scale", useCustSize.boolValue);
        EditorGUILayout.PropertyField(sizeScale);
        EditorGUILayout.EndToggleGroup();

        useCustDist.boolValue = EditorGUILayout.BeginToggleGroup("Custom Distance Scale", useCustDist.boolValue);
        EditorGUILayout.PropertyField(distScale);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.PropertyField(initVel, new GUIContent("Trail Color"));

        Rect r = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r, GUIContent.none)) {
            var value = (GravityObject)target;
            SceneView.lastActiveSceneView.pivot = Mathd.GetFloatVector3(value.GameWorldPos);
            SceneView.lastActiveSceneView.Repaint();
        }
        GUILayout.Label("Teleport to object");

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}

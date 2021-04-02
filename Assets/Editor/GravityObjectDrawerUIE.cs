using UnityEngine;
using UnityEditor;


// IngredientDrawerUIE
[CustomEditor(typeof(GravityObject))]
[CanEditMultipleObjects]
public class GravityObjectEditor : Editor {
    SerializedProperty mass, radius, initPos, initVel, useCustSize, useCustDist, sizeScale, distScale;
    
    void OnEnable() {
        mass = serializedObject.FindProperty("mass");
        radius = serializedObject.FindProperty("radius");
        initPos = serializedObject.FindProperty("initPos");
        initVel = serializedObject.FindProperty("initVel");
        useCustSize = serializedObject.FindProperty("useCustomSizeScale");
        useCustDist = serializedObject.FindProperty("useCustomDistanceScale");
        sizeScale = serializedObject.FindProperty("sizeScale");
        distScale = serializedObject.FindProperty("distanceScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(mass, new GUIContent("Mass x10^23 (kg)"));
        EditorGUILayout.PropertyField(radius, new GUIContent("Radius (km)"));
        EditorGUILayout.PropertyField(initPos, new GUIContent("Initial Pos (au)"));
        EditorGUILayout.PropertyField(initVel, new GUIContent("Inital Vel (au/day)"));

        useCustSize.boolValue = EditorGUILayout.BeginToggleGroup("Custom Size Scale", useCustSize.boolValue);
        EditorGUILayout.PropertyField(sizeScale);
        EditorGUILayout.EndToggleGroup();

        useCustDist.boolValue = EditorGUILayout.BeginToggleGroup("Custom Distance Scale", useCustDist.boolValue);
        EditorGUILayout.PropertyField(distScale);
        EditorGUILayout.EndToggleGroup();

        Rect r = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r, GUIContent.none)) {
            var value = (GravityObject)target;
            SceneView.lastActiveSceneView.pivot = value.GameWorldPos;
            SceneView.lastActiveSceneView.Repaint();
        }
        GUILayout.Label("Teleport to object");

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}

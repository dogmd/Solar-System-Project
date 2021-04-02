using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

[CustomPropertyDrawer(typeof(Vector3d))]
public class Vector3dUIE : PropertyDrawer {
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var xField = new PropertyField(property.FindPropertyRelative("x"));
        var yField = new PropertyField(property.FindPropertyRelative("y"));
        var zField = new PropertyField(property.FindPropertyRelative("z"));

        // Add fields to the container.
        container.Add(xField);
        container.Add(yField);
        container.Add(zField);

        return container;
    }

    // yoinked from Vector3 property drawer
    private const float SubLabelSpacing = 4;
    private const float BottomSpacing = 2;
 
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label) {
        pos.height -= BottomSpacing;
        label = EditorGUI.BeginProperty(pos, label, prop);
        var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
        var labels      = new[] {new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z")};
        var properties  = new[] {prop.FindPropertyRelative("x"), prop.FindPropertyRelative("y"), prop.FindPropertyRelative("z")};
        DrawMultiplePropertyFields(contentRect, labels, properties);
 
        EditorGUI.EndProperty();
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) + BottomSpacing;
    }
 
    public static void DrawMultiplePropertyFields(Rect pos, GUIContent[] subLabels, SerializedProperty[] props) {
        // backup gui settings
        var indent     = EditorGUI.indentLevel;
        var labelWidth = EditorGUIUtility.labelWidth;
     
        // draw properties
        var propsCount = props.Length;
        var width      = (pos.width - (propsCount - 1) * SubLabelSpacing) / propsCount;
        var contentPos = new Rect(pos.x, pos.y, width, pos.height);
        EditorGUI.indentLevel = 0;
        for (var i = 0; i < propsCount; i++) {
            EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x;
            EditorGUI.PropertyField(contentPos, props[i], subLabels[i]);
            contentPos.x += width + SubLabelSpacing;
        }
 
        // restore gui settings
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.indentLevel       = indent;
    }
}

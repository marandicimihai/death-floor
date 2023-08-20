using DeathFloor.Utilities;
using UnityEditor;
using UnityEngine;

namespace DeathFloor.Editor
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enabled = property.FindPropertyRelative("_enabled");
            var value = property.FindPropertyRelative("_value");

            position.width -= 24;
            EditorGUI.BeginDisabledGroup(!enabled.boolValue);
            EditorGUI.PropertyField(position, value, label, true);
            EditorGUI.EndDisabledGroup();

            position.x += position.width + 24;
            position.width = position.height = EditorGUI.GetPropertyHeight(enabled);
            position.x -= position.width;
            EditorGUI.PropertyField(position, enabled, GUIContent.none);
        }
    }
}
using UnityEngine;
using UnityEditor;
using DeathFloor.Utilities;

[CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
public class RequireInterfaceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            var requiredAttribute = this.attribute as RequireInterfaceAttribute;
            
            EditorGUI.BeginProperty(position, label, property);

            property.objectReferenceValue = EditorGUI.ObjectField(
                                                position, 
                                                label, 
                                                property.objectReferenceValue, 
                                                requiredAttribute.InterfaceType, 
                                                true);
            
            EditorGUI.EndProperty();
        }
    }
}
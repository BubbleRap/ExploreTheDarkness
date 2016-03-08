using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof (Indexer))]
public class DialogIndexPropertyDrawer : PropertyDrawer {

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
    {
        EditorGUI.BeginProperty (position, label, property);

        position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var amountRect = new Rect (position.x, position.y, 30, position.height);

        GUI.SetNextControlName(label.text);

        SerializedProperty propertyItem = property.FindPropertyRelative ("index");
        EditorGUI.PropertyField (amountRect, propertyItem, GUIContent.none);

        if( KeyPressed(label.text, KeyCode.Return))
        {
            DialogChoices dialogSystem = property.serializedObject.targetObject as DialogChoices;
            dialogSystem.SortDialogsByIndex();
        }

        EditorGUI.indentLevel = indent;


        EditorGUI.EndProperty ();
    }

    private bool KeyPressed(string controlName, KeyCode key)
    {
        if(GUI.GetNameOfFocusedControl() == controlName)
        {
            if ((Event.current.type == EventType.KeyUp) && 
                (Event.current.keyCode == key))

                return true;

            return false;
        }
        else
        {
            return false;
        }
    }
}

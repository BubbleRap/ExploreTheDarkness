using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomPropertyDrawer (typeof(MinMaxSliderAttribute))]
class RangeDrawer : PropertyDrawer 
{
	void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		EditorGUI.BeginProperty (position, label, property);

		MinMaxSliderAttribute minMaxSlider = attribute as MinMaxSliderAttribute;
		EditorGUI.MinMaxSlider(label, position, ref minMaxSlider.minValue, ref minMaxSlider.maxValue, minMaxSlider.minLimit, minMaxSlider.maxLimit);

		EditorGUI.EndProperty ();
	}

//	void DrawDefaultProperty(Rect position, SerializedProperty property, GUIContent label,bool includeChildren = true)
//	{
//		Dictionary<string, PropertyDrawer> s_dictionary =		
//			typeof(PropertyDrawer).GetField("s_PropertyDrawers", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(null) as Dictionary<string, PropertyDrawer>;
//
//		foreach (var entry in s_dictionary)			
//		{
//			if (entry.Value == this)	
//			{	
//				s_dictionary[entry.Key] = null;
//				EditorGUI.PropertyField(position, property, label, true);
//
//				s_dictionary[entry.Key] = this;
//				return;	
//			}	
//		}
//		EditorGUI.PropertyField(position, property, label, true);
//	}

}

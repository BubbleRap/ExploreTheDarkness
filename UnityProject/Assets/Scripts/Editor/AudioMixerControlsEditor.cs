using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AudioMixerControls))]
public class AudioMixerControlsEditor : Editor {

	private SerializedProperty m_audioSources;

	public void OnEnable()
	{
		m_audioSources = serializedObject.FindProperty("m_audioSources");
	}

    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();
		return;

		//serializedObject.UpdateIfDirtyOrScript();
		//
        //AudioMixerControls myTarget = target as AudioMixerControls;
		//
        //myTarget.m_mixingType = (AudioMixerControls.MixingType) EditorGUILayout.EnumPopup("Mixing type",  myTarget.m_mixingType);
		//
		//switch(myTarget.m_mixingType)
		//{
		//case SwitchType.FadeInOutParameter:
        //
        //    myTarget.m_minValue = EditorGUILayout.FloatField("Min Volume", myTarget.m_minValue);
        //    myTarget.m_maxValue = EditorGUILayout.FloatField("Max Volume", myTarget.m_maxValue);
        //    myTarget.m_paramName = EditorGUILayout.TextField("Parameter Name", myTarget.m_paramName);
		//
        //    myTarget.m_audioMixer = (AudioMixer) EditorGUILayout.ObjectField("Audio Mixer", myTarget.m_audioMixer, typeof(AudioMixer), false);
        //    myTarget.m_audioSource = (AudioSource) EditorGUILayout.ObjectField("Audio Source", myTarget.m_audioSource, typeof(AudioSource), true);
		//	myTarget.m_time = EditorGUILayout.FloatField ("Time", myTarget.m_time);
		//	break;
		//
		//case SwitchType.FadeToSnapshot:
       	//	
        //    myTarget.m_snapshotName = EditorGUILayout.TextField("Snapshot Name", myTarget.m_snapshotName);
        //    myTarget.m_audioMixer = (AudioMixer) EditorGUILayout.ObjectField("Audio Mixer", myTarget.m_audioMixer, typeof(AudioMixer), false);
		//	myTarget.m_time = EditorGUILayout.FloatField ("Time", myTarget.m_time);
		//	break;
		//
		//case SwitchType.PlayOneShot:
        //
        //    myTarget.m_audioSource = (AudioSource) EditorGUILayout.ObjectField("Audio Source", myTarget.m_audioSource, typeof(AudioSource), true);
		//		break;
		//
		//case SwitchType.ContextBasedListRandom:
		//case SwitchType.ContextBasedListOrdered:
		//
		//	EditorGUILayout.PropertyField(m_audioSources);
		//
		//
		//	break;
		//}
		//	
		//serializedObject.ApplyModifiedProperties();
    }
}

using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using System.Collections;

[CustomEditor(typeof(AudioMixerControls))]
public class AudioMixerControlsEditor : Editor {

    public override void OnInspectorGUI()
    {
        AudioMixerControls myTarget = target as AudioMixerControls;

        myTarget.m_mixingType = (AudioMixerControls.MixingType) EditorGUILayout.EnumPopup("Mixing type",  myTarget.m_mixingType);

        if(myTarget.m_mixingType == AudioMixerControls.MixingType.FadeInOutParameter)
        {
            myTarget.m_minValue = EditorGUILayout.FloatField("Min Volume", myTarget.m_minValue);
            myTarget.m_maxValue = EditorGUILayout.FloatField("Max Volume", myTarget.m_maxValue);
            myTarget.m_paramName = EditorGUILayout.TextField("Parameter Name", myTarget.m_paramName);

            myTarget.m_audioMixer = (AudioMixer) EditorGUILayout.ObjectField("Audio Mixer", myTarget.m_audioMixer, typeof(AudioMixer), false);
            myTarget.m_audioSource = (AudioSource) EditorGUILayout.ObjectField("Audio Source", myTarget.m_audioSource, typeof(AudioSource), true);
        }

        if( myTarget.m_mixingType == AudioMixerControls.MixingType.FadeToSnapshot )
        {
            myTarget.m_snapshotName = EditorGUILayout.TextField("Snapshot Name", myTarget.m_snapshotName);
            myTarget.m_audioMixer = (AudioMixer) EditorGUILayout.ObjectField("Audio Mixer", myTarget.m_audioMixer, typeof(AudioMixer), false);
        }

        if( myTarget.m_mixingType == AudioMixerControls.MixingType.PlayOneShot )
        {
            myTarget.m_audioSource = (AudioSource) EditorGUILayout.ObjectField("Audio Source", myTarget.m_audioSource, typeof(AudioSource), true);
        }
    }
}

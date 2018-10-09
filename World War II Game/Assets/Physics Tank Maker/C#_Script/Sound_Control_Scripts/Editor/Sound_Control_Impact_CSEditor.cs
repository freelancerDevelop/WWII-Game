using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Sound_Control_Impact_CS))]
	public class Sound_Control_Impact_CSEditor : Editor
	{

		SerializedProperty Min_ImpactProp;
		SerializedProperty Max_ImpactProp;
		SerializedProperty Min_Impact_PitchProp;
		SerializedProperty Max_Impact_PitchProp;
		SerializedProperty Min_Impact_VolumeProp;
		SerializedProperty Max_Impact_VolumeProp;


		void  OnEnable ()
		{
			Min_ImpactProp = serializedObject.FindProperty ("Min_Impact");
			Max_ImpactProp = serializedObject.FindProperty ("Max_Impact");
			Min_Impact_PitchProp = serializedObject.FindProperty ("Min_Impact_Pitch");
			Max_Impact_PitchProp = serializedObject.FindProperty ("Max_Impact_Pitch");
			Min_Impact_VolumeProp = serializedObject.FindProperty ("Min_Impact_Volume");
			Max_Impact_VolumeProp = serializedObject.FindProperty ("Max_Impact_Volume");
		}


		public override void  OnInspectorGUI ()
		{
			serializedObject.Update ();

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
		
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Impact sound settings.", MessageType.None, true);

			EditorGUILayout.Slider (Min_ImpactProp, 0.1f, 10.0f, "Min Impact");
			EditorGUILayout.Slider (Max_ImpactProp, 0.1f, 10.0f, "Max Impact");
			EditorGUILayout.Slider (Min_Impact_PitchProp, 0.1f, 10.0f, "Min Impact Pitch");
			EditorGUILayout.Slider (Max_Impact_PitchProp, 0.1f, 10.0f, "Max Impact Pitch");
			EditorGUILayout.Slider (Min_Impact_VolumeProp, 0.1f, 10.0f, "Min Impact Volume");
			EditorGUILayout.Slider (Max_Impact_VolumeProp, 0.1f, 10.0f, "Max Impact Volume");

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		
			serializedObject.ApplyModifiedProperties ();
		}

	}

}
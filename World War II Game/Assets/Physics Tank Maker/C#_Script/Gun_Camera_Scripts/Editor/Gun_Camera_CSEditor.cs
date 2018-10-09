using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Gun_Camera_CS))]
	public class Gun_Camera_CSEditor : Editor
	{

		SerializedProperty Gun_CameraProp;
		SerializedProperty Main_CameraProp;
		SerializedProperty Min_FOVProp;
		SerializedProperty Max_FOVProp;
	

		void OnEnable ()
		{
			Gun_CameraProp = serializedObject.FindProperty ("Gun_Camera");
			Main_CameraProp = serializedObject.FindProperty ("Main_Camera");
			Min_FOVProp = serializedObject.FindProperty ("Min_FOV");
			Max_FOVProp = serializedObject.FindProperty ("Max_FOV");
		}


		public override void OnInspectorGUI ()
		{
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Gun Camera settings", MessageType.None, true);
			Gun_CameraProp.objectReferenceValue = EditorGUILayout.ObjectField ("Gun Camera", Gun_CameraProp.objectReferenceValue, typeof(Camera), true);
			Main_CameraProp.objectReferenceValue = EditorGUILayout.ObjectField ("Main Camera", Main_CameraProp.objectReferenceValue, typeof(Camera), true);
			EditorGUILayout.Slider (Min_FOVProp, 1.0f, 100.0f, "Min FOV");
			EditorGUILayout.Slider (Max_FOVProp, 1.0f, 100.0f, "Max FOV");


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
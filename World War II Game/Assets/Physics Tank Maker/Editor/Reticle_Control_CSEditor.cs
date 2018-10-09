using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Reticle_Control_CS))]
	public class Reticle_Control_CSEditor : Editor
	{

		SerializedProperty Reticle_NameProp;
		SerializedProperty Gun_CameraProp;
	

		void OnEnable ()
		{
			Reticle_NameProp = serializedObject.FindProperty ("Reticle_Name");
			Gun_CameraProp = serializedObject.FindProperty ("Gun_Camera");
		}


		public override void OnInspectorGUI ()
		{
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Reticle settings", MessageType.None, true);
			Reticle_NameProp.stringValue = EditorGUILayout.TextField ("Reticle Name", Reticle_NameProp.stringValue);
			Gun_CameraProp.objectReferenceValue = EditorGUILayout.ObjectField ("Gun Camera", Gun_CameraProp.objectReferenceValue, typeof(Camera), true);


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
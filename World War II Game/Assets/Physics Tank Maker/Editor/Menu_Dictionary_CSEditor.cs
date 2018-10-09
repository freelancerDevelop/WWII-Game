using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Menu_Dictionary_CS))]
	public class Menu_Dictionary_CSEditor : Editor
	{
		
		SerializedProperty Battle_Scene_NameProp;
	

		void OnEnable ()
		{
			Battle_Scene_NameProp = serializedObject.FindProperty ("Battle_Scene_Name");
		}


		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Dictionary settings", MessageType.None, true);
			Battle_Scene_NameProp.stringValue = EditorGUILayout.TextField ("Battle Scene Name", Battle_Scene_NameProp.stringValue);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
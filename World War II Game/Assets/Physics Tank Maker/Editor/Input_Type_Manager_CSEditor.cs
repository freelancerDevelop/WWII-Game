using System.Collections;
using UnityEngine;
using UnityEditor;

namespace ChobiAssets.PTM
{
	
	[ CustomEditor (typeof(Input_Type_Manager_CS))]
	public class Input_Type_Manager_CSEditor : Editor
	{

		SerializedProperty Input_TypeProp;
		SerializedProperty Use_Auto_LeadProp;
		SerializedProperty Show_Cursor_ForciblyProp;

		string[] inputTypeNames = { "Mouse + Keyboard (Stepwise)", "Mouse + Keyboard (Pressing)", "Mouse + Keyboard (Legacy)", "GamePad (Single stick)", "GamePad (Twin stick)", "GamePad (Triggers)"};
	

		void  OnEnable ()
		{
			Input_TypeProp = serializedObject.FindProperty("Input_Type");
			Use_Auto_LeadProp = serializedObject.FindProperty("Use_Auto_Lead");
			Show_Cursor_ForciblyProp = serializedObject.FindProperty("Show_Cursor_Forcibly");
		}


		public override void OnInspectorGUI ()
		{
			if (EditorApplication.isPlaying) {
				return;
			}

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();

			// Input settings.
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Input settings", MessageType.None, true);

			Input_TypeProp.intValue = EditorGUILayout.Popup ("Input Type", Input_TypeProp.intValue, inputTypeNames);
			Use_Auto_LeadProp.boolValue = EditorGUILayout.Toggle ("Use Auto Lead", Use_Auto_LeadProp.boolValue);
			Show_Cursor_ForciblyProp.boolValue = EditorGUILayout.Toggle ("Show Cursor Forcibly", Show_Cursor_ForciblyProp.boolValue);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}

	}
}

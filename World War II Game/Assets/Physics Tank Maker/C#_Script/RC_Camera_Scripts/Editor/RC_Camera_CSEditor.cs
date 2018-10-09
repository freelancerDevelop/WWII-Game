using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{
	
	[ CustomEditor (typeof(RC_Camera_CS))]
	public class RC_Camera_CSEditor : Editor
	{
		
		SerializedProperty Horizontal_SpeedProp;
		SerializedProperty Vertical_SpeedProp;
		SerializedProperty Min_FOVProp;
		SerializedProperty Max_FOVProp;
		SerializedProperty Position_PackProp;


		void OnEnable ()
		{
			Horizontal_SpeedProp = serializedObject.FindProperty ("Horizontal_Speed");
			Vertical_SpeedProp = serializedObject.FindProperty ("Vertical_Speed");
			Min_FOVProp = serializedObject.FindProperty ("Min_FOV");
			Max_FOVProp = serializedObject.FindProperty ("Max_FOV");
			Position_PackProp = serializedObject.FindProperty ("Position_Pack");
		}


		public override void OnInspectorGUI ()
		{
			if (EditorApplication.isPlaying) {
				return;
			}

			serializedObject.Update ();
		
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
		
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();	
			EditorGUILayout.HelpBox ("RC Camera settings.", MessageType.None, true);
			EditorGUILayout.Slider (Horizontal_SpeedProp, 0.1f, 10.0f, "Horizontal Speed");
			EditorGUILayout.Slider (Vertical_SpeedProp, 0.1f, 10.0f, "Vertical Speed");
			EditorGUILayout.Slider (Min_FOVProp, 1.0f, 100.0f, "Minimum FOV");
			EditorGUILayout.Slider (Max_FOVProp, 1.0f, 100.0f, "Maximum FOV");
			Position_PackProp.objectReferenceValue = EditorGUILayout.ObjectField ("Camera Position Pack", Position_PackProp.objectReferenceValue, typeof(GameObject), true);
		
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		
			serializedObject.ApplyModifiedProperties ();
		}

	}

}
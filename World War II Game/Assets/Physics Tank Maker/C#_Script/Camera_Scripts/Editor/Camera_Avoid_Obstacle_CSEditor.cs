using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Camera_Avoid_Obstacle_CS))]
	public class Camera_Avoid_Obstacle_CSEditor : Editor
	{
		
		SerializedProperty Main_CameraProp;
		SerializedProperty Move_SpeedProp;
		SerializedProperty Min_DistProp;
		SerializedProperty Max_DistProp;


		void OnEnable ()
		{
			Main_CameraProp = serializedObject.FindProperty ("Main_Camera");
			Move_SpeedProp = serializedObject.FindProperty ("Move_Speed");
			Min_DistProp = serializedObject.FindProperty ("Min_Dist");
			Max_DistProp = serializedObject.FindProperty ("Max_Dist");
		}


		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Camera Pop Up settings", MessageType.None, true);
			Main_CameraProp.objectReferenceValue = EditorGUILayout.ObjectField ("Main Camera", Main_CameraProp.objectReferenceValue, typeof(Camera), true);
			EditorGUILayout.Slider (Move_SpeedProp, 1.0f, 100.0f, "Move Speed");
			EditorGUILayout.Slider (Min_DistProp, 1.0f, 100.0f, "Min Dist");
			EditorGUILayout.Slider (Max_DistProp, 1.0f, 100.0f, "Max Dist");


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
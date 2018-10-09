using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Aiming_Control_CS))]
	public class Aiming_Control_CSEditor : Editor
	{
	
		SerializedProperty Aiming_SensibilityProp;
		SerializedProperty OpenFire_AngleProp;
		SerializedProperty Use_Auto_Elevation_AngleProp;


		void OnEnable ()
		{
			Aiming_SensibilityProp = serializedObject.FindProperty ("Aiming_Sensibility");
			OpenFire_AngleProp = serializedObject.FindProperty ("OpenFire_Angle");
			Use_Auto_Elevation_AngleProp = serializedObject.FindProperty ("Use_Auto_Elevation_Angle");
		}


		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Aiming settings", MessageType.None, true);
			EditorGUILayout.Slider (Aiming_SensibilityProp, 0.1f, 1.0f, "Aiming Sensibility");
			EditorGUILayout.Slider (OpenFire_AngleProp, 1.0f, 360.0f, "Open Fire Angle");
			Use_Auto_Elevation_AngleProp.boolValue = EditorGUILayout.Toggle ("Use Auto Elevation Angle", Use_Auto_Elevation_AngleProp.boolValue);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Game_Controller_CS))]
	public class Game_Controller_CSEditor : Editor
	{

		SerializedProperty Fixed_TimeStepProp;
		SerializedProperty Sleep_ThresholdProp;


		void OnEnable ()
		{
			Fixed_TimeStepProp = serializedObject.FindProperty ("Fixed_TimeStep");
			Sleep_ThresholdProp = serializedObject.FindProperty ("Sleep_Threshold");
		}


		public override void OnInspectorGUI ()
		{
			if (EditorApplication.isPlaying) {
				return;
			}

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();

			// Physics settings.
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Physics settings", MessageType.None, true);
			EditorGUILayout.Slider (Fixed_TimeStepProp, 0.005f, 0.05f, "Fixed TimeStep");
			EditorGUILayout.Slider (Sleep_ThresholdProp, 0.00f, 10.0f, "Sleep Threshold");

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		
			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
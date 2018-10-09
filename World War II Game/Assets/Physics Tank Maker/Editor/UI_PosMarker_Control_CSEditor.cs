using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(UI_PosMarker_Control_CS))]
	public class UI_PosMarker_Control_CSEditor : Editor
	{
		
		SerializedProperty Friend_ColorProp;
		SerializedProperty Hostile_ColorProp;
		SerializedProperty Landmark_ColorProp;
		SerializedProperty Defensive_AlphaProp;
		SerializedProperty Offensive_AlphaProp;
		SerializedProperty Upper_OffsetProp;
		SerializedProperty Side_OffsetProp;
		SerializedProperty Show_AllProp;


		void OnEnable ()
		{
			Friend_ColorProp = serializedObject.FindProperty ("Friend_Color");
			Hostile_ColorProp = serializedObject.FindProperty ("Hostile_Color");
			Landmark_ColorProp = serializedObject.FindProperty ("Landmark_Color");
			Defensive_AlphaProp = serializedObject.FindProperty ("Defensive_Alpha");
			Offensive_AlphaProp = serializedObject.FindProperty ("Offensive_Alpha");
			Upper_OffsetProp = serializedObject.FindProperty ("Upper_Offset");
			Side_OffsetProp = serializedObject.FindProperty ("Side_Offset");
			Show_AllProp = serializedObject.FindProperty ("Show_All");
		}


		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Position Marker settings", MessageType.None, true);
			Friend_ColorProp.colorValue = EditorGUILayout.ColorField("Friend Color", Friend_ColorProp.colorValue);
			Hostile_ColorProp.colorValue = EditorGUILayout.ColorField("Hostile Color", Hostile_ColorProp.colorValue);
			Landmark_ColorProp.colorValue = EditorGUILayout.ColorField("Landmark Color", Landmark_ColorProp.colorValue);
			EditorGUILayout.Slider (Defensive_AlphaProp, 0.01f, 1.0f, "Defensive Alpha");
			EditorGUILayout.Slider (Offensive_AlphaProp, 0.01f, 1.0f, "Offensive Alpha");
			EditorGUILayout.Slider (Upper_OffsetProp, -128.0f, 128.0f, "Upper Offset");
			EditorGUILayout.Slider (Side_OffsetProp, -128.0f, 128.0f, "Side Offset");
			Show_AllProp.boolValue = EditorGUILayout.Toggle ("Show All", Show_AllProp.boolValue);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}
	
	}

}
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Static_Track_CS))]
	public class Static_Track_CSEditor : Editor
	{
	
		public override void OnInspectorGUI ()
		{
			GUI.backgroundColor = new Color (1.0f, 0.5f, 0.5f, 1.0f);
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("'Static_Track_CS' script is obsolete.\nPlease update this track by pressing 'Update Track Pieces' button below.", MessageType.Error, true);
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();


			if (Selection.activeGameObject) {
				Static_Track_Parent_CS parentScript = Selection.activeGameObject.GetComponent <Static_Track_Parent_CS>();
				if (parentScript == null) {
					Selection.activeGameObject.AddComponent <Static_Track_Parent_CS>();
				}
			}
		}

	}

}
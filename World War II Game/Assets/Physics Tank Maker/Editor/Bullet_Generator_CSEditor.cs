using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Bullet_Generator_CS))]
	public class Bullet_Generator_CSEditor : Editor
	{

		SerializedProperty AP_Bullet_PrefabProp;
		SerializedProperty HE_Bullet_PrefabProp;
		SerializedProperty MuzzleFire_ObjectProp;
		SerializedProperty Attack_PointProp;
		SerializedProperty Attack_Point_HEProp;
		SerializedProperty Initial_VelocityProp;
		SerializedProperty Initial_Velocity_HEProp;

		SerializedProperty Life_TimeProp;
		SerializedProperty Initial_Bullet_TypeProp;
		SerializedProperty OffsetProp;
		SerializedProperty Debug_FlagProp;

		SerializedProperty Bullet_MeshProp;
		SerializedProperty Bullet_MaterialProp;
		SerializedProperty Bullet_ScaleProp;
		SerializedProperty Bullet_MassProp;
		SerializedProperty Bullet_DragProp;
		SerializedProperty Bullet_PhysicMatProp;
		SerializedProperty Impact_ObjectProp;
		SerializedProperty Ricochet_ObjectProp;
		SerializedProperty Trail_FlagProp;
		SerializedProperty Trail_MaterialProp;
		SerializedProperty Trail_Start_WidthProp;
		SerializedProperty Trail_End_WidthProp;
		SerializedProperty Trail_TimeProp;

		SerializedProperty Bullet_Mesh_HEProp;
		SerializedProperty Bullet_Material_HEProp;
		SerializedProperty Bullet_Scale_HEProp;
		SerializedProperty Bullet_Mass_HEProp;
		SerializedProperty Bullet_Drag_HEProp;
		SerializedProperty Explosion_ObjectProp;
		SerializedProperty Explosion_ForceProp;
		SerializedProperty Explosion_RadiusProp;
		SerializedProperty Trail_Flag_HEProp;
		SerializedProperty Trail_Material_HEProp;
		SerializedProperty Trail_Start_Width_HEProp;
		SerializedProperty Trail_End_Width_HEProp;
		SerializedProperty Trail_Time_HEProp;

		string[] typeNames = { "AP", "HE" };
		bool folding;


		void  OnEnable ()
		{
			AP_Bullet_PrefabProp = serializedObject.FindProperty ("AP_Bullet_Prefab");
			HE_Bullet_PrefabProp = serializedObject.FindProperty ("HE_Bullet_Prefab");
			MuzzleFire_ObjectProp = serializedObject.FindProperty ("MuzzleFire_Object");
			Attack_PointProp = serializedObject.FindProperty ("Attack_Point");
			Attack_Point_HEProp = serializedObject.FindProperty ("Attack_Point_HE");
			Initial_VelocityProp = serializedObject.FindProperty ("Initial_Velocity");
			Initial_Velocity_HEProp = serializedObject.FindProperty ("Initial_Velocity_HE");

			Life_TimeProp = serializedObject.FindProperty ("Life_Time");
			Initial_Bullet_TypeProp = serializedObject.FindProperty ("Initial_Bullet_Type");
			OffsetProp = serializedObject.FindProperty ("Offset");
			Debug_FlagProp = serializedObject.FindProperty ("Debug_Flag");

			Bullet_MeshProp = serializedObject.FindProperty ("Bullet_Mesh");
			Bullet_MaterialProp = serializedObject.FindProperty ("Bullet_Material");
			Bullet_ScaleProp = serializedObject.FindProperty ("Bullet_Scale");
			Bullet_MassProp = serializedObject.FindProperty ("Bullet_Mass");
			Bullet_DragProp = serializedObject.FindProperty ("Bullet_Drag");
			Bullet_PhysicMatProp = serializedObject.FindProperty ("Bullet_PhysicMat");
			Impact_ObjectProp = serializedObject.FindProperty ("Impact_Object");
			Ricochet_ObjectProp = serializedObject.FindProperty ("Ricochet_Object");
			Trail_FlagProp = serializedObject.FindProperty ("Trail_Flag");
			Trail_MaterialProp = serializedObject.FindProperty ("Trail_Material");
			Trail_Start_WidthProp = serializedObject.FindProperty ("Trail_Start_Width");
			Trail_End_WidthProp = serializedObject.FindProperty ("Trail_End_Width");
			Trail_TimeProp = serializedObject.FindProperty ("Trail_Time");

			Bullet_Mesh_HEProp = serializedObject.FindProperty ("Bullet_Mesh_HE");
			Bullet_Material_HEProp = serializedObject.FindProperty ("Bullet_Material_HE");
			Bullet_Scale_HEProp = serializedObject.FindProperty ("Bullet_Scale_HE");
			Bullet_Mass_HEProp = serializedObject.FindProperty ("Bullet_Mass_HE");
			Bullet_Drag_HEProp = serializedObject.FindProperty ("Bullet_Drag_HE");
			Explosion_ObjectProp = serializedObject.FindProperty ("Explosion_Object");
			Explosion_ForceProp = serializedObject.FindProperty ("Explosion_Force");
			Explosion_RadiusProp = serializedObject.FindProperty ("Explosion_Radius");
			Trail_Flag_HEProp = serializedObject.FindProperty ("Trail_Flag_HE");
			Trail_Material_HEProp = serializedObject.FindProperty ("Trail_Material_HE");
			Trail_Start_Width_HEProp = serializedObject.FindProperty ("Trail_Start_Width_HE");
			Trail_End_Width_HEProp = serializedObject.FindProperty ("Trail_End_Width_HE");
			Trail_Time_HEProp = serializedObject.FindProperty ("Trail_Time_HE");
		}


		public override void  OnInspectorGUI ()
		{
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();
		
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Bullet Prefab settings", MessageType.None, true);
			AP_Bullet_PrefabProp.objectReferenceValue = EditorGUILayout.ObjectField ("AP Bullet Prefab", AP_Bullet_PrefabProp.objectReferenceValue, typeof(GameObject), true);
			HE_Bullet_PrefabProp.objectReferenceValue = EditorGUILayout.ObjectField ("HE Bullet Prefab", HE_Bullet_PrefabProp.objectReferenceValue, typeof(GameObject), true);
			EditorGUILayout.Space ();
			MuzzleFire_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ("MuzzleFire Prefab", MuzzleFire_ObjectProp.objectReferenceValue, typeof(GameObject), true);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Basic settings", MessageType.None, true);
			EditorGUILayout.Slider (Attack_PointProp, 10.0f, 10000.0f, "AP Attack Point");
			EditorGUILayout.Slider (Attack_Point_HEProp, 10.0f, 10000.0f, "HE Attack Point");
			EditorGUILayout.Slider (Initial_VelocityProp, 10.0f, 1000.0f, "AP Initial Velocity");
			EditorGUILayout.Slider (Initial_Velocity_HEProp, 10.0f, 1000.0f, "HE Initial Velocity");
			EditorGUILayout.Space ();
			EditorGUILayout.Slider (Life_TimeProp, 1.0f, 180.0f, "Life Time");
			EditorGUILayout.Space ();
			Initial_Bullet_TypeProp.intValue = EditorGUILayout.Popup ("Initial Bullet Type", Initial_Bullet_TypeProp.intValue, typeNames);
			EditorGUILayout.Slider (OffsetProp, 0.0f, 10.0f, "Offset");
			Debug_FlagProp.boolValue = EditorGUILayout.Toggle ("Debug Mode", Debug_FlagProp.boolValue);


			if (folding = EditorGUILayout.Foldout (folding, "For Creating Bullet Prefab")) {
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("AP Bullet Prefab settings", MessageType.None, true);
				Bullet_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ("Mesh", Bullet_MeshProp.objectReferenceValue, typeof(Mesh), false);
				Bullet_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ("Material", Bullet_MaterialProp.objectReferenceValue, typeof(Material), false);
				Bullet_ScaleProp.vector3Value = EditorGUILayout.Vector3Field ("Scale", Bullet_ScaleProp.vector3Value);
				EditorGUILayout.Space ();
				EditorGUILayout.Slider (Bullet_MassProp, 0.1f, 100.0f, "Mass");
				EditorGUILayout.Slider (Bullet_DragProp, 0.0f, 1.0f, "Drag (Air Resistance)");
				Bullet_PhysicMatProp.objectReferenceValue = EditorGUILayout.ObjectField ("Physic Material", Bullet_PhysicMatProp.objectReferenceValue, typeof(PhysicMaterial), false);
				EditorGUILayout.Space ();
				Impact_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ("Impact Prefab", Impact_ObjectProp.objectReferenceValue, typeof(GameObject), true);
				Ricochet_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ("Ricochet Prefab", Ricochet_ObjectProp.objectReferenceValue, typeof(GameObject), true);
				EditorGUILayout.Space ();
				Trail_FlagProp.boolValue = EditorGUILayout.Toggle ("Trail", Trail_FlagProp.boolValue);
				if (Trail_FlagProp.boolValue) {
					Trail_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ("Trail Material", Trail_MaterialProp.objectReferenceValue, typeof(Material), false);
					EditorGUILayout.Slider (Trail_Start_WidthProp, 0.0f, 10.0f, "Start Width");
					EditorGUILayout.Slider (Trail_End_WidthProp, 0.0f, 10.0f, "End Width");
					EditorGUILayout.Slider (Trail_TimeProp, 0.0f, 10.0f, "Time");
				}
				EditorGUILayout.Space ();
				if (GUILayout.Button ("Create Prefab (AP)", GUILayout.Width (200))) {
					Create_Prefab_AP ();
				}
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
		
				EditorGUILayout.HelpBox ("HE Bullet Prefab settings", MessageType.None, true);
				Bullet_Mesh_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ("Mesh", Bullet_Mesh_HEProp.objectReferenceValue, typeof(Mesh), false);
				Bullet_Material_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ("Material", Bullet_Material_HEProp.objectReferenceValue, typeof(Material), false);
				Bullet_Scale_HEProp.vector3Value = EditorGUILayout.Vector3Field ("Scale", Bullet_Scale_HEProp.vector3Value);
				EditorGUILayout.Space ();
				EditorGUILayout.Slider (Bullet_Mass_HEProp, 0.1f, 100.0f, "Mass");
				EditorGUILayout.Slider (Bullet_Drag_HEProp, 0.0f, 1.0f, "Drag (Air Resistance)");
				EditorGUILayout.Space ();
				Explosion_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ("Explosion Prefab", Explosion_ObjectProp.objectReferenceValue, typeof(GameObject), true);
				EditorGUILayout.Slider (Explosion_ForceProp, 10.0f, 1000000.0f, "Explosion Force");
				EditorGUILayout.Slider (Explosion_RadiusProp, 0.1f, 1000.0f, "Explosion Radius");
				EditorGUILayout.Space ();
				Trail_Flag_HEProp.boolValue = EditorGUILayout.Toggle ("Trail", Trail_Flag_HEProp.boolValue);
				if (Trail_Flag_HEProp.boolValue) {
					Trail_Material_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ("Trail Material", Trail_Material_HEProp.objectReferenceValue, typeof(Material), false);
					EditorGUILayout.Slider (Trail_Start_Width_HEProp, 0.0f, 10.0f, "Start Width");
					EditorGUILayout.Slider (Trail_End_Width_HEProp, 0.0f, 10.0f, "End Width");
					EditorGUILayout.Slider (Trail_Time_HEProp, 0.0f, 10.0f, "Time");
				}
				EditorGUILayout.Space ();
				if (GUILayout.Button ("Create Prefab (HE)", GUILayout.Width (200))) {
					Create_Prefab_HE ();
				}
			}

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}

		void Create_Prefab_AP ()
		{
			if (Bullet_MeshProp.objectReferenceValue == null) {
				Debug.LogWarning("Set the 'Mesh' for the bullet prefab.");
				return;
			}

			// Create GameObject & Set Transform
			GameObject bulletObject = new GameObject ("Bullet");
			bulletObject.transform.localScale = Bullet_ScaleProp.vector3Value;
			// Add Components
			MeshRenderer meshRenderer = bulletObject.AddComponent < MeshRenderer > ();
			meshRenderer.material = Bullet_MaterialProp.objectReferenceValue as Material;
			MeshFilter meshFilter = bulletObject.AddComponent < MeshFilter > ();
			meshFilter.mesh = Bullet_MeshProp.objectReferenceValue as Mesh;
			Rigidbody rigidbody = bulletObject.AddComponent < Rigidbody > ();
			rigidbody.mass = Bullet_MassProp.floatValue;
			rigidbody.drag = Bullet_DragProp.floatValue;
			CapsuleCollider capsuleCollider = bulletObject.AddComponent < CapsuleCollider > ();
			capsuleCollider.direction = 2;
			capsuleCollider.radius = meshFilter.sharedMesh.bounds.extents.x;
			capsuleCollider.height = meshFilter.sharedMesh.bounds.size.z;
			capsuleCollider.material = Bullet_PhysicMatProp.objectReferenceValue as PhysicMaterial;
			if (Trail_FlagProp.boolValue) {
				TrailRenderer trailRenderer = bulletObject.AddComponent < TrailRenderer > ();
				trailRenderer.startWidth = Trail_Start_WidthProp.floatValue;
				trailRenderer.endWidth = Trail_End_WidthProp.floatValue;
				trailRenderer.time = Trail_TimeProp.floatValue;
				trailRenderer.material = Trail_MaterialProp.objectReferenceValue as Material;
			}
			// Add Scripts
			Bullet_Control_CS bulletScript = bulletObject.AddComponent < Bullet_Control_CS > ();
			bulletScript.Type = 0;
			bulletScript.This_Transform = bulletObject.transform;
			bulletScript.This_Rigidbody = rigidbody;
			bulletScript.Impact_Object = Impact_ObjectProp.objectReferenceValue as GameObject;
			bulletScript.Ricochet_Object = Ricochet_ObjectProp.objectReferenceValue as GameObject;
			// Set Tag
			bulletObject.tag = "Finish";
			// Create Prefab.
			if (Directory.Exists ("Assets/Physics Tank Maker/User/") == false) {
				AssetDatabase.CreateFolder ("Assets/Physics Tank Maker", "User");
			}
			string path = "Assets/Physics Tank Maker/User/" + "AP_Bullet (" + DateTime.Now.ToString ("yyMMdd_HHmmss") + ").prefab";
			PrefabUtility.CreatePrefab (path, bulletObject);
			Debug.Log ("New AP Bullet prefab has been created in 'User' folder.");
			DestroyImmediate (bulletObject);
			AP_Bullet_PrefabProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath (path, typeof (GameObject)) as GameObject;
		}

		void Create_Prefab_HE ()
		{
			if (Bullet_Mesh_HEProp.objectReferenceValue == null) {
				Debug.LogWarning("Set the 'Mesh' for the bullet prefab.");
				return;
			}

			// Create GameObject & Set Transform
			GameObject bulletObject = new GameObject ("Bullet");
			bulletObject.transform.localScale = Bullet_Scale_HEProp.vector3Value;
			// Add Components
			MeshRenderer meshRenderer = bulletObject.AddComponent < MeshRenderer > ();
			meshRenderer.material = Bullet_Material_HEProp.objectReferenceValue as Material;
			MeshFilter meshFilter = bulletObject.AddComponent < MeshFilter > ();
			meshFilter.mesh = Bullet_Mesh_HEProp.objectReferenceValue as Mesh;
			Rigidbody rigidbody = bulletObject.AddComponent < Rigidbody > ();
			rigidbody.mass = Bullet_Mass_HEProp.floatValue;
			rigidbody.drag = Bullet_Drag_HEProp.floatValue;
			CapsuleCollider capsuleCollider = bulletObject.AddComponent < CapsuleCollider > ();
			capsuleCollider.direction = 2;
			capsuleCollider.radius = meshFilter.sharedMesh.bounds.extents.x;
			capsuleCollider.height = meshFilter.sharedMesh.bounds.size.z;
			if (Trail_Flag_HEProp.boolValue) {
				TrailRenderer trailRenderer = bulletObject.AddComponent < TrailRenderer > ();
				trailRenderer.startWidth = Trail_Start_Width_HEProp.floatValue;
				trailRenderer.endWidth = Trail_End_Width_HEProp.floatValue;
				trailRenderer.time = Trail_Time_HEProp.floatValue;
				trailRenderer.material = Trail_Material_HEProp.objectReferenceValue as Material;
			}
			// Add Scripts
			Bullet_Control_CS bulletScript = bulletObject.AddComponent < Bullet_Control_CS > ();
			bulletScript.Type = 1;
			bulletScript.This_Transform = bulletObject.transform;
			bulletScript.This_Rigidbody = rigidbody;
			bulletScript.Explosion_Force = Explosion_ForceProp.floatValue;
			bulletScript.Explosion_Radius = Explosion_RadiusProp.floatValue;
			bulletScript.Explosion_Object = Explosion_ObjectProp.objectReferenceValue as GameObject;
			// Set Tag
			bulletObject.tag = "Finish";
			// Create Prefab.
			if (Directory.Exists ("Assets/Physics Tank Maker/User/") == false) {
				AssetDatabase.CreateFolder ("Assets/Physics Tank Maker", "User");
			}
			string path = "Assets/Physics Tank Maker/User/" + "HE_Bullet (" + DateTime.Now.ToString ("yyMMdd_HHmmss") + ").prefab";
			PrefabUtility.CreatePrefab (path, bulletObject);
			Debug.Log ("New HE Bullet prefab has been created in 'User' folder.");
			DestroyImmediate (bulletObject);
			HE_Bullet_PrefabProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath (path, typeof (GameObject)) as GameObject;
		}

	}

}
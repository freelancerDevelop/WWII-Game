using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Tank_ID_Control_CS))]
	public class Tank_ID_Control_CSEditor : Editor
	{


		SerializedProperty Camera_Pivot_PrefabProp;

		GameObject thisGameObject;
		bool isActiveInHierarchy;


		void OnEnable ()
		{
			Camera_Pivot_PrefabProp = serializedObject.FindProperty ("Camera_Pivot_Prefab");

			thisGameObject = Selection.activeGameObject;

			// Stop this script when this prefab is selected in the Project window.
			if (thisGameObject && thisGameObject.activeInHierarchy) {
				isActiveInHierarchy = true;
			}
			else {
				isActiveInHierarchy = false;
			}
		}


		public override void OnInspectorGUI ()
		{
			if (isActiveInHierarchy == false) {
				return;
			}

			if (EditorApplication.isPlaying) {
				return;
			}

			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();
			EditorGUILayout.Space ();

			Camera_Pivot_PrefabProp.objectReferenceValue = EditorGUILayout.ObjectField ("'Camera_Pivot' Prefab", Camera_Pivot_PrefabProp.objectReferenceValue, typeof(GameObject), true);
			EditorGUILayout.Space ();

			if (GUILayout.Button ("Add New Scripts")) {
				Add_New_Scripts ();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}
	

		void Add_New_Scripts()
		{
			Update_Top_Object();
			Update_MainBody();
			Add_Camera_Components();
			Update_Cannon_Base();
			Update_Gun_Camera();
			Convert_Sound_Scripts();
			Convert_Damage_Scripts();
			Update_AudioSource_Settings();
			Remove_AI_Semi_Core();
			Update_AI_CS_Settings();
		}


		void Update_Top_Object()
		{
			var oldScript = thisGameObject.GetComponent <Tank_ID_Control_CS>();

			// Add "ID_Settings_CS".
			var idScript = thisGameObject.GetComponent <ID_Settings_CS>();
			if (idScript == null) {
				idScript = thisGameObject.AddComponent <ID_Settings_CS>();
			}
			idScript.Tank_ID = oldScript.Tank_ID;
			idScript.Relationship = oldScript.Relationship;

			// Add "Input_Type_Settings_CS".
			var inputScript = thisGameObject.GetComponent <Input_Type_Settings_CS>();
			if (inputScript == null) {
				inputScript = thisGameObject.AddComponent <Input_Type_Settings_CS>();
			}
			inputScript.Use_Auto_Lead = oldScript.Auto_Lead;

			// Add "Respawn_Controller_CS".
			var respawnScript = thisGameObject.GetComponent <Respawn_Controller_CS>();
			if (respawnScript == null) {
				respawnScript = thisGameObject.AddComponent <Respawn_Controller_CS>();
			}
			respawnScript.This_Prefab = oldScript.This_Prefab;
			respawnScript.Respawn_Times = oldScript.ReSpawn_Times;
			respawnScript.Auto_Respawn_Interval = oldScript.ReSpawn_Interval;
			respawnScript.Remove_After_Death = (oldScript.Remove_Time != Mathf.Infinity);
			respawnScript.Remove_Interval = oldScript.Remove_Time;

			// Add "AI_Headquaters_Helper_CS".
			var helperScript = thisGameObject.GetComponent <AI_Headquaters_Helper_CS>();
			if (helperScript == null) {
				helperScript = thisGameObject.AddComponent <AI_Headquaters_Helper_CS>();
			}
			var bodyScript = thisGameObject.GetComponentInChildren <MainBody_Setting_CS>();
			if (bodyScript) {
				helperScript.Visibility_Upper_Offset = bodyScript.AI_Upper_Offset;
			}

			// Add "Special_Settings_CS".
			var settingsScript = thisGameObject.GetComponent <Special_Settings_CS>();
			if (settingsScript == null) {
				settingsScript = thisGameObject.AddComponent <Special_Settings_CS>();
			}
			settingsScript.Attack_Multiplier = oldScript.Attack_Multiplier;

			// Add "AI_Settings_CS".
			if (thisGameObject.GetComponentInChildren <AI_CS>()) {
				var aiSettingsScript = thisGameObject.GetComponent <AI_Settings_CS>();
				if (aiSettingsScript == null) {
					aiSettingsScript = thisGameObject.AddComponent <AI_Settings_CS>();
				}
				aiSettingsScript.WayPoint_Pack = oldScript.WayPoint_Pack;
				aiSettingsScript.Patrol_Type = oldScript.Patrol_Type;
				aiSettingsScript.Follow_Target = oldScript.Follow_Target;
				aiSettingsScript.No_Attack = oldScript.No_Attack;
				aiSettingsScript.Breakthrough = oldScript.Breakthrough;
				aiSettingsScript.Visibility_Radius = oldScript.Visibility_Radius;
				aiSettingsScript.Approach_Distance = oldScript.Approach_Distance;
				aiSettingsScript.OpenFire_Distance = oldScript.OpenFire_Distance;
				aiSettingsScript.Lost_Count = oldScript.Lost_Count;
				aiSettingsScript.Face_Offest_Angle = oldScript.Face_Offest_Angle;
				aiSettingsScript.AI_State_Text = oldScript.AI_State_Text;
				aiSettingsScript.Tank_Name = oldScript.Tank_Name;
			}
		}


		void Update_MainBody()
		{
			GameObject bodyObject = thisGameObject.GetComponentInChildren <Rigidbody>().gameObject;

			// Add "Aiming_Control_CS".
			var aimingScript = bodyObject.GetComponent <Aiming_Control_CS>();
			if (aimingScript == null) {
				aimingScript = bodyObject.AddComponent <Aiming_Control_CS>();
				for (int i = 0; i < 9; i++) {
					UnityEditorInternal.ComponentUtility.MoveComponentUp(aimingScript);
				}
			}

			// Add "UI_Aim_Marker_Control_CS".
			var aimMarkerScript = bodyObject.GetComponent <UI_Aim_Marker_Control_CS>();
			if (aimMarkerScript == null) {
				aimMarkerScript = bodyObject.AddComponent <UI_Aim_Marker_Control_CS>();
				for (int i = 0; i < 9; i++) {
					UnityEditorInternal.ComponentUtility.MoveComponentUp(aimMarkerScript);
				}
			}

			// Add "Damage_Control_Center_CS".
			var damageCenterScript = bodyObject.GetComponent <Damage_Control_Center_CS>();
			if (damageCenterScript == null) {
				damageCenterScript = bodyObject.AddComponent <Damage_Control_Center_CS>();
				for (int i = 0; i < 9; i++) {
					UnityEditorInternal.ComponentUtility.MoveComponentUp(damageCenterScript);
				}
				Turret_Base_CS[] turretBaseScripts = bodyObject.GetComponentsInChildren <Turret_Base_CS>();
				damageCenterScript.Turret_Num = turretBaseScripts.Length;
				damageCenterScript.Turret_Props = new TurretDamageControlProp[turretBaseScripts.Length];
				for (int i = 0; i < damageCenterScript.Turret_Props.Length; i++) {
					damageCenterScript.Turret_Props[i].turretBaseTransform = turretBaseScripts[i].transform;
					damageCenterScript.Turret_Props[i].hitPoints = 1000.0f;
					damageCenterScript.Turret_Props[i].damageThreshold = 100.0f;
					damageCenterScript.Turret_Props[i].blowOff = true;
					damageCenterScript.Turret_Props[i].mass = 200.0f;
					damageCenterScript.Turret_Props[i].destroyedEffectPrefab = turretBaseScripts[i].Damage_Effect_Object;
				}
			}
		}


		void Update_Cannon_Base()
		{
			Cannon_Fire_CS cannonFireScript = thisGameObject.GetComponentInChildren <Cannon_Fire_CS>();
			if (cannonFireScript == null) {
				return;
			}
			GameObject cannonBaseObject = cannonFireScript.gameObject;

			// Add "UI_Reloading_Circle_CS".
			var reloadingCircleScript = cannonBaseObject.GetComponent <UI_Reloading_Circle_CS>();
			if (reloadingCircleScript == null) {
				reloadingCircleScript = cannonBaseObject.AddComponent <UI_Reloading_Circle_CS>();
			}
		}


		void Update_Gun_Camera()
		{
			var gunCameraScript = thisGameObject.GetComponentInChildren <Gun_Camera_CS>();
			if (gunCameraScript == null) {
				return;
			}

			// Set the "Gun_Camera_CS".
			Camera gunCamera = gunCameraScript.GetComponent <Camera>();
			gunCamera.rect = new Rect (0.0f, 0.0f, 1.0f, 1.0f);
			gunCameraScript.Gun_Camera = gunCamera;
			Camera[] tankCameras = thisGameObject.GetComponentsInChildren <Camera>();
			foreach (Camera tankCamera in tankCameras) {
				if (tankCamera != gunCamera) {
					gunCameraScript.Main_Camera = tankCamera;
					break;
				}
			}

			/*
			// Remove "GUILayer".
			GUILayer guiLayer = gunCameraScript.GetComponent <GUILayer>();
			if (guiLayer) {
				DestroyImmediate(guiLayer);
			}
			*/

			// Add "Reticle_Control_CS".
			var reticleScript = gunCameraScript.gameObject.GetComponent <Reticle_Control_CS>();
			if (reticleScript == null) {
				reticleScript = gunCameraScript.gameObject.AddComponent <Reticle_Control_CS>();
			}
			reticleScript.Gun_Camera = gunCamera;
			for (int i = 0; i < 2; i++) {
				UnityEditorInternal.ComponentUtility.MoveComponentUp(reticleScript);
			}

			// Add "ReticleWheel_Control_CS".
			var reticleWheelScript = gunCameraScript.gameObject.GetComponent <ReticleWheel_Control_CS>();
			if (reticleWheelScript == null) {
				reticleWheelScript = gunCameraScript.gameObject.AddComponent <ReticleWheel_Control_CS>();
			}
			reticleWheelScript.Gun_Camera = gunCamera;
			for (int i = 0; i < 2; i++) {
				UnityEditorInternal.ComponentUtility.MoveComponentUp(reticleWheelScript);
			}
		}


		void Convert_Damage_Scripts()
		{
			Damage_Control_CS[] oldDamageScripts = thisGameObject.GetComponentsInChildren <Damage_Control_CS>();
			foreach (Damage_Control_CS oldDamageScript in oldDamageScripts) {
				switch (oldDamageScript.Type) {
					case 1: // Armor_Collider
						var newArmorColliderScript = oldDamageScript.gameObject.AddComponent <Damage_Control_09_Armor_Collider_CS>();
						for (int i = 0; i < 2; i++) {
							UnityEditorInternal.ComponentUtility.MoveComponentUp(newArmorColliderScript);
						}
						DestroyImmediate(oldDamageScript);
						break;

					case 2: // Turret
					case 3: // Cannon
					case 4: // Barrel
						oldDamageScript.gameObject.AddComponent <Damage_Control_02_Turret_CS>();
						DestroyImmediate(oldDamageScript);
						break;

					case 5: // MainBody
						oldDamageScript.gameObject.AddComponent <Damage_Control_01_MainBody_CS>();
						DestroyImmediate(oldDamageScript);
						break;

					case 6: // Physics_Track
						oldDamageScript.gameObject.AddComponent <Damage_Control_03_Physics_Track_Piece_CS>();
						DestroyImmediate(oldDamageScript);
						break;

					case 8: // Physics_Wheel
						DestroyImmediate(oldDamageScript);
						break;

					case 9: // Track_Collider
						var newTrackColliderScript = oldDamageScript.gameObject.AddComponent <Damage_Control_04_Track_Collider_CS>();
						for (int i = 0; i < 2; i++) {
							UnityEditorInternal.ComponentUtility.MoveComponentUp(newTrackColliderScript);
						}
						DestroyImmediate(oldDamageScript);
						break;
				}
			}
		}


		void Convert_Sound_Scripts()
		{
			Sound_Control_CS[] oldSoundScripts = thisGameObject.GetComponentsInChildren <Sound_Control_CS>();
			foreach (Sound_Control_CS oldSoundScript in oldSoundScripts) {
				switch (oldSoundScript.Type) {
					case 0: // Engine Sound.
						var newEngineSoundScript = oldSoundScript.gameObject.AddComponent <Sound_Control_Engine_CS>();
						newEngineSoundScript.Left_Reference_Rigidbody = oldSoundScript.Left_Wheel_Rigidbody;
						newEngineSoundScript.Right_Reference_Rigidbody = oldSoundScript.Right_Wheel_Rigidbody;
						newEngineSoundScript.Min_Engine_Pitch = oldSoundScript.Min_Engine_Pitch;
						newEngineSoundScript.Max_Engine_Pitch = oldSoundScript.Max_Engine_Pitch;
						UnityEditorInternal.ComponentUtility.MoveComponentUp(newEngineSoundScript);
						DestroyImmediate(oldSoundScript);
						break;

					case 1: // Impact Sound.
						var newImpactScript = oldSoundScript.gameObject.AddComponent <Sound_Control_Impact_CS>();
						for (int i = 0; i < 7; i++) {
							UnityEditorInternal.ComponentUtility.MoveComponentUp(newImpactScript);
						}
						DestroyImmediate(oldSoundScript);
						break;

					case 2: // Turret Motor Sound.
					case 3: // Cannon Motor Sound.
						oldSoundScript.gameObject.AddComponent <Sound_Control_Motor_CS>();
						DestroyImmediate(oldSoundScript);
						break;
				}
			}
		}


		void Add_Camera_Components()
		{
			// Add "Camera_Point (1)".
			GameObject cameraPoint01 = new GameObject ("Camera_Point (1)");
			Transform bodyTransform = thisGameObject.GetComponentInChildren <Rigidbody>().transform;
			cameraPoint01.transform.parent = bodyTransform;
			Turret_Base_CS turretBaseScript = thisGameObject.GetComponentInChildren <Turret_Base_CS>();
			if (turretBaseScript) {
				cameraPoint01.transform.position = turretBaseScript.transform.position;
			}
			else {
				cameraPoint01.transform.position = bodyTransform.position;
			}

			// Add "Camera_Point (2)".
			GameObject cameraPoint02 = new GameObject ("Camera_Point (2)");
			Look_At_Point_CS lookAtScript = thisGameObject.GetComponentInChildren <Look_At_Point_CS>();
			if (lookAtScript) {
				cameraPoint02.transform.parent = lookAtScript.transform.parent;
				cameraPoint02.transform.position = lookAtScript.transform.position;
				DestroyImmediate(lookAtScript.gameObject);
			}

			// Add "Camera_Pivot".
			if (Camera_Pivot_PrefabProp.objectReferenceValue != null) {
				GameObject cameraPivotObject = Instantiate(Camera_Pivot_PrefabProp.objectReferenceValue as GameObject);
				cameraPivotObject.name = "Camera_Pivot";
				cameraPivotObject.transform.parent = thisGameObject.GetComponentInChildren <Rigidbody>().transform;
				cameraPivotObject.transform.localPosition = Vector3.zero;
				cameraPivotObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 10.0f);

				Camera_Points_Manager_CS cameraPointsScript = cameraPivotObject.GetComponent <Camera_Points_Manager_CS>();
				cameraPointsScript.Camera_Points[0].Camera_Point = cameraPoint01.transform;
				cameraPointsScript.Camera_Points[1].Camera_Point = cameraPoint02.transform;
			}
		}


		void Update_AudioSource_Settings()
		{
			AudioSource[] audioSources = thisGameObject.GetComponentsInChildren <AudioSource>();
			foreach (AudioSource audioSource in audioSources) {
				audioSource.spread = 100.0f;
				audioSource.minDistance = 5.0f;
				audioSource.maxDistance = 50.0f;
			}
		}


		void Remove_AI_Semi_Core()
		{
			AI_Semi_CS aiSemiScript = thisGameObject.GetComponentInChildren <AI_Semi_CS>();
			if (aiSemiScript) {
				DestroyImmediate(aiSemiScript.gameObject);
			}
		}


		void Update_AI_CS_Settings()
		{
			AI_CS aiScript = thisGameObject.GetComponentInChildren <AI_CS>();
			if (aiScript) {
				aiScript.Min_Speed_Rate = 0.1f;
			}
		}

	}

}
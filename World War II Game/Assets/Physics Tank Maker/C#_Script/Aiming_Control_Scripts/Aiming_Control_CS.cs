using System.Collections;
using UnityEngine;
using System.Collections.Generic;


namespace ChobiAssets.PTM
{
	
	public class Aiming_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "MainBody" of the tank.
		 * This script controls the aiming of the tank.
		 * "Turret_Horizontal_CS" and "Cannon_Vertical_CS" scripts rotate the turret and cannon referring to this variables.
		*/

		// User options >>
		public float Aiming_Sensibility = 0.2f;
		public float OpenFire_Angle = 180.0f;
		public bool Use_Auto_Elevation_Angle = true;
		// << User options


		// Set by "Input_Type_Settings_CS".
		public int Input_Type = 0;
		public bool Use_Auto_Lead = false;


		Turret_Horizontal_CS[] turretHorizontalScripts;
		Cannon_Vertical_CS[] cannonVerticalScripts;
		public bool Use_Auto_Turn; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".

		// For auto-turn.
		public int Mode; // Referred to from "UI_Aim_Marker_Control_CS". // 0 => Keep the initial positon, 1 => Free aiming,  2 => Locking on.
		Transform rootTransform;
		public Vector3 Target_Position; // Referred to from "Turret_Horizontal_CS", "Cannon_Vertical_CS", "UI_Aim_Marker_Control_CS", "ReticleWheel_Control_CS".
		public Transform Target_Transform; // Referred to from "UI_Aim_Marker_Control_CS", "UI_HP_Bars_Target_CS".
		Vector3 targetOffset;
		public Rigidbody Target_Rigidbody; // Referred to from "Turret_Horizontal_CS".
		public Vector3 Adjust_Angle; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".
		float spherecastRadius = 1.0f;
		Ray ray;
		int layerMask = ~((1 << 2) + (1 << 9) + (1 << 10)); // Layer 2 = Ignore Ray, Layer 9 = Wheel, Layer 10 = Ignore All.

		// For manual-turn.
		public float Turret_Turn_Rate; // Referred to from "Turret_Horizontal_CS".
		public float Cannon_Turn_Rate; // Referred to from "Cannon_Vertical_CS".

		UI_HP_Bars_Target_CS targetHPBarsScript;

		Aiming_Control_Input_00_Base_CS inputScript;

		public bool Is_Selected; // Referred to from "UI_HP_Bars_Target_CS".


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			rootTransform = transform.root;

			// Set the input script.
			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
					inputScript = gameObject.AddComponent <Aiming_Control_Input_01_Mouse_Keyboard_CS>();
					break;
				case 2: // Mouse + Keyboard (Legacy)
					inputScript = gameObject.AddComponent <Aiming_Control_Input_02_Mouse_Keyboard_Legacy_CS>();
					break;
				case 3: // GamePad (Single stick)
					inputScript = gameObject.AddComponent <Aiming_Control_Input_03_For_Single_Stick_Drive_CS>();
					break;
				case 4: // GamePad (Twin sticks)
					inputScript = gameObject.AddComponent <Aiming_Control_Input_04_For_Twin_Sticks_Drive_CS>();
					break;
				case 5: // GamePad (Triggers)
					inputScript = gameObject.AddComponent <Aiming_Control_Input_05_For_Triggers_Drive_CS>();
					break;

				case 10: // AI
					Use_Auto_Turn = true;
					Use_Auto_Lead = true;
					Use_Auto_Elevation_Angle = true;
					OpenFire_Angle = GetComponentInChildren <AI_CS>().Fire_Angle;
					break;
			}

			// Prepare the input script.
			if (inputScript != null) {
				inputScript.Prepare(this);
			}

			// Get the "Turret_Horizontal_CS" and "Cannon_Vertical_CS" scripts in the tank.
			turretHorizontalScripts = GetComponentsInChildren <Turret_Horizontal_CS>();
			cannonVerticalScripts = GetComponentsInChildren <Cannon_Vertical_CS>();
		}
			

		void LateUpdate ()
		{
			// Update the target position.
			Update_Target_Position();

			if (Is_Selected == false) {
				return;
			}

			if (inputScript != null) {
				inputScript.Get_Input();
			}
		}


		void Update_Target_Position()
		{
			if (Target_Transform) {
				// Check the target is living.
				if (Target_Transform.root.tag == "Finish") { // The target has been destroyed.
					Target_Transform = null;
					return;
				}

				// Update the target position.
				Target_Position = Target_Transform.position + (Target_Transform.forward * targetOffset.z) + (Target_Transform.right * targetOffset.x) + (Target_Transform.up * targetOffset.y);
			}
		}


		public void Switch_Mode ()
		{
			switch (Mode) {
				case 0: // Keep the initial positon.
					Target_Transform = null;
					Target_Rigidbody = null;
					for (int i = 0; i < turretHorizontalScripts.Length; i++) {
						turretHorizontalScripts[i].Stop_Tracking();
					}
					for (int i = 0; i < cannonVerticalScripts.Length; i++) {
						cannonVerticalScripts[i].Stop_Tracking();
					}
					break;

				case 1: // Free aiming.
					Target_Transform = null;
					Target_Rigidbody = null;
					for (int i = 0; i < turretHorizontalScripts.Length; i++) {
						turretHorizontalScripts[i].Start_Tracking();
					}
					for (int i = 0; i < cannonVerticalScripts.Length; i++) {
						cannonVerticalScripts[i].Start_Tracking(false);
					}
					break;

				case 2: // Locking on.
					for (int i = 0; i < turretHorizontalScripts.Length; i++) {
						turretHorizontalScripts[i].Start_Tracking();
					}
					for (int i = 0; i < cannonVerticalScripts.Length; i++) {
						cannonVerticalScripts[i].Start_Tracking(Use_Auto_Elevation_Angle);
					}
					break;
			}
		}


		public void Cast_Ray_Lock (Vector3 screenPos)
		{ // Find a target with a rigidbody by casting a ray from the main camera.
			ray = Camera.main.ScreenPointToRay (screenPos);
			RaycastHit raycastHit;
			if (Physics.Raycast (ray, out raycastHit, 2048.0f, layerMask)) {
				if (raycastHit.collider.transform.root != rootTransform) { // The hit collider is not itself.
					// (Note.) When the hit collider has no rigidbody and its parent has a rigidbody, the parent's transform is set as 'RaycastHits.transform'.
					Rigidbody tempRigidbody = raycastHit.transform.GetComponent <Rigidbody>(); // The rigidbody should be in a MainBody, because wheels are ignored by the LayerMask.
					if (tempRigidbody) {
						Target_Transform = raycastHit.collider.transform; // Set the hit collider as the target.
						targetOffset = Target_Transform.InverseTransformPoint (raycastHit.point);
						if (Target_Transform.localScale != Vector3.one) { // for "Armor_Collider".
							targetOffset.x *= Target_Transform.localScale.x;
							targetOffset.y *= Target_Transform.localScale.y;
							targetOffset.z *= Target_Transform.localScale.z;
						}
						Target_Rigidbody = tempRigidbody;
						Mode = 2; // Lock on.
						Switch_Mode ();
						return;
					} else { // No Rigidbody.
						Target_Transform = null; // Set temporarily.
						Target_Position = raycastHit.point; // Set temporarily.
						Cast_Ray_Sphere_Lock (screenPos); // Now, cast a sphere to find a target with a rigidbody.
						return;
					}
				} else { // The ray hits itself.
					Target_Transform = null;
					Mode = 0; // Keep the initial position.
					Switch_Mode ();
					return;
				}
			} else { // The ray does not hit anythig.
				Target_Transform = null; // Set temporarily.
				screenPos.z = 1024.0f;
				Target_Position = Camera.main.ScreenToWorldPoint (screenPos); // Set temporarily.
				Cast_Ray_Sphere_Lock (screenPos); // Now, cast a sphere to find a target with a rigidbody.
				return;
			}
		}


		void Cast_Ray_Sphere_Lock (Vector3 screenPos)
		{ // Cast a sphere to find a target with a rigidbody.
			RaycastHit [] raycastHits;
			raycastHits = Physics.SphereCastAll (ray, spherecastRadius, 2048.0f, layerMask);
			for (int i = 0; i < raycastHits.Length; i++) {
				RaycastHit raycastHit = raycastHits [i];
				Transform colliderTransform = raycastHit.collider.transform;
				if (colliderTransform.root != rootTransform && colliderTransform.root.tag != "Finish") { // The hit collider is not itself, and is not destroyed.
					// (Note.) When the hit collider has no rigidbody and its parent has a rigidbody, the parent's transform is set as 'RaycastHits.transform'.
					Rigidbody tempRigidbody = raycastHit.transform.GetComponent<Rigidbody> (); // The rigidbody should be in a MainBody, because wheels are ignored by the LayerMask.
					if (tempRigidbody) {
						Target_Transform = raycastHit.transform; // Set the MainBody as the target.
						targetOffset = Vector3.zero;
						targetOffset.y = 0.5f;
						Target_Rigidbody = tempRigidbody;
						Mode = 2; // Lock on.
						Switch_Mode ();
						return;
					}
				}
			}
			// The target with a rigidbody cannot be found.
			Mode = 2; // Lock on.
			Switch_Mode ();
		}


		public void Cast_Ray_Sphere_Free (Vector3 screenPos)
		{ // Find a target with a rigidbody by casting a sphere ray from the main camera.
			ray = Camera.main.ScreenPointToRay (screenPos);
			RaycastHit [] raycastHits;
			raycastHits = Physics.SphereCastAll (ray, spherecastRadius, 2048.0f, layerMask);
			for (int i = 0; i < raycastHits.Length; i++) {
				Transform colliderTransform = raycastHits[i].collider.transform;
				if (colliderTransform.root != rootTransform && colliderTransform.root.tag != "Finish") { // The hit collider is not itself, and is not destroyed.
					// (Note.) When the hit collider has no rigidbody and its parent has a rigidbody, the parent's transform is set as 'RaycastHits.transform'.
					Rigidbody tempRigidbody = raycastHits[i].transform.GetComponent<Rigidbody> (); // The rigidbody should be in a MainBody, because wheels are ignored by the LayerMask.
					if (tempRigidbody) {
						// Set the MainBody as the target.
						Target_Transform = raycastHits[i].transform;
						targetOffset = Vector3.zero;
						targetOffset.y = 0.5f;
						return;
					}
				}
			} // Target with a rigidbody cannot be found.
			// Set the target position by casting a ray.
			Cast_Ray_Free (screenPos);
		}


		void Cast_Ray_Free (Vector3 screenPos)
		{ // Set the target position by casting a ray from the main camera.
			RaycastHit raycastHit;
			if (Physics.Raycast (ray, out raycastHit, 2048.0f, layerMask)) {
				if (raycastHit.collider.transform.root != rootTransform) { // The hit collider is not itself.
					// Set the hit position.
					Target_Transform = null;
					Target_Position = raycastHit.point;
					return;
				} // The ray hits itself.
			} // The ray does not hit anythig.
			// Set the position through this tank.
			Target_Transform = null;
			screenPos.z = 1024.0f;
			Target_Position = Camera.main.ScreenToWorldPoint (screenPos);
		}


		public void Auto_Lock (int direction, int thisRelationship)
		{ // 0 = Left, 1 = Right, 2 = Front.
			// Create the list of all the enemy tanks in the scene.
			ID_Settings_CS[] idScripts = FindObjectsOfType <ID_Settings_CS>();
			List <Transform> enemyTankList = new List <Transform>();
			for (int i = 0; i < idScripts.Length; i++) {
				if (idScripts[i].Relationship == thisRelationship) { // The tank is a friend.
					continue;
				}
				Rigidbody tempRigidbody = idScripts[i].GetComponentInChildren <Rigidbody>(); // Find the MainBody's rigidbody.
				if (tempRigidbody) {
					enemyTankList.Add(tempRigidbody.transform); // Set the MainBody's transform.
				}
			}

			float baseAng;
			if (direction != 2 && Target_Transform) {
				Vector3 currentLocalPos = Camera.main.transform.InverseTransformPoint (Target_Position);
				baseAng = Vector2.Angle (Vector2.up, new Vector2 (currentLocalPos.x, currentLocalPos.z)) * Mathf.Sign (currentLocalPos.x);
			} else {
				baseAng = 0.0f;
			}

			int targetIndex = 0;
			int oppositeTargetIndex = 0;
			float minAng = 180.0f;
			float maxAng = 0.0f;
			for (int i = 0; i < enemyTankList.Count; i++) {
				if (enemyTankList[i].root.tag == "Finish" || (Target_Transform && Target_Transform == enemyTankList[i])) {
					continue;
				}
				Vector3 localPos = Camera.main.transform.InverseTransformPoint (enemyTankList[i].position);
				float tempAng = Vector2.Angle (Vector2.up, new Vector2 (localPos.x, localPos.z)) * Mathf.Sign (localPos.x);
				float deltaAng = Mathf.Abs (Mathf.DeltaAngle (tempAng, baseAng));
				if ((direction == 0 && tempAng > baseAng) || (direction == 1 && tempAng < baseAng)) { // on the opposite side.
					if (deltaAng > maxAng) {
						oppositeTargetIndex = i;
						maxAng = deltaAng;
					}
					continue;
				}
				if (deltaAng < minAng) {
					targetIndex = i;
					minAng = deltaAng;
				}
			}

			if (minAng != 180.0f) { // Target is found on the indicated side.
				Target_Transform = enemyTankList [targetIndex];
				Target_Rigidbody = enemyTankList [targetIndex].GetComponent <Rigidbody>();
				targetOffset = Vector3.zero;
				targetOffset.y = 0.5f;
				Mode = 2; // Lock on.
				Switch_Mode ();
			}
			else if (maxAng != 0.0f) { // Target is not found on the indicated side, but it could be found on the opposite side.
				Target_Transform = enemyTankList [oppositeTargetIndex];
				Target_Rigidbody = enemyTankList [oppositeTargetIndex].GetComponent <Rigidbody>();
				targetOffset = Vector3.zero;
				targetOffset.y = 0.5f;
				Mode = 2; // Lock on.
				Switch_Mode ();
			}

			if (Target_Transform) {
				StartCoroutine ("Send_Target_Position");
			}
		}


		IEnumerator Send_Target_Position ()
		{
			// Send the target position to the "Camera_Rotation_CS" in the 'Camera_Pivot' object.
			yield return new WaitForFixedUpdate ();
			Camera_Rotation_CS cameraScript = GetComponentInChildren <Camera_Rotation_CS>();
			if (cameraScript) {
				cameraScript.Look_At_Target(Target_Transform.position);
			}
		}


		public void AI_Lock_On (Transform tempTransform)
		{ // Called from "AI_CS".
			Target_Transform = tempTransform;
			Target_Rigidbody = Target_Transform.GetComponent <Rigidbody>();
			Mode = 2;
			Switch_Mode ();
		}


		public void AI_Lock_Off ()
		{ // Called from "AI_CS".
			Target_Transform = null;
			Mode = 0;
			Switch_Mode ();
		}


		public void AI_Random_Offset ()
		{ // Called from "Cannon_Fire".
			Vector3 newOffset;
			newOffset.x = Random.Range (-0.5f, 0.5f);
			newOffset.y = Random.Range ( 0.0f, 1.5f);
			newOffset.z = Random.Range (-1.0f, 1.0f);
			targetOffset = newOffset;
		}


		void Get_Input_Type(int inputType)
		{ // Called from "Input_Settings_CS".
			Input_Type = inputType;
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			this.Is_Selected = isSelected;

			if (isSelected == false) {
				return;
			} // This tank is selected.

			// Send this reference to the "UI_HP_Bars_Target_CS" in the scene.
			if (targetHPBarsScript == null) {
				targetHPBarsScript = FindObjectOfType <UI_HP_Bars_Target_CS>();
			}
			if (targetHPBarsScript) {
				targetHPBarsScript.Get_Aiming_Script(this);
			}
		}


		void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			Destroy (inputScript as Object);
			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}

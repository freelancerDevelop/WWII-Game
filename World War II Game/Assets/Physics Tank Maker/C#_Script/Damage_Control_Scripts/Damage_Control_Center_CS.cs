using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace ChobiAssets.PTM
{

	[ System.Serializable]
	public struct TurretDamageControlProp
	{
		public Transform turretBaseTransform;
		public float hitPoints;
		public float damageThreshold;
		public bool blowOff;
		public float mass;
		public GameObject destroyedEffectPrefab;
	}


	public class Damage_Control_Center_CS : MonoBehaviour
	{
		/* 
		 * This script is attached to the "MainBody" in the tank.
		 * This script controls the hit points of the tank parts, and their destruction processes.
		 * This script works in combination with "Damage_Control_##_##_CS" scripts in the tank parts.
		*/


		// User options >>
		public float MainBody_HP = 1000.0f;
		public float Left_Track_HP = 1000.0f;
		public float Right_Track_HP = 1000.0f;
		public float MainBody_Damage_Threshold = 100.0f;
		public float Track_Damage_Threshold = 50.0f;
		public int Turret_Num = 1;
		public TurretDamageControlProp[] Turret_Props;
		// << User options
		

		// Referred to from "UI_HP_Bars_Self_CS" and "UI_HP_Bars_Target_CS".
		public float Initial_Body_HP;
		public float Initial_Turret_HP;
		public float Initial_Left_Track_HP;
		public float Initial_Right_Track_HP;

		Transform bodyTransform;
		UI_HP_Bars_Self_CS selfHPBarsScript;
		bool isDead;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			bodyTransform = transform;

			// Store the initial HP values for the "UI_HP_Bars_Self_CS" and "UI_HP_Bars_Target_CS".
			Initial_Body_HP = MainBody_HP;
			Initial_Turret_HP = Turret_Props[0].hitPoints;
			Initial_Left_Track_HP = Left_Track_HP;
			Initial_Right_Track_HP = Right_Track_HP;
		}


		public bool Receive_Damage (float damage, int type, int index)
		{ // Called from "Damage_Control_##_##_CS" scripts in the tank.
			switch (type) {
				case 0: // MainBody
					return MainBody_Damaged(damage);

				case 1: // Turret
					return Turret_Damaged(damage, index);

				case 2: // Physics_Track piece
				case 3: // Track_Collider
					return Track_Damaged(damage, index);

				default:
					return false;
			}
		}


		bool MainBody_Damaged(float damage)
		{
			if (damage < MainBody_Damage_Threshold) { // Never receive any damage under the threshold value.
				return false;
			}

			MainBody_HP -= damage;
			if (MainBody_HP <= 0) {
				MainBody_Destroyed();
				return true;
			}
			return false;
		}


		bool Turret_Damaged(float damage, int index)
		{
			if (Turret_Props[index].turretBaseTransform == null) { // The turret had already been destroyed.
				return false;
			}

			if (damage < Turret_Props[index].damageThreshold) { // Never receive any damage under the threshold value.
				return false;
			}

			Turret_Props[index].hitPoints -= damage;
			if (Turret_Props[index].hitPoints <= 0) {
				if (index == 0) { // The main turret has been destroyed.
					MainBody_Destroyed();
				}
				else {
					Turret_Destroyed(index);
				}
				return true;
			}
			return false;
		}


		bool Track_Damaged(float damage, int index)
		{
			if (damage < Track_Damage_Threshold) { // Never receive any damage under the threshold value.
				return false;
			}

			switch (index) {
				case 0: // Left track
					Left_Track_HP -= damage;
					if (Left_Track_HP <= 0) {
						Track_Destroyed(true);
						return true;
					}
					return false;

				case 1: // Right track
					Right_Track_HP -= damage;
					if (Right_Track_HP <= 0) {
						Track_Destroyed(false);
						return true;
					}
					return false;

				default:
					return false;
			}
		}


		void MainBody_Destroyed()
		{
			// Check the tank has already been dead or not.
			if (isDead) { // (Note.) When the tank has been destroyed by an explosion, this function might be called several times in the one frame.
				return;
			}
			isDead = true;

			// Set the HP value to zero.
			MainBody_HP = 0.0f;

			// Set the tag.
			bodyTransform.root.tag = "Finish";

			// Destroy all the turret.
			for (int i = 0; i < Turret_Props.Length; i++) {
				Turret_Destroyed(i);
			}

			// Send Message to "Damage_Control_00_MainBody_CS", "Damage_Control_01_Turret_CS", "Respawn_Controller_CS", "AI_CS", "UI_Aim_Marker_Control_CS", "Aiming_Marker_Control_CS", "Drive_Control_CS", "Drive_Wheel_Parent_CS", "Drive_Wheel_CS", "Steer_Wheel_CS", "Stabilizer_CS", "Fix_Shaking_Rotation_CS", "Sound_Control_##_CS".
			bodyTransform.parent.BroadcastMessage ("MainBody_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);

			// Add NavMeshObstacle to the MainBody.
			NavMeshObstacle navMeshObstacle = bodyTransform.gameObject.AddComponent <NavMeshObstacle>();
			navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
			navMeshObstacle.carving = true;
			navMeshObstacle.carvingMoveThreshold = 1.0f;

			/// Release the parking brake, and Destroy this script.
			StartCoroutine ("Disable_MainBody_Constraints");
		}


		IEnumerator Disable_MainBody_Constraints ()
		{
			// Disable the rigidbody constraints in the MainBody in order to release the parking brake.
			yield return new WaitForFixedUpdate (); // This wait is required for PhysX.
			Rigidbody bodyRigidBody = bodyTransform.GetComponent<Rigidbody>();
			bodyRigidBody.constraints = RigidbodyConstraints.None;
		}


		void Turret_Destroyed(int index)
		{
			if (Turret_Props[index].turretBaseTransform == null) { // The turret had already been destroyed.
				return;
			}

			// Set the HP value to zero.
			Turret_Props[index].hitPoints = 0.0f;

			// Create the destroyed effect.
			if (Turret_Props[index].destroyedEffectPrefab) {
				Instantiate (Turret_Props[index].destroyedEffectPrefab, Turret_Props[index].turretBaseTransform.position, Turret_Props[index].turretBaseTransform.rotation, bodyTransform); // (Note.) The parent must be the MainBody, because the turret might be blown away.
			}

			// Send Message to "Damage_Control_01_Turret_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS", "Cannon_Fire_CS", "Gun_Camera_CS", "Recoil_Brake_CS", "Sound_Control_Motor_CS".
			Turret_Props[index].turretBaseTransform.BroadcastMessage ("Turret_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);

			// Blow off the turret.
			if (Turret_Props[index].blowOff == true) {
				Rigidbody turretRigidbody = Turret_Props[index].turretBaseTransform.gameObject.AddComponent <Rigidbody>();
				turretRigidbody.mass = Turret_Props[index].mass;
				turretRigidbody.AddForceAtPosition(Turret_Props[index].turretBaseTransform.up * Random.Range(Turret_Props[index].mass, Turret_Props[index].mass * 5.0f), Turret_Props[index].turretBaseTransform.position + new Vector3(Random.Range(0.0f, 1.0f), 0.0f, Random.Range(0.0f, 1.0f)), ForceMode.Impulse);
				// Change the hierarchy.
				Turret_Props[index].turretBaseTransform.parent = bodyTransform.parent; // Make it a child of the top object.
			}

			// Remove the "turretBaseTransform" from the array element.
			Turret_Props[index].turretBaseTransform = null;
		}


		void Track_Destroyed(bool isLeft)
		{
			// Send message to "Damage_Control_02_Physics_Track_Piece_CS", "Damage_Control_03_Track_Collider_CS", "Track_Joint_CS", "Stabilizer_CS", "Drive_Wheel_CS", "Static_Wheel_CS", "Track_Scroll_CS", "Track_LOD_Control_CS".
			bodyTransform.BroadcastMessage ("Track_Destroyed_Linkage", isLeft, SendMessageOptions.DontRequireReceiver);
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			if (isSelected == false) {
				return;
			} // This tank is selected.

			// Send this reference to the "UI_HP_Bars_Self_CS" in the scene.
			if (selfHPBarsScript == null) {
				selfHPBarsScript = FindObjectOfType <UI_HP_Bars_Self_CS>();
			}
			if (selfHPBarsScript) {
				selfHPBarsScript.Get_Damage_Script(this);
			}
		}

	}

}
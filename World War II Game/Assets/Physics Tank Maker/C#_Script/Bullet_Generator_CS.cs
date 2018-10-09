using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Bullet_Generator_CS : MonoBehaviour
	{
		/* 
		 * This script is attached to the "Bullet_Generator" under the "Barrel_Base" in the tank.
		 * This script instantiates the bullet prefab and shoot it from the muzzle.
		 * Also you can create a prefab for the bullet using this script.
		*/

		// User options >>
		public GameObject AP_Bullet_Prefab;
		public GameObject HE_Bullet_Prefab;
		public GameObject MuzzleFire_Object;
		public float Attack_Point = 500.0f;
		public float Attack_Point_HE = 500.0f;
		public float Initial_Velocity = 500.0f;
		public float Initial_Velocity_HE = 500.0f;

		public float Life_Time = 5.0f;
		public int Initial_Bullet_Type = 0;
		public float Offset = 0.5f;
		public bool Debug_Flag = false;

		// For creating AP bullet prefab.
		public Mesh Bullet_Mesh;
		public Material Bullet_Material;
		public Vector3 Bullet_Scale = new Vector3 (0.762f, 0.762f, 0.762f);
		public float Bullet_Mass = 5.0f;
		public float Bullet_Drag = 0.5f;
		public PhysicMaterial Bullet_PhysicMat;
		public GameObject Impact_Object;
		public GameObject Ricochet_Object;
		public bool Trail_Flag = false;
		public Material Trail_Material;
		public float Trail_Start_Width = 0.01f;
		public float Trail_End_Width = 0.2f;
		public float Trail_Time = 0.1f;

		// For creating HE bullet prefab.
		public Mesh Bullet_Mesh_HE;
		public Material Bullet_Material_HE;
		public Vector3 Bullet_Scale_HE = new Vector3 (0.762f, 0.762f, 0.762f);
		public float Bullet_Mass_HE = 5.0f;
		public float Bullet_Drag_HE = 0.5f;
		public GameObject Explosion_Object;
		public float Explosion_Force = 60000.0f;
		public float Explosion_Radius = 20.0f;
		public bool Trail_Flag_HE = false;
		public Material Trail_Material_HE;
		public float Trail_Start_Width_HE = 0.01f;
		public float Trail_End_Width_HE = 0.2f;
		public float Trail_Time_HE = 0.1f;
		// << User options


		// Set by "Special_Settings_CS".
		public float Attack_Multiplier = 1.0f;

		public int Barrel_Type = 0; // 0 = Single barrel, 1 = Left of twins, 2 = Right of twins. // Set by "Barrel_Base".
		int bulletType;
		float bulletVelocity;
		Transform thisTransform;
		Turret_Horizontal_CS turretScript;
		Cannon_Vertical_CS cannonScript;

		// Only for AI tank.
		public bool Can_Aim; // Set by "AI_CS", and referred to from "Cannon_Fire_Input_99_AI_CS" script.


		void Start()
		{
			Initialize();
		}


		void Initialize ()
		{
			thisTransform = transform;
			cannonScript = GetComponentInParent <Cannon_Vertical_CS>();
			turretScript = GetComponentInParent <Turret_Horizontal_CS >();

			// Switch the bullet type at the first time.
			bulletType = Initial_Bullet_Type - 1; // (Note.) The "bulletType" value is added by 1 soon in the "Switch_Bullet_Type()".
			Switch_Bullet_Type ();
		}


		public void Switch_Bullet_Type ()
		{ // Called from "Aiming_Control_Input_##_##_CS" scripts.
			bulletType += 1;
			if (bulletType > 1) {
				bulletType = 0;
			}

			// Set the bullet velocity.
			switch (bulletType) {
				case 0: // AP
					bulletVelocity = Initial_Velocity;
					break;
				case 1: // HE
					bulletVelocity = Initial_Velocity_HE;
					break;
			}

			// Update the "Bullet_Velocity" value in the "Turret_Horizontal_CS" and "Cannon_Vertical_CS".
			if (turretScript) {
				turretScript.Bullet_Velocity = bulletVelocity;
			}
			if (cannonScript) {
				cannonScript.Bullet_Velocity = bulletVelocity;
			}
		}


		public void Fire_Linkage (int direction)
		{ // Called from "Cannon_Fire_CS".
			if (Barrel_Type == 0 || Barrel_Type == direction) { // Single barrel, or the same direction.
				// Generate the bullet and shoot it.
				StartCoroutine ("Generate_Bullet");
			}
		}


		IEnumerator Generate_Bullet ()
		{
			// Generate the muzzle fire.
			if (MuzzleFire_Object) {
				Instantiate (MuzzleFire_Object, thisTransform.position, thisTransform.rotation, thisTransform);
			}

			// Generate the bullet.
			GameObject bulletObject;
			float attackPoint = 0;
			switch (bulletType) {
				case 0: // AP
					if (AP_Bullet_Prefab == null) {
						Debug.LogError("'AP_Bullet_Prefab' is not assigned in the 'Bullet_Generator'.");
						yield break;
					}
					bulletObject = Instantiate(AP_Bullet_Prefab, thisTransform.position + (thisTransform.forward * Offset), thisTransform.rotation) as GameObject;
					attackPoint = Attack_Point;
					break;
				case 1: // HE
					if (HE_Bullet_Prefab == null) {
						Debug.LogError ("'HE_Bullet_Prefab' is not assigned in the 'Bullet_Generator'.");
						yield break;
					}
					bulletObject = Instantiate (HE_Bullet_Prefab, thisTransform.position + (thisTransform.forward * Offset), thisTransform.rotation) as GameObject;
					attackPoint = Attack_Point_HE;
					break;
				default:
					yield break;
			}

			// Set values of "Bullet_Control_CS" in the bullet.
			Bullet_Control_CS bulletScript= bulletObject.GetComponent <Bullet_Control_CS>();
			bulletScript.Attack_Point = attackPoint;
			bulletScript.Initial_Velocity = bulletVelocity;
			bulletScript.Life_Time = Life_Time;
			bulletScript.Attack_Multiplier = Attack_Multiplier;
			bulletScript.Debug_Flag = Debug_Flag;

			// Set the tag.
			bulletObject.tag = "Finish"; // (Note.) The ray cast for aiming does not hit any object with "Finish" tag.

			// Shoot.
			yield return new WaitForFixedUpdate ();
			Rigidbody rigidbody = bulletObject.GetComponent <Rigidbody>();
			Vector3 currentVelocity = bulletObject.transform.forward * bulletVelocity;
			rigidbody.velocity = currentVelocity;
		}

	}

}
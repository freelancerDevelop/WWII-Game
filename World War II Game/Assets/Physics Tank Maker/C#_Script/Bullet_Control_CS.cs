using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Bullet_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to bullet prefabs.
		 * This script controls the posture of the bullet, and supports the collision detecting by casting a ray while flying.
		 * When the bullet hits the target, this script sends the damage value to the "Damage_Control_##_##_CS" script in the hit collider.
		 * The damage value is calculated considering the hit angle.
		*/


		// User options >>
		public int Type; // 0=AP , 1=HE.
		public Transform This_Transform;
		public Rigidbody This_Rigidbody;
		// Only for AP
		public GameObject Impact_Object;
		public GameObject Ricochet_Object;
		// Only for HE
		public GameObject Explosion_Object;
		public float Explosion_Force;
		public float Explosion_Radius;
		// << User options


		// Set by "Bullet_Generator_CS".
		public float Attack_Point;
		public float Initial_Velocity;
		public float Life_Time;
		public float Attack_Multiplier = 1.0f;
		public bool Debug_Flag = false;

		bool isLiving = true;
		bool isDetecting = true;

		Ray ray = new Ray ();
		int layerMask = ~((1 << 10) + (1 << 2)); // Layer 2 = Ignore Ray, Layer 10 = Ignore All.
		Vector3 nextPos;
		GameObject rayHitObject;
		Vector3 rayHitNormal;
		float rayHitVelocity;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			if (This_Transform == null) {
				This_Transform = transform;
			}
			if (This_Rigidbody == null) {
				This_Rigidbody = GetComponent <Rigidbody>();
			}

			Destroy (this.gameObject, Life_Time);
		}


		void FixedUpdate ()
		{
			if (isLiving == false) {
				return;
			}

			// Set the posture.
			This_Transform.LookAt (This_Rigidbody.position + This_Rigidbody.velocity);

			// Find an obstacle in front of the bullet.
			if (isDetecting) { // This script has not detect any obstacle yet.
				ray.origin = This_Rigidbody.position;
				ray.direction = This_Rigidbody.velocity;
				RaycastHit raycastHit;
				Physics.Raycast (ray, out raycastHit, This_Rigidbody.velocity.magnitude * Time.fixedDeltaTime, layerMask);
				if (raycastHit.collider) { // The ray hits something.
					// Set the position in the next frame, and store the hit information.
					nextPos = raycastHit.point;
					if (Type == 1) { // HE
						// Explode right before the target.
						nextPos += This_Transform.forward * -1.0f;
					}
					rayHitObject = raycastHit.collider.gameObject;
					rayHitVelocity = This_Rigidbody.velocity.magnitude;
					rayHitNormal = raycastHit.normal;
					isDetecting = false;
				}
			}
			else { // This script had detected an obstacle in the previous frame.
				// Set the position.
				This_Transform.position = nextPos;
				// Start the hit process.
				switch (Type) {
					case 0: // AP
						AP_Hit_Process(rayHitObject, rayHitVelocity, rayHitNormal);
						break;
					case 1: // HE
						HE_Hit_Process ();
						break;
				}
			}
		}


		void OnCollisionEnter (Collision collision)
		{ // The collision has been detected by the physics engine.
			if (isLiving) {
				// Start the hit process.
				switch (Type) {
					case 0: // AP
						AP_Hit_Process(collision.gameObject, collision.relativeVelocity.magnitude, collision.contacts [0].normal);
						break;
					case 1: // HE
						HE_Hit_Process ();
						break;
				}
			}
		}


		void AP_Hit_Process(GameObject hitObject, float hitVelocity, Vector3 hitNormal)
		{
			isLiving = false;

			if (hitObject == null) { // The hit object had been removed from the scene.
				return;
			}

			// Get the "Damage_Control_##_##_CS" script in the hit object.
			var damageScript = hitObject.GetComponent <Damage_Control_00_Base_CS>();
			if (damageScript != null) { // The hit object has "Damage_Control_##_##_CS" script. >> It should be a breakable object.
				// Calculate the hit damage.
				float hitAngle = Mathf.Abs(90.0f - Vector3.Angle(This_Rigidbody.velocity, hitNormal));
				float damageValue = Attack_Point * Mathf.Pow(hitVelocity / Initial_Velocity, 2.0f) * Mathf.Lerp(0.0f, 1.0f, Mathf.Sqrt(hitAngle / 90.0f)) * Attack_Multiplier;

				// Output for debugging.
				if (Debug_Flag) {
					float tempMultiplier = 1.0f;
					Damage_Control_09_Armor_Collider_CS armorColliderScript = hitObject.GetComponent <Damage_Control_09_Armor_Collider_CS>();
					if (armorColliderScript) {
						tempMultiplier = armorColliderScript.Damage_Multiplier;
					}
					Debug.Log("AP Damage " + damageValue * tempMultiplier + " on " + hitObject.name + " (" + (90.0f - hitAngle) + " degrees)");
				}

				// Send the damage value to "Damage_Control_##_##_CS" script.
				if (damageScript.Get_Damage(damageValue, Type) == true) { // The hit part has been destroyed.
					// Remove the bullet from the scene.
					Destroy(this.gameObject);
				}
				else { // The hit part has not been destroyed.
					// Create the ricochet object.
					if (Ricochet_Object) {
						Instantiate(Ricochet_Object, This_Transform.position, Quaternion.identity);
					}
				}

			}
			else { // The hit object does not have "Damage_Control_##_##_CS" script. >> It should not be a breakable object.
				// Create the impact object.
				if (Impact_Object) {
					Instantiate(Impact_Object, This_Transform.position, Quaternion.identity);
				}
			}
		}


		void HE_Hit_Process ()
		{
			isLiving = false;

			// Create the explosion effect object.
			if (Explosion_Object) {
				Instantiate (Explosion_Object, This_Transform.position, Quaternion.identity);
			}

			// Remove the useless components.
			Destroy (GetComponent <Renderer>());
			Destroy (GetComponent <Rigidbody> ());
			Destroy (GetComponent <Collider>());

			// Add the explosion force to the objects within the explosion radius.
			Explosion_Force *= Attack_Multiplier;
			Collider[] colliders = Physics.OverlapSphere (This_Transform.position, Explosion_Radius);
			for (int i = 0; i < colliders.Length; i++) {
				Add_Explosion_Force (colliders [i]);
			}
			Destroy (this.gameObject, 0.01f * Explosion_Radius);
		}


		void Add_Explosion_Force (Collider collider)
		{
			if (collider == null) {
				return;
			}

			Vector3 direction = (collider.transform.position - This_Transform.position).normalized;
			ray.origin = This_Rigidbody.position;
			ray.direction = direction;
			RaycastHit raycastHit;
			if (Physics.Raycast (ray, out raycastHit, Explosion_Radius, layerMask)) {
				if (raycastHit.collider != collider) { // The collider should be behind an obstacle.
					return;
				}

				// Calculate the distance loss rate.
				float loss = Mathf.Pow ((Explosion_Radius - raycastHit.distance) / Explosion_Radius, 2);

				// Add force to the rigidbody.
				Rigidbody rigidbody = collider.GetComponent <Rigidbody>();
				if (rigidbody) {
					rigidbody.AddForce (direction * Explosion_Force * loss);
				}

				// Send the damage value to "Damage_Control_##_##_CS" script in the collider.
				var damageScript = collider.GetComponent <Damage_Control_00_Base_CS>();
				if (damageScript != null) { // The collider should be a breakable object.
					float damageValue = Attack_Point * loss;
					damageScript.Get_Damage (damageValue, Type);
					// Output for debugging.
					if (Debug_Flag) {
						Debug.Log ("HE Damage " + damageValue + " on " + collider.name);
					}
				}
			}
		}

	}

}
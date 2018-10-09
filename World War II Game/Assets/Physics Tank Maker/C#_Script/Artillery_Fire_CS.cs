using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Artillery_Fire_CS : MonoBehaviour
	{
		/*
		 * This script creates the explosive shells above the target when the 'Artillery Fire' event occurs.
		 * This script works in combination with "Event_Event_05_Artillery_Fire_CS" script in the Event Controller object in the scene.
		*/


		// User options >>
		public float Interval_Min = 0.1f;
		public float Interval_Max = 0.5f;
		public float Radius = 256.0f;
		public float Height = 64.0f;
		public float Mass = 20.0f;
		public float Life_Time = 10.0f;
		public float Attack_Point = 100.0f;
		public float Explosion_Force = 100000.0f;
		public float Explosion_Radius = 32.0f;
		public GameObject Explosion_Object;
		// << User options


		bool isFiring = false;


		public void Fire (Transform targetTransform, int num)
		{ // Called from "Event_Event_05_Artillery_Fire_CS" script.
			if (isFiring) {
				return;
			}

			isFiring = true;

			// Set the target position.
			Rigidbody targetRigidbody = targetTransform.GetComponentInChildren <Rigidbody>();
			if (targetRigidbody) {
				targetTransform = targetRigidbody.transform;
			}
			StartCoroutine (Create_Shells (targetTransform, num));
		}


		IEnumerator Create_Shells (Transform targetTransform, int totalShellCount)
		{
			int shellCount = 0;
			float timeCount = 0.0f;
			float currentInterval = Random.Range (Interval_Min, Interval_Max);
			Vector3 initialPos = targetTransform.position;

			while (shellCount <= totalShellCount) {
				timeCount += Time.deltaTime;
				if (timeCount < currentInterval) {
					yield return null;
					continue;
				}

				// Create the gameobject.
				GameObject shellObject = new GameObject ("Artillery_Shell");

				// Set position.
				Vector3 targetPos;
				if (targetTransform) { // The target exists in the scene.
					targetPos = targetTransform.position;
				}
				else { // The target mighe be removed from the scene.
					targetPos = initialPos;
				}
				targetPos.x += Random.Range(0.0f, Radius) * Mathf.Cos(Random.Range(0.0f, 2.0f * Mathf.PI));
				targetPos.z += Random.Range (0.0f, Radius) * Mathf.Sin (Random.Range (0.0f, 2.0f * Mathf.PI));
				targetPos.y += Height;
				shellObject.transform.position = targetPos;

				// Add component.
				Rigidbody rigidbody = shellObject.AddComponent <Rigidbody>();
				rigidbody.mass = Mass;
				shellObject.AddComponent <SphereCollider>();

				// Add Scripts
				Bullet_Control_CS bulletScript = shellObject.AddComponent <Bullet_Control_CS>();
				bulletScript.Type = 1; // HE
				bulletScript.Life_Time = Life_Time;
				bulletScript.Attack_Point = Attack_Point;
				bulletScript.Explosion_Force = Explosion_Force;
				bulletScript.Explosion_Radius = Explosion_Radius;
				bulletScript.Explosion_Object = Explosion_Object;

				shellCount += 1;
				currentInterval = Random.Range (Interval_Min, Interval_Max);
				timeCount = 0.0f;
			}

			isFiring = false;
		}

	}

}
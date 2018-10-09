using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
	
	public class AI_Hand_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "AI_Hand" placed under the "AI_Core" in the AI tank.
		 * This script provides auto-brake function to the AI tank.
		 * This script works in combination with "AI_CS" in the parent object.
		*/

		public bool Is_Working; // Referred to from "AI_CS".
		bool isTouching;
		float touchCount;
		Collider touchCollider;
		AI_CS aiScript;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			aiScript = GetComponentInParent <AI_CS>();
			gameObject.layer = 2; // "Ignore Raycast" layer.
			Is_Working = false;

			// Make this gameobject invisible.
			Renderer renderer = GetComponent <Renderer>();
			if (renderer) {
				renderer.enabled = false;
			}

			// Make this collider a trigger.
			Collider collider = GetComponent <Collider>();
			if (collider) {
				collider.isTrigger = true;
			}
		}


		void Update ()
		{
			Auto_Brake();
		}


		void Auto_Brake()
		{
			if (Is_Working == false) {
				return;
			}

			if (isTouching) { // The hand is touching an obstacle now.
				if (touchCollider == null) { // The collider might have been removed from the scene.
					isTouching = false;
					return;
				}

				touchCount += Time.deltaTime;
				if (touchCount > aiScript.Stuck_Count) {
					touchCount = 0.0f;
					// Call the "AI_CS" to escape from a stuck.
					aiScript.Escape_From_Stuck ();
				}
				return;

			}
			else { // The hand has been away form the obstacle.
				touchCount -= Time.deltaTime;
				if (touchCount < 0.0f) {
					touchCount = 0.0f;
					Is_Working = false;
				}
			}
		}


		void OnTriggerStay (Collider collider)
		{ // Called when the hand touches an obstacle.
			if (isTouching == false && collider.attachedRigidbody) { // The hand is not touching an obstacle until now, and the collider has a rigidbody.
				Is_Working = true;
				isTouching = true;
				touchCollider = collider;
			}
		}


		void OnTriggerExit ()
		{ // Called when the hand has been away form the obstacle.
			isTouching = false;
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{
	
	public class Respawn_Controller_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the top object of the tank.
		 * This script is used for respawning the tank in the runtime.
		 * The player can respawn the tank manually, also the tank is automatically respawned when the tank is destroyed.
		*/

		// User options >>
		public GameObject This_Prefab;
		public int Respawn_Times = 0;
		public float Auto_Respawn_Interval = 10.0f;
		public bool Remove_After_Death = false;
		public float Remove_Interval = 30.0f;
		// << User options


		bool isAutoRespawning;

		bool isSelected = true;


		void Update()
		{
			if (isSelected == false) {
				return;
			}
			Manual_Respawn();
		}


		void Manual_Respawn()
		{
			// Respawn the tank Manually.
			if (Input.GetKeyDown(KeyCode.Return)) {
				if (Respawn_Times > 0 && isAutoRespawning == false) {
					Respawn();
				}
			}
		}


		void Respawn()
		{
			// Make sure that "This_Prefab" is assigned.
			if (This_Prefab == null) {
				Debug.LogError("'The prefab for respawning is not assigned.");
				return;
			}

			// Reduce "ReSpawn_Times".
			Respawn_Times -= 1;

			// Destroy all the children.
			//(Note.) This GameObject is continuously used after the tank is respawned.
			int childCount = this.transform.childCount;
			for (int i = 0; i < childCount; i++) {
				DestroyImmediate (this.transform.GetChild (0).gameObject);
			}

			// Instantiate the prefab.
			GameObject newTankObject = Instantiate(This_Prefab, this.transform.position, this.transform.rotation) as GameObject;

			// Make all the new child objects this children.
			childCount = newTankObject.transform.childCount;
			for (int i = 0; i < childCount; i++) {
				newTankObject.transform.GetChild (0).parent = this.transform;
			}

			// Destroy the new parent object that has no child any longer.
			DestroyImmediate (newTankObject);

			// Set the tag.
			transform.root.tag = "Untagged";

			// Send "Respawned" message to components in this GameObject ("ID_Settings_CS", "AI_Headquaters_Helper_CS", "Special_Settings_CS").
			this.gameObject.SendMessage ("Respawned", SendMessageOptions.DontRequireReceiver);
		}


		IEnumerator Auto_Respawn()
		{
			isAutoRespawning = true; // To cancel the manual respawning.

			// Wait.
			yield return new WaitForSeconds (Auto_Respawn_Interval);
			isAutoRespawning = false;

			// Start respawning.
			Respawn();
		}


		public IEnumerator Remove_Tank (float interval)
		{ // This function is called when the tank has been completely destroyed, also from "Event_Event_04_Remove_Tank_CS" script.
			if (isSelected) { // This tank is selected now.
				yield break;
			}

			// Wait.
			yield return new WaitForSeconds (interval);

			// Send "Prepare_Removing" message to components in this GameObject ("ID_Settings_CS", "AI_Headquaters_Helper_CS", "UI_PosMarker_Control_CS").
			this.gameObject.SendMessage ("Prepare_Removing", SendMessageOptions.DontRequireReceiver);

			// Destroy this tank from the root.
			Destroy (transform.root.gameObject);
		}


		void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			// Automatically respawn or remove the tank.
			if (Respawn_Times > 0) { // "ReSpawn_Times" remains.
				// Start auto respawning.
				StartCoroutine ("Auto_Respawn");
			}
			else { // "ReSpawn_Times" does not remain.
				if (Remove_After_Death) {
					// Start removing the tank.
					StartCoroutine("Remove_Tank", Remove_Interval);
				}
			}
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			this.isSelected = isSelected;
		}


		void Pause(bool isPaused)
		{ // Called from "Game_Controller".
			this.enabled = !isPaused;
		}

	}

}
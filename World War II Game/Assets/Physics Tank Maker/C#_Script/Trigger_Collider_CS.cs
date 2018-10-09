using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChobiAssets.PTM
{

	public class Trigger_Collider_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Trigger_Collider" object used for the events in the scene.
		 * This script works in combination with "Event_Trigger_03_##_TriggerCollider_##_CS" scripts.
		*/


		// User options >>
		public bool Invisible_Flag = true;
		public int Store_Count = 16;
		// << User options


		List <Event_Trigger_03_00_TriggerCollider_Base_CS> triggerScriptsList = new List <Event_Trigger_03_00_TriggerCollider_Base_CS>();
		List <GameObject> detectedObjectsList = new List <GameObject>();


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			// Set the Layer.
			this.gameObject.layer = 2; // Ignore Raycast.

			// Set "isTrigger" to "true" in all the colliders.
			Collider[] colliders = GetComponents <Collider>();
			for (int i = 0; i < colliders.Length; i++) {
				colliders[i].isTrigger = true;
			}

			// Make the mesh invisible.
			if (Invisible_Flag) {
				MeshRenderer meshRenderer = GetComponent <MeshRenderer>();
				if (meshRenderer) {
					meshRenderer.enabled = false;
				}
			}
		}


		void OnTriggerEnter(Collider collider)
		{
			Transform rootTransform = collider.transform.root;
			if (rootTransform.tag == "Finish") { // The tank should have been destroyed.
				return;
			}

			GameObject detectedObject = collider.gameObject;
			if (detectedObject.layer == 11 && Check_DetectedObjects(detectedObject)) { // MainBody && This is the first time to be detected. 
				// Send the root transform of the collider to "Event_Trigger_03_##_TriggerCollider_##_CS" scripts.
				for (int i = 0; i < triggerScriptsList.Count; i++) {
					if (triggerScriptsList[i] == null) {
						triggerScriptsList.RemoveAt(i);
						continue;
					}
					triggerScriptsList[i].Detect_Collider(collider.transform.root);
				}
			}
		}


		bool Check_DetectedObjects(GameObject detectedObject)
		{
			// Check the "detectedObject" had already been detected or not.
			GameObject newObject = detectedObjectsList.Find(delegate ( GameObject tempObject)
				{
					return tempObject == detectedObject;
				});
			if (newObject == null) { // The "detectedObject" had not been detected yet.
				// Add the "detectedObject" to the list.
				detectedObjectsList.Add(detectedObject);
				if (detectedObjectsList.Count > Store_Count) { // The size of the list has been over.
					// Remove the first element from the list.
					detectedObjectsList.RemoveAt(0);
				}
				return true;
			} // The "detectedObject" had already been detected.
			return false;
		}


		public void Get_Trigger_Script(Event_Trigger_03_00_TriggerCollider_Base_CS triggerScript)
		{ // Called from "Event_Trigger_03_##_TriggerCollider_##_CS".
			triggerScriptsList.Add(triggerScript);
		}

	}

}
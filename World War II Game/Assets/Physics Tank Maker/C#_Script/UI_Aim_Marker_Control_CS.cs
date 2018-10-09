using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace ChobiAssets.PTM
{
	
	[DefaultExecutionOrder (+1)] // (Note.) This script is executed after other scripts, in order to move the marker smoothly.
	public class UI_Aim_Marker_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "MainBody" of the tank with "Aiming_Control_CS".
		 * This script controls the aiming marker in the scene.
		 * The appearance and position of the aiming marker are controlled by this script.
		 * This script works in combination with the "Aiming_Control_CS".
		*/

		// User options >>
		public string Aim_Marker_Name = "Aim_Marker";
		// << User options


		Aiming_Control_CS aimingScript;
		Image markerImage;
		Transform markerTransform;

		bool isSelected;


		void Start()
		{
			// Get the marker image in the scene.
			if (string.IsNullOrEmpty(Aim_Marker_Name)) {
				return;
			}
			GameObject markerObject = GameObject.Find(Aim_Marker_Name);
			if (markerObject) {
				markerImage = markerObject.GetComponent <Image>();
			}
			else {
				// The aiming marker cannot be found in the scene.
				Debug.LogWarning(Aim_Marker_Name + " cannot be found in the scene.");
				Destroy(this);
				return;
			}
			markerTransform = markerImage.transform;

			// Get the "Aiming_Control_CS" in the tank.
			aimingScript = GetComponent <Aiming_Control_CS>();
			if (aimingScript == null) {
				Debug.LogWarning("'Aiming_Control_CS' cannot be found in the MainBody.");
				Destroy(this);
			}
		}


		void LateUpdate ()
		{
			if (isSelected == false) {
				return;
			}

			Marker_Control();
		}


		void Marker_Control ()
		{
			// Set the appearance.
			switch (aimingScript.Mode) {
				case 0: // Keep the initial positon.
					markerImage.enabled = false;
					return;
				case 1: // Free aiming.
				case 2: // Locking on.
					markerImage.enabled = true;
					if (aimingScript.Target_Transform) {
						markerImage.color = Color.red;
					} else {
						markerImage.color = Color.white;
					}
					break;
			}

			// Set the position.
			Vector3 currentPosition = Camera.main.WorldToScreenPoint (aimingScript.Target_Position);
			if (currentPosition.z < 0.0f) { // Behind of the camera.
				markerImage.enabled = false;
			}
			else {
				currentPosition.z = 128.0f;
			}
			markerTransform.position = currentPosition;
		}

	
		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			if (isSelected) {
				this.isSelected = true;
			}
			else {
				if (this.isSelected) { // This tank is selected until now.
					this.isSelected = false;
					markerImage.enabled = false;
				}
			}
		}


		void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			// Turn off the marker.
			if (isSelected) {
				markerImage.enabled = false;
			}

			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}

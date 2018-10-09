using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	[ RequireComponent (typeof(Camera))]
	[ RequireComponent (typeof(AudioListener))]

	public class Gun_Camera_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Gun_Camera" under the "Barrel_Base" in the tank.
		 * This script controls the gun camera used for aiming the target.
		 * The main camera and the gun camera are switched by this script.
		*/


		// User options >>
		public Camera Gun_Camera;
		public Camera Main_Camera;
		public float Min_FOV = 0.1f;
		public float Max_FOV = 50.0f;
		// << User options


		// Set by "Input_Type_Settings_CS".
		public int Input_Type = 0;

		//Set by "Gun_Camera_Input_##_##_CS" scripts.
		public float Zoom_Input; 

		float targetFOV;
		float currentFOV;
		AudioListener thisListener;
		AudioListener mainCameraListener;

		Gun_Camera_Input_00_Base_CS inputScript;

		bool isSelected;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			// Get the gun camera.
			if (Gun_Camera == null) {
				Gun_Camera = GetComponent <Camera>();
			}
			Gun_Camera.rect = new Rect (0.0f, 0.0f, 1.0f, 1.0f);
			Gun_Camera.enabled = false;
			currentFOV = Gun_Camera.fieldOfView;
			targetFOV = currentFOV;

			// Get the AudioListener.
			thisListener = GetComponent < AudioListener > ();
			if (thisListener == null) {
				thisListener = gameObject.AddComponent <AudioListener>();
			}
			thisListener.enabled = false;
			this.tag = "Untagged";

			// Get the main camera.
			if (Main_Camera == null) {
				Camera[] tankCameras = transform.root.GetComponentsInChildren <Camera>();
				for (int i = 0; i < tankCameras.Length; i++) {
					if (tankCameras[i] != Gun_Camera) {
						Main_Camera = tankCameras[i];
						break;
					}
				}
			}
			if (Main_Camera == null) {
				Debug.LogWarning("'Gun_Camera_CS' cannot find the main camera.");
				Destroy(this.gameObject);
				return;
			}

			// Get the main camera's AudioListener.
			mainCameraListener = Main_Camera.GetComponent <AudioListener>();
			if (mainCameraListener == null) {
				mainCameraListener = Main_Camera.gameObject.AddComponent <AudioListener>();
			}

			// Set the input script.
			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
				case 2: // Mouse + Keyboard (Legacy)
				case 10: // AI.
					inputScript = gameObject.AddComponent <Gun_Camera_Input_01_Mouse_CS>();
					break;
				case 3: // Gamepad (Single stick)
					inputScript = gameObject.AddComponent <Gun_Camera_Input_02_For_Single_Stick_Drive_CS>();
					break;
				case 4: // Gamepad (Twin sticks)
					inputScript = gameObject.AddComponent <Gun_Camera_Input_03_For_Twin_Stick_Drive_CS>();
					break;
				case 5: // Gamepad (Triggers)
					inputScript = gameObject.AddComponent <Gun_Camera_Input_04_For_Triggers_Drive_CS>();
					break;
			}

			// Prepare the input script.
			if (inputScript != null) {
				inputScript.Prepare(this);
			}
		}


		void Update ()
		{
			if (isSelected == false) {
				return;
			}

			inputScript.Get_Input();

			if (Gun_Camera.enabled) {
				Zoom();
			}
		}


		public void Switch_Mode (int mode)
		{
			switch (mode) {
				case 0: // Not selected.
					// Disable all the cameras.
					Main_Camera.enabled = false;
					mainCameraListener.enabled = false;
					Gun_Camera.enabled = false;
					thisListener.enabled = false;
					this.tag = "Untagged";
					break;
				case 1: // Off
					// Enable the "Main_Camera".
					Main_Camera.enabled = true;
					mainCameraListener.enabled = true;
					// Disable the "Gun_Camera".
					Gun_Camera.enabled = false;
					thisListener.enabled = false;
					this.tag = "Untagged";
					break;
				case 2: // On
					// Disable the "Main_Camera".
					Main_Camera.enabled = false;
					mainCameraListener.enabled = false;
					// Enable the "Gun_Camera".
					Gun_Camera.enabled = true;
					thisListener.enabled = true;
					this.tag = "MainCamera";
					break;
			}
		}


		void Zoom ()
		{
			targetFOV *= 1.0f + Zoom_Input;
			targetFOV = Mathf.Clamp (targetFOV, Min_FOV, Max_FOV);

			if (currentFOV != targetFOV) {
				currentFOV = Mathf.MoveTowards (currentFOV, targetFOV, Mathf.Lerp (0.4f, 4.0f, currentFOV /Max_FOV));
				Gun_Camera.fieldOfView = currentFOV;
			}
		}
			

		void Get_Input_Type(int inputType)
		{ // Called from "Input_Settings_CS".
			Input_Type = inputType;
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			if (isSelected) {
				this.isSelected = true;
			}
			else {
				if (this.isSelected) { // This tank is selected until now.
					this.isSelected = false;
					Switch_Mode (0); // Not selected.
				}
			}
		}


		void Turret_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			// Turn off the gun camera.
			if (isSelected) {
				Switch_Mode (1); // Off
			}

			Destroy (this.gameObject);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
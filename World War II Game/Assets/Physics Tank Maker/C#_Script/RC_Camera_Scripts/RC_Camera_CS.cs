using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	[ RequireComponent (typeof(Camera))]
	[ RequireComponent (typeof(AudioListener))]

	public class RC_Camera_CS : MonoBehaviour
	{
		/*
		 * This script controls the RC camera in the scene.
		*/

		// User options >>
		public Camera RC_Camera;
		public float Horizontal_Speed = 1.0f;
		public float Vertical_Speed = 1.0f;
		public float Min_FOV = 1.0f;
		public float Max_FOV = 50.0f;
		public Transform Position_Pack;
		// << User options


		// Set by "Input_Type_Settings_CS".
		public int Input_Type = 0;

		// Set by "RC_Camera_Input_##_###_CS" scripts.
		public bool Use_Analog_Stick;
		public float Zoom_Input;
		public float Horizontal_Input;
		public float Vertical_Input;
		public bool Is_Turning;

		Transform thisTransform;
		AudioListener thisAudioListener;
		bool isSelected = true;
		Transform targetTransform;
		Camera_Points_Manager_CS targetCameraScript;
		Camera targetMainCamera;
		AudioListener targetMainAudioListener;
		Camera targetGunCamera;
		Transform[] cameraPositions;
		float targetFOV;
		float currentFOV;
		Vector3 currentAngles;

		RC_Camera_Input_00_Base_CS inputScript;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			thisTransform = transform;
			if (RC_Camera == null) {
				RC_Camera = GetComponent <Camera>();
			}
			thisAudioListener = GetComponent <AudioListener>();
			currentFOV = RC_Camera.fieldOfView;
			targetFOV = currentFOV;

			this.tag = "Untagged";
			RC_Camera.enabled = false;
			thisAudioListener.enabled = false;

			if (Position_Pack) {
				Set_Positions();
			}

			// Set the input script.
			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
				case 2: // Mouse + Keyboard (Legacy)
					inputScript = gameObject.AddComponent <RC_Camera_Input_01_Mouse_CS>();
					break;
				case 3: // Gamepad (Single stick)
					inputScript = gameObject.AddComponent <RC_Camera_Input_02_For_Single_Stick_Drive_CS>();
					break;
				case 4: // Gamepad (Twin sticks)
					inputScript = gameObject.AddComponent <RC_Camera_Input_03_For_Twin_Sticks_Drive_CS>();
					break;
				case 5: // Gamepad (Triggers)
					inputScript = gameObject.AddComponent <RC_Camera_Input_04_For_Triggers_Drive_CS>();
					break;
			}

			// Prepare the input script.
			if (inputScript != null) {
				inputScript.Prepare(this);
			}
		}


		void Set_Positions ()
		{
			if (Position_Pack.childCount != 0) { // The "Position_Pack" is not empty.
				cameraPositions = new Transform[Position_Pack.childCount];
				for (int i = 0; i < Position_Pack.childCount; i++) {
					cameraPositions[i] = Position_Pack.GetChild (i);
				}
				// Move to the first position.
				thisTransform.position = cameraPositions[0].position;
			}
			else { // The "Position_Pack" is empty.
				// Set the initial position.
				cameraPositions = new Transform[1];
				GameObject newPositionObject = new GameObject("RC_Camera_Position (1)");
				newPositionObject.transform.position = thisTransform.position;
				cameraPositions[0] = newPositionObject.transform;
			}
		}


		void LateUpdate ()
		{
			if (targetCameraScript == null || targetCameraScript.Is_Selected == false) { // The target is removed from the scene, or has not been selected.
				if (Find_Target() == false) { // The target cannot be found.
					return;
				}
			}

			// Get the input values.
			inputScript.Get_Input();

			// Turn on / off the three cameras. (RC camera, tatget's main camera, tatget's gun camera)
			if (Control_Cameras_Enabling() == false) { // The RC camera is disabled.
				return;
			}

			// Control the camera position.
			if (Position_Pack) {
				Control_Position ();
			}

			// Zoom.
			Zoom();

			// Rotate.
			if (Is_Turning) {
				if (Use_Analog_Stick) {
					Rotate_With_Stick();
				}
				else {
					Rotate_With_Mouse();
				}
			}
			else {
				thisTransform.LookAt(targetTransform.position + targetTransform.up);
			}
		}


		public void Switch_RC_Camera()
		{
			isSelected = !isSelected;
		}


		bool Control_Cameras_Enabling()
		{
			if (isSelected == true) { // RC camera is selected now.
				// Disable the target's main camera.
				targetMainCamera.enabled = false;
				targetMainAudioListener.enabled = false;
			
				// Check the target's gun camera.
				if (targetGunCamera && targetGunCamera.enabled == true) { // The target is using the gun camera.
					// Disable RC camera.
					this.tag = "Untagged";
					RC_Camera.enabled = false;
					thisAudioListener.enabled = false;
					return false;
				}
				else { // The target is not using the gun camera.
					// Enable RC camera.
					this.tag = "MainCamera";
					RC_Camera.enabled = true;
					thisAudioListener.enabled = true;
					return true;
				}
			}
			else { // RC camera is not selected now.
				// Disable RC camera.
				this.tag = "Untagged";
				RC_Camera.enabled = false;
				thisAudioListener.enabled = false;
			
				// Check the target's gun camera.
				if (targetGunCamera && targetGunCamera.enabled == true) { // The target is using the gun camera.
					// Disable the target's main camera.
					targetMainCamera.enabled = false;
					targetMainAudioListener.enabled = false;
				}
				else { // The target is not using the gun camera.
					// Enable the target's main camera.
					targetMainCamera.enabled = true;
					targetMainAudioListener.enabled = true;
				}
				return false;
			}
		}


		bool Find_Target()
		{
			// Find "Camera_Points_Manager_CS" in the current selected tank.
			Camera_Points_Manager_CS[] cameraScripts = FindObjectsOfType <Camera_Points_Manager_CS>();
			for (int i = 0; i < cameraScripts.Length; i++) {
				if (cameraScripts[i].Is_Selected) {
					targetCameraScript = cameraScripts[i];
					break;
				}
			}
			if (targetCameraScript == null) {
				return false;
			}

			// Get the main camera components in the target.
			targetMainCamera = targetCameraScript.Main_Camera;
			targetMainAudioListener = targetCameraScript.Main_AudioListener;
			targetTransform = targetCameraScript.GetComponentInParent <Rigidbody>().transform;

			// Get the gun camera in the target.
			Camera[] targetCameras = targetTransform.GetComponentsInChildren <Camera>();
			for (int i = 0; i < targetCameras.Length; i++) {
				if (targetCameras[i] != targetMainCamera) {
					targetGunCamera = targetCameras[i];
					break;
				}
			}
			return true;
		}


		void Control_Position ()
		{
			float shortestDistance = Mathf.Infinity;
			int tempIndex = 0;
			for (int i = 0; i < cameraPositions.Length; i++) {
				float tempDist = Vector3.Distance (targetTransform.position, cameraPositions[i].position);
				if (tempDist < shortestDistance) {
					shortestDistance = tempDist;
					tempIndex = i;
				}
			}
			thisTransform.position = cameraPositions[tempIndex].position;
		}


		void Trigger_Input ()
		{
			if (Input.GetButton ("Fire1") && Input.GetAxis ("Vertical_Input") > 0) {
				targetFOV *= 0.975f;
				targetFOV = Mathf.Clamp (targetFOV, Min_FOV, Max_FOV);
			} else if (Input.GetButton ("Fire1") && Input.GetAxis ("Vertical_Input") < 0) {
				targetFOV *= 1.025f;
				targetFOV = Mathf.Clamp (targetFOV, Min_FOV, Max_FOV);
			}
			// Turn
			float multiplier = Mathf.Lerp (0.01f, 0.5f, currentFOV /Max_FOV);
			Horizontal_Input = Input.GetAxis ("Horizontal_Input2") * multiplier;
			Vertical_Input = Input.GetAxis ("Vertical_Input2") * multiplier;
			if (Horizontal_Input == 0.0f && Vertical_Input == 0.0f) {
				Is_Turning = false;
			} else {
				Is_Turning = true;
				//Stick_Rotate ();
			}
		}


		void Zoom()
		{
			// Set the target FOV.
			targetFOV *= 1.0f + Zoom_Input;
			targetFOV = Mathf.Clamp (targetFOV, Min_FOV, Max_FOV);

			// Set the RC camera's FOV.
			if (currentFOV != targetFOV) {
				currentFOV = Mathf.MoveTowards (currentFOV, targetFOV, Mathf.Lerp (0.2f, 2.0f, currentFOV /Max_FOV));
				currentFOV = Mathf.Clamp (currentFOV, Min_FOV, Max_FOV);
				RC_Camera.fieldOfView = currentFOV;
			}
		}


		void Rotate_With_Mouse ()
		{
			currentAngles = thisTransform.localEulerAngles;
			currentAngles.x -= Vertical_Input * Vertical_Speed;
			currentAngles.y += Horizontal_Input * Horizontal_Speed;
			thisTransform.eulerAngles = currentAngles;
		}


		void Rotate_With_Stick ()
		{
			thisTransform.LookAt (targetTransform.position + targetTransform.up);
			currentAngles = thisTransform.eulerAngles;
			currentAngles.x -= Vertical_Input * 90.0f;
			currentAngles.y += Horizontal_Input * 180.0f;
			currentAngles.z = 0.0f;
			thisTransform.eulerAngles = currentAngles;
		}


		void Get_Input_Type(int inputType)
		{ // Called from "Input_Settings_CS".
			Input_Type = inputType;
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}
	}

}
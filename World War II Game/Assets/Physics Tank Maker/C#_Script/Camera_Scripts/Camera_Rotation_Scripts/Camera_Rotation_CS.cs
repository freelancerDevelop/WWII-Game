using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Camera_Rotation_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Camera_Pivot" in the tank.
		 * This script rotate the camera pivot in two ways, first person view and third person view.
		*/


		// User options >>
		public Camera Main_Camera;
		public float Horizontal_Speed = 3.0f;
		public float Vertical_Speed = 2.0f;
		public bool Invert_Flag = false;
		public bool Use_Demo_Camera = false;
		// << User options


		// Set by "Input_Type_Settings_CS".
		public int Input_Type = 0;

		// Set by "Camera_Points_Manager_CS".
		public int Rotation_Type = 1; // 0 = Not allowed, 1 = Third person, 2 = First person.
		public bool Enable_Auto_Look = true;

		// Set by "Camera_Rotation_Input_##_##_CS" scripts.
		public float Horizontal_Input;
		public float Vertical_Input;

		Transform thisTransform;
		Vector3 targetAngles;
		Vector3 currentAngles;
		int invertNum;

		Camera_Rotation_Input_00_Base_CS inputScript;


		void Awake()
		{ // (Note.) The "thisTransform" must be prepared before "Start()", because the "Change_Camera_Settings()" function is called at Start().
			thisTransform = transform;
		}


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			currentAngles = thisTransform.eulerAngles;
			targetAngles.y = currentAngles.y;
			targetAngles.z = currentAngles.z;
			if (Invert_Flag) {
				invertNum = -1;
			} else {
				invertNum = 1;
			}

			// Get the main camera.
			if (Main_Camera == null) {
				Main_Camera = GetComponentInChildren <Camera>();
			}
			if (Main_Camera == null) {
				Debug.LogError("'Camera_Rotation_CS' cannot find the main camera.");
				Destroy(this.gameObject);
				return;
			}

			// Set the input script.
			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
				case 10: // AI.
					inputScript = gameObject.AddComponent <Camera_Rotation_Input_01_Mouse_CS>();
					break;
				case 2: // Mouse + Keyboard (Legacy)
					inputScript = gameObject.AddComponent <Camera_Rotation_Input_02_Mouse_Drag_CS>();
					break;
				case 3: // Gamepad (Single stick)
					inputScript = gameObject.AddComponent <Camera_Rotation_Input_03_For_Single_Stick_Drive_CS>();
					break;
				case 4: // Gamepad (Twin sticks)
					inputScript = gameObject.AddComponent <Camera_Rotation_Input_04_For_Twin_Sticks_Drive_CS>();
					break;
				case 5: // Gamepad (Triggers)
					inputScript = gameObject.AddComponent <Camera_Rotation_Input_05_For_Triggers_Drive_CS>();
					break;
				case 99 : // Demo.
					inputScript = gameObject.AddComponent <Camera_Rotation_Input_99_Demo_CS>();
					break;
			}

			// Prepare the input script.
			if (inputScript != null) {
				inputScript.Prepare(this);
			}
		}


		void Update ()
		{
			if (Main_Camera.enabled) {
				
				inputScript.Get_Input();

				switch (Rotation_Type) {
					case 0: // No rotation.
						break;
					case 1: // Third person view.
						Rotate_TPV();
						break;
					case 2: // First person view.
						Rotate_FPV();
						break;
				}
			}
		}


		void Rotate_TPV ()
		{
			// Rotate.
			targetAngles.y += Horizontal_Input * Horizontal_Speed;
			targetAngles.z -= Vertical_Input * Vertical_Speed * invertNum;
			targetAngles.z = Mathf.Clamp(targetAngles.z, -10.0f, 90.0f);
			if (currentAngles.y != targetAngles.y) {
				currentAngles.y = Mathf.MoveTowardsAngle (currentAngles.y, targetAngles.y, 1080.0f * Time.deltaTime);
			}
			if (currentAngles.z != targetAngles.z) {
				currentAngles.z = Mathf.MoveTowardsAngle (currentAngles.z, targetAngles.z, 1080.0f * Time.deltaTime);
			}
			thisTransform.eulerAngles = currentAngles;
		}


		void Rotate_FPV ()
		{
			Vector3 currentLocalAngles = thisTransform.localEulerAngles;

			// Horizontal rotation.
			targetAngles.y += Horizontal_Input * Horizontal_Speed;
			currentLocalAngles.y = Mathf.MoveTowardsAngle (currentLocalAngles.y, targetAngles.y, 1080.0f * Time.deltaTime);

			// Vertical and rolling rotation.
			if (Vertical_Input == 0.0f && Mathf.Abs (Mathf.DeltaAngle(0.0f, currentLocalAngles.z)) < 10.0f) { // The vertical input is zero, and the camera is almost horizotal.
				// Make the camera horizotal.
				Vector3 lookAtPos = thisTransform.InverseTransformPoint(thisTransform.position + Vector3.up);
				// X axis (Rolling rotation).
				float deltaAng = Mathf.Atan(lookAtPos.z / lookAtPos.y) * Mathf.Rad2Deg;
				currentLocalAngles.x = Mathf.MoveTowardsAngle(currentLocalAngles.x, deltaAng, Mathf.Abs(deltaAng) * Time.deltaTime);
				// Z aixs (Vertical rotation)
				deltaAng = -Mathf.Atan(lookAtPos.x / lookAtPos.y) * Mathf.Rad2Deg;
				targetAngles.z = Mathf.Clamp(currentLocalAngles.z + deltaAng, -10.0f, 10.0f);
				currentLocalAngles.z = Mathf.MoveTowardsAngle(currentLocalAngles.z, targetAngles.z, Mathf.Abs(deltaAng) * Time.deltaTime);
			}
			else { // The vertical input is not zero, and the camera is not horizotal.
				// X axis (Rolling rotation).
				currentLocalAngles.x = Mathf.MoveTowardsAngle(currentLocalAngles.x, 0.0f, 5.0f * Time.deltaTime);
				// Z aixs (Vertical rotation)
				targetAngles.z = Mathf.DeltaAngle(0.0f, currentLocalAngles.z) - (Vertical_Input * Vertical_Speed * invertNum);
				targetAngles.z = Mathf.Clamp(targetAngles.z, -60.0f, 60.0f);
				currentLocalAngles.z = Mathf.MoveTowardsAngle (currentLocalAngles.z, targetAngles.z, 1080.0f * Time.deltaTime);
			}

			thisTransform.localEulerAngles = currentLocalAngles;
		}


		public void Look_At_Target (Vector3 targetPos)
		{ // Called from "Aiming_Control_CS".
			if (Enable_Auto_Look == false) {
				return;
			}

			Vector3 localPos = thisTransform.InverseTransformPoint(targetPos);
			float deltaAngle = Mathf.Atan(localPos.z / localPos.x) * Mathf.Rad2Deg;
			if (localPos.x > 0.0f) {
				deltaAngle -= 180.0f;
			}
			switch (Rotation_Type) {
				case 0: // No rotation.
					break;
				case 1: // Third person view.
					targetAngles.y = currentAngles.y - deltaAngle;
					targetAngles.z = 15.0f * Mathf.Lerp(0.0f, 1.0f, Main_Camera.fieldOfView / 50.0f); // Change the angle according to the FOV of the main camera.

					break;
				case 2: // First person view.
					targetAngles.y = thisTransform.localEulerAngles.y - deltaAngle;
					targetAngles.z = 0.0f;
					break;
			}
		}


		public void Look_Forward ()
		{ // Called form "Camera_Rotation_Input_##_##_CS" scripts.
			float deltaAngle = 0.0f;
			Transform bodyTransform = thisTransform.GetComponentInParent <Rigidbody>().transform;
			Vector3 localPos = thisTransform.InverseTransformPoint(bodyTransform.position + bodyTransform.forward * 128.0f);
			deltaAngle = Mathf.Atan(localPos.z / localPos.x) * Mathf.Rad2Deg;
			if (localPos.x > 0.0f) {
				deltaAngle -= 180.0f;
			}

			switch (Rotation_Type) {
				case 0: // No rotation.
					break;
				case 1: // Third person view.
					targetAngles.y = currentAngles.y - deltaAngle;
					targetAngles.z = 10.0f;
					break;
				case 2: // First person view.
					targetAngles.y = thisTransform.localEulerAngles.y - deltaAngle;
					targetAngles.z = 0.0f;
					break;
			}
		}


		public void Change_Camera_Settings(CameraPoint cameraPointClass)
		{ // Called from "Camera_Points_Manager_CS".
			Rotation_Type = cameraPointClass.Rotation_Type;
			Enable_Auto_Look = cameraPointClass.Enable_Auto_Look;
			// Set the initial angles.
			switch (Rotation_Type) {
				case 0: // Not allowed.
					thisTransform.localEulerAngles = new Vector3 (0.0f, 90.0f, 0.0f);
					break;
				case 1: // Third Person.
					currentAngles = thisTransform.eulerAngles;
					currentAngles.x = 0.0f;
					currentAngles.z = Mathf.DeltaAngle(360.0f, currentAngles.z);
					targetAngles.y = currentAngles.y;
					targetAngles.z = currentAngles.z;
					break;
				case 2: // First Person.
					thisTransform.localEulerAngles = new Vector3 (0.0f, 90.0f, 0.0f);
					targetAngles.y = 90.0f;
					break;
			}
		}


		void Get_Input_Type(int inputType)
		{ // Called from "Input_Settings_CS".
			if (Use_Demo_Camera) {
				Input_Type = 99;
			}
			else {
				Input_Type = inputType;
			}
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
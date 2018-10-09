using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	[ System.Serializable]
	public class CameraPoint
	{
		public Transform Camera_Point;
		[Range (1.0f, 179.0f)] public float FOV = 30.0f;
		[Range (0.01f, 1.0f)] public float Clipping_Planes_Near = 0.3f;
		[Tooltip ("0 = Not allowed, 1 = Third person, 2 = First person"), Range (0, 2)] public int Rotation_Type = 1; // 0 = Not allowed, 1 = Third person, 2 = First person.
		public bool Enable_Zooming = true;
		[Tooltip ("Look at the target when locking-on")] public bool Enable_Auto_Look = true;
		public bool Enable_PopUp = true;
		public bool Enable_Avoid_Obstacle = true;
	}


	public class Camera_Points_Manager_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Camera_Pivot" in the tank.
		 * This script switches the camera points in the tank.
		 * You can attach camera points as much as you need, and can change the settings in each point.
		*/


		// User options >>
		public Camera Main_Camera;
		public AudioListener Main_AudioListener;
		public int Camera_Points_Num = 1;
		public CameraPoint[] Camera_Points;
		// << User options


		int index;
		Transform thisTransform;
		Transform cameraTransform;
		Vector3 cameraTPVLocalPos = new Vector3(15.0f, 0.0f, 0.0f);

		public bool Is_Selected; // Referred to from "RC_Camera_CS".


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			thisTransform = transform;

			// Get the main camera.
			if (Main_Camera == null) {
				Main_Camera = GetComponentInChildren <Camera>();
			}
			Main_Camera.tag = "MainCamera";
			cameraTransform = Main_Camera.transform;

			// Get the AudioListener.
			if (Main_AudioListener == null) {
				Main_AudioListener = GetComponentInChildren <AudioListener>();
			}

			// Switch the camera point at the first time.
			Switch_Camera_Point();
		}


		void Update()
		{
			if (Is_Selected == false || Main_Camera.enabled == false) {
				return;
			}

			if (Input.GetKeyDown(KeyCode.F)) { // || Input.GetButtonDown("Stick Press R")) {
				index += 1;
				if (index >= Camera_Points.Length) {
					index = 0;
				}
				Switch_Camera_Point();
			}
		}


		void Switch_Camera_Point()
		{
			// Set the pivot (this) position.
			thisTransform.parent = Camera_Points[index].Camera_Point;
			thisTransform.localPosition = Vector3.zero;

			// Set the camera's local position.
			switch (Camera_Points[index].Rotation_Type) {
				case 0: // Not allowed.
				case 2: // First Person.
					// Move to the camera pivot.
					cameraTransform.localPosition = Vector3.zero;
					break;
				case 1: // Third person.
					// Keep a distance from the pivot.
					cameraTransform.localPosition = cameraTPVLocalPos;
					break;
			}

			// Call the "Change_Camera_Settings" function in "Camera_###_CS" scripts in this object and the child object.
			BroadcastMessage ("Change_Camera_Settings", Camera_Points[index], SendMessageOptions.DontRequireReceiver);
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			this.Is_Selected = isSelected;
		
			// Turn on / off the main camera, and the AudioListener.
			Main_Camera.enabled = Is_Selected;
			Main_AudioListener.enabled = Is_Selected;
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}

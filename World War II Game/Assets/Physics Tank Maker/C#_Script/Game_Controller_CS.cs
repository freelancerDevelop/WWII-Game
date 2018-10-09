using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ChobiAssets.PTM
{
	
	public class Game_Controller_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Game_Controller" in the scene.
		 * This script controls the physics settings, the layers collision settings and the cursor state in the scene.
		 * Also the general functions such as quit and pause are controlled by this script.
		*/


		// User options >>
		public float Fixed_TimeStep = 0.02f;
		public float Sleep_Threshold = 0.5f;
		// << User options

		bool isPaused = false;
		bool storedCursorVisible;


		void Start ()
		{
			// Physics settings.
			Time.fixedDeltaTime = Fixed_TimeStep;
			Physics.sleepThreshold = Sleep_Threshold;

			// Layer settings.
			Layers_Collision_Settings ();
		}


		void Layers_Collision_Settings ()
		{
			/* 
			 * Layer Collision Settings.
			 * Layer 9 >> for wheels.
			 * Layer 10 >> for suspension arms and track reinforce objects.
			 * Layer 11 >> for MainBody.
			*/

			// Layer 9 (for Wheels) settings.
			for (int i = 0; i <= 11; i++) {
				Physics.IgnoreLayerCollision (9, i, false);
				Physics.IgnoreLayerCollision (11, i, false);
			}
			Physics.IgnoreLayerCollision (9, 9, true); // Wheels ignore each other.
			Physics.IgnoreLayerCollision (9, 11, true); // Wheels ignore MainBody.

			// Layer 10 (for suspension arms and track reinforce objects) settings.
			for (int i = 0; i <= 11; i++) {
				Physics.IgnoreLayerCollision (10, i, true); // Suspension arms and track reinforce objects ignore all.
			}
		}
			

		void Update ()
		{
			General_Functions();
		}

	
		void General_Functions ()
		{
			if (Input.anyKeyDown == false) {
				return;
			}

			// Reload the scene.
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				Time.timeScale = 1.0f;
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
				return;
			}

			// Quit.
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
				return;
			}

			// Cursor state.
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				Set_Cursor_State(Cursor.visible == false);
				return;
			}

			// Pause.
			if (Input.GetKeyDown (KeyCode.P)) {
				Pause ();
				return;
			}
		}


		void Set_Cursor_State(bool isVisible)
		{
			if (isVisible) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}


		void Pause ()
		{
			isPaused = !isPaused;

			// Set the time scale, and the cursor state.
			if (isPaused) {
				Time.timeScale = 0.0f;
				storedCursorVisible = Cursor.visible;
				Set_Cursor_State(true);
			}
			else {
				Time.timeScale = 1.0f;
				Set_Cursor_State(storedCursorVisible);
			}

			// Send Message to "Input_Type_Settings_CS" in the all tanks in the scene.
			Input_Type_Settings_CS [] inputScript = FindObjectsOfType <Input_Type_Settings_CS>();
			for (int i = 0; i < inputScript.Length; i++) {
				inputScript [i].BroadcastMessage ("Pause", isPaused, SendMessageOptions.DontRequireReceiver);
			}
		}

	}

}
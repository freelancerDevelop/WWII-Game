using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Cannon_Fire_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Cannon_Base" in the tank.
		 * This script controls the firining of the tank.
		 * When firing, this script calls "Bullet_Generator_CS" and "Recoil_Brake_CS" scripts placed under this object in the hierarchy.
		 * In case of AI tank, this script works in combination with "AI_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS" and "Aiming_Control_CS".
		*/

		// User options >>
		public float Reload_Time = 2.0f;
		public float Recoil_Force = 5000.0f;
		// << User options


		// Set by "Input_Type_Settings_CS".
		public int Input_Type = 0;

		// Referred to from "UI_Reloading_Circle_CS".
		public float Loading_Count;
		public bool Is_Loaded = true;

		Rigidbody bodyRigidbody;
		Transform thisTransform;
		int direction = 1; // For twin barrels, 1 = left, 2 = right.
		public Bullet_Generator_CS[] Bullet_Generator_Scripts; // Referred to from "Cannon_Fire_Input_99_AI_CS" script.
		Recoil_Brake_CS[] recoilScripts;

		Cannon_Fire_Input_00_Base_CS inputScript;

		bool isSelected;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{ // This function must be called in Start() after changing the hierarchy.
			thisTransform = transform;
			Bullet_Generator_Scripts = GetComponentsInChildren <Bullet_Generator_CS>();
			recoilScripts = thisTransform.parent.GetComponentsInChildren <Recoil_Brake_CS> ();
			bodyRigidbody = GetComponentInParent <Rigidbody>();

			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
				case 2: // Mouse + Keyboard (Legacy)
					inputScript = gameObject.AddComponent <Cannon_Fire_Input_01_Mouse_CS>();
					break;
				case 3: // GamePad (Single stick)
				case 4: // GamePad (Twin stick)
					inputScript = gameObject.AddComponent <Cannon_Fire_Input_02_For_Sticks_Drive_CS>();
					break;
				case 5: // GamePad (Triggers)
					inputScript = gameObject.AddComponent <Cannon_Fire_Input_03_For_Triggers_Drive_CS>();
					break;
				case 10: // AI
					inputScript = gameObject.AddComponent <Cannon_Fire_Input_99_AI_CS>();
					break;
			}

			// Prepare the "inputScript".
			if (inputScript != null) {
				inputScript.Prepare(this);
			}
		}


		void Update ()
		{
			if (Is_Loaded == false) {
				return;
			}

			if (isSelected || Input_Type == 10) { // The tank is selected, or AI.
				inputScript.Get_Input();
			}
		}
	

		public void Fire ()
		{
			// Call all the "Bullet_Generator_CS".
			for (int i = 0; i < Bullet_Generator_Scripts.Length; i++) {
				Bullet_Generator_Scripts [i].Fire_Linkage (direction);
			}

			// Call all the "Recoil_Brake_CS".
			for (int i = 0; i < recoilScripts.Length; i++) {
				recoilScripts [i].Fire_Linkage (direction);
			}

			// Add recoil shock force to the MainBody.
			bodyRigidbody.AddForceAtPosition (-thisTransform.forward * Recoil_Force, thisTransform.position, ForceMode.Impulse);

			// Reload.
			StartCoroutine ("Reload");
		}


		IEnumerator Reload ()
		{
			Is_Loaded = false;
			Loading_Count = 0.0f;

			while (Loading_Count < Reload_Time) {
				Loading_Count += Time.deltaTime;
				yield return null;
			}

			Is_Loaded = true;
			Loading_Count = Reload_Time;

			// Set direction for twin barrels, 1 = left, 2 = right.
			if (direction == 1) {
				direction = 2;
			} else {
				direction = 1;
			}
		}


		void Get_Input_Type(int inputType)
		{ // Called from "Input_Settings_CS".
			Input_Type = inputType;
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			this.isSelected = isSelected;
		}


		void Turret_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
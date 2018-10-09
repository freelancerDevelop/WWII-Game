using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Aiming_Control_Input_03_For_Single_Stick_Drive_CS : Aiming_Control_Input_01_Mouse_Keyboard_CS
	{
		
		Camera gunCamera;


		public override void Prepare(Aiming_Control_CS aimingScript)
		{
			this.aimingScript = aimingScript;
			bulletGeneratorScripts = GetComponentsInChildren <Bullet_Generator_CS>();

			// Set the "Use_Auto_Turn".
			aimingScript.Use_Auto_Turn = true;

			// Set the relationship.
			ID_Settings_CS idScript = GetComponentInParent <ID_Settings_CS>();
			if (idScript) {
				thisRelationship = idScript.Relationship;
			}

			// Get the gun camera.
			var gunCameraScript = GetComponentInChildren <Gun_Camera_CS>();
			if (gunCameraScript) {
				gunCamera = gunCameraScript.Gun_Camera;
			}
		}


		public override void Get_Input()
		{
			// Adjust aiming.
			if (Input.GetButton ("Bumper L")) {
				float multiplier = 1.0f;
				if (gunCamera) {
					multiplier = Mathf.Lerp(0.05f, 1.0f, gunCamera.fieldOfView / 10.0f); // Change the speed according to the FOV of the gun camera.
				}
				aimingScript.Adjust_Angle.x += Input.GetAxis ("Horizontal2") * aimingScript.Aiming_Sensibility * multiplier;
				aimingScript.Adjust_Angle.y += Input.GetAxis ("Vertical2") * aimingScript.Aiming_Sensibility * 0.5f * multiplier;
			} 
			else {
				aimingScript.Adjust_Angle = Vector3.zero;
			}

			// Free aiming.
			if (aimingScript.Mode == 1) { // Free aiming.
				if (Input.GetButton ("Bumper L") == false) { // Not aiming now.
					screenCenter.x = Screen.width * 0.5f;
					screenCenter.y = Screen.height * 0.7f;
					aimingScript.Cast_Ray_Sphere_Free(screenCenter);
				}
			}

			// Left lock on.
			if (Input.GetButtonDown ("Fire3")) {
				aimingScript.Auto_Lock(0, thisRelationship);
				return;
			}
			// Right lock on.
			if (Input.GetButtonDown ("Fire2")) {
				aimingScript.Auto_Lock(1, thisRelationship);
				return;
			}
			// Front lock on.
			if (Input.GetButtonDown ("Jump")) {
				aimingScript.Auto_Lock(2, thisRelationship);
				return;
			}
			// Lock off.
			if (Input.GetButtonDown ("Fire1")) {
				aimingScript.Mode = 0;
				aimingScript.Switch_Mode();
				return;
			}

			// Switch the aiming mode.
			if (Input.GetButtonDown ("Stick Press L")) {
				if (aimingScript.Mode == 1) { // Free aiming.
					aimingScript.Mode = 0; // Lock off.
				} else {
					aimingScript.Mode = 1; // Free aiming.
				}
				aimingScript.Switch_Mode ();
			}

			// Switch the bullet type.
			if (Input.GetButtonDown("Back")) {
				for (int i = 0; i < bulletGeneratorScripts.Length; i++) {
					if (bulletGeneratorScripts[i] == null) {
						continue;
					}
					bulletGeneratorScripts[i].Switch_Bullet_Type();
				}
			}
		}

	}

}

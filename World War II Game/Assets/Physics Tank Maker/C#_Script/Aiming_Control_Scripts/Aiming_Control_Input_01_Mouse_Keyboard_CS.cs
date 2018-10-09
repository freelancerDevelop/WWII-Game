using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Aiming_Control_Input_01_Mouse_Keyboard_CS : Aiming_Control_Input_00_Base_CS
	{

		protected int thisRelationship;
		protected Vector3 screenCenter = Vector3.zero;


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
		}


		public override void Get_Input()
		{
			// Adjust aiming.
			if (Input.GetKey (KeyCode.Space)) {
				aimingScript.Adjust_Angle.x += Input.GetAxis ("Mouse X") * aimingScript.Aiming_Sensibility;
				aimingScript.Adjust_Angle.y += Input.GetAxis ("Mouse Y") * aimingScript.Aiming_Sensibility;
			} 
			else {
				aimingScript.Adjust_Angle = Vector3.zero;
			}

			// Free aiming.
			if (aimingScript.Mode == 1) { // Free aiming.
				if (Input.GetKey (KeyCode.Space) == false) { // Not aiming now.
					screenCenter.x = Screen.width * 0.5f;
					screenCenter.y = Screen.height * 0.7f;
					aimingScript.Cast_Ray_Sphere_Free(screenCenter);
				}
			}

			// Left lock on.
			if (Input.GetKeyDown(KeyCode.Q)) {
				aimingScript.Auto_Lock(0, thisRelationship);
				return;
			}
			// Right lock on.
			if (Input.GetKeyDown(KeyCode.E)) {
				aimingScript.Auto_Lock(1, thisRelationship);
				return;
			}
			// Front lock on.
			if (Input.GetMouseButtonUp(1)) {
				aimingScript.Auto_Lock(2, thisRelationship);
				return;
			}
			// Lock off.
			if (Input.GetMouseButtonDown(2)) {
				aimingScript.Mode = 0;
				aimingScript.Switch_Mode();
				return;
			}

			// Switch the aiming mode.
			if (Input.GetKeyDown (KeyCode.R)) {
				if (aimingScript.Mode == 1) { // Free aiming.
					aimingScript.Mode = 0; // Lock off.
				} else {
					aimingScript.Mode = 1; // Free aiming.
				}
				aimingScript.Switch_Mode ();
			}

			// Switch the bullet type.
			if (Input.GetKeyDown(KeyCode.V)) {
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

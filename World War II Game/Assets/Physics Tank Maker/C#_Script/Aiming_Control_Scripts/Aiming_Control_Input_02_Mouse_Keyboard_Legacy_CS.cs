using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Aiming_Control_Input_02_Mouse_Keyboard_Legacy_CS : Aiming_Control_Input_01_Mouse_Keyboard_CS
	{

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
					aimingScript.Cast_Ray_Sphere_Free(Input.mousePosition);
				}
			}

			// Free lock on.
			if (Input.GetMouseButtonDown (2)) {
				aimingScript.Cast_Ray_Lock (Input.mousePosition);
				return;
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

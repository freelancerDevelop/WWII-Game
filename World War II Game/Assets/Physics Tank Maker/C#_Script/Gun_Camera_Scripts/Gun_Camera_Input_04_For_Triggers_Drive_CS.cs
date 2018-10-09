using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Gun_Camera_Input_04_For_Triggers_Drive_CS : Gun_Camera_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			// Turn on / off.
			if (Input.GetButtonDown("Stick Press L")) {
				if (gunCameraScript.Gun_Camera.enabled) {
					gunCameraScript.Switch_Mode(1); // Off
				}
				else {
					gunCameraScript.Switch_Mode(2); // On
				}
			}

			// Zoom.
			if (Input.GetButton("Jump")) {
				gunCameraScript.Zoom_Input = -0.05f;
			}
			else if (Input.GetButton("Fire2")) {
				gunCameraScript.Zoom_Input = 0.05f;
			}
			else {
				gunCameraScript.Zoom_Input = 0.0f;
			}
		}

	}

}

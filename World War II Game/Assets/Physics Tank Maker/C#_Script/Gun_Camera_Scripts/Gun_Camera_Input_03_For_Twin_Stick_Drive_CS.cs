using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Gun_Camera_Input_03_For_Twin_Stick_Drive_CS : Gun_Camera_Input_00_Base_CS
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
			gunCameraScript.Zoom_Input = (Input.GetAxis("Trigger L") - Input.GetAxis("Trigger R")) *  0.05f;
		}

	}

}

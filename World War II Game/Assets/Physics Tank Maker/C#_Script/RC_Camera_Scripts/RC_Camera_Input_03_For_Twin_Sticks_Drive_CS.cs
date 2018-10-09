using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class RC_Camera_Input_03_For_Twin_Sticks_Drive_CS : RC_Camera_Input_02_For_Single_Stick_Drive_CS
	{

		public override void Get_Input()
		{
			// Switch
			if (dPadPressed == false && Input.GetAxis("Vertical3") < 0.0f) { // D-Pad down.
				dPadPressed = true;
				rcCameraScript.Switch_RC_Camera();
			}
			else if (dPadPressed == true && Input.GetAxis("Vertical3") == 0.0f) { // D-pad is not pressed.
				dPadPressed = false;
			}

			if (rcCameraScript.RC_Camera.enabled == false) { // The RC camera is disabled.
				return;
			}

			// Zoom
			rcCameraScript.Zoom_Input = (Input.GetAxis("Trigger L") - Input.GetAxis("Trigger R")) * 0.05f;

			// Turn
			multiplier = Mathf.Lerp (0.01f, 1.0f, rcCameraScript.RC_Camera.fieldOfView / 50.0f); // Change the rotation speed according to the FOV of the RC camera.
			rcCameraScript.Horizontal_Input = Input.GetAxis("Horizontal") * multiplier;
			if (Input.GetButton("Bumper L")) { // Cancel for rotating the cannon.
				rcCameraScript.Vertical_Input = 0.0f;
			}
			else {
				rcCameraScript.Vertical_Input = Input.GetAxis("Vertical2") * multiplier;
			}
			rcCameraScript.Is_Turning = (rcCameraScript.Horizontal_Input != 0.0f || rcCameraScript.Vertical_Input != 0.0f);
		}

	}

}

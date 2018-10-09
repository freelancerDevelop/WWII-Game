using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class RC_Camera_Input_04_For_Triggers_Drive_CS : RC_Camera_Input_02_For_Single_Stick_Drive_CS
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
			if (Input.GetButton("Jump")) {
				rcCameraScript.Zoom_Input = -0.05f;
			}
			else if (Input.GetButton("Fire2")) {
				rcCameraScript.Zoom_Input = 0.05f;
			}
			else {
				rcCameraScript.Zoom_Input = 0.0f;
			}

			// Rotation.
			multiplier = Mathf.Lerp (0.01f, 1.0f, rcCameraScript.RC_Camera.fieldOfView / 50.0f); // Change the rotation speed according to the FOV of the RC camera.
			rcCameraScript.Horizontal_Input = Mathf.Pow (Input.GetAxis ("Horizontal2"), 2.0f) * Mathf.Sign(Input.GetAxis ("Horizontal2")) * multiplier;
			rcCameraScript.Vertical_Input = Mathf.Pow (Input.GetAxis ("Vertical2"), 2.0f) * Mathf.Sign(Input.GetAxis ("Vertical2")) * multiplier;
			rcCameraScript.Is_Turning = (rcCameraScript.Horizontal_Input != 0.0f || rcCameraScript.Vertical_Input != 0.0f);
		}

	}

}

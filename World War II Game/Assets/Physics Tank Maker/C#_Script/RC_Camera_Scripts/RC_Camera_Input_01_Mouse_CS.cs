using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class RC_Camera_Input_01_Mouse_CS : RC_Camera_Input_00_Base_CS
	{

		public override void Prepare(RC_Camera_CS rcCameraScript)
		{
			this.rcCameraScript = rcCameraScript;

			rcCameraScript.Use_Analog_Stick = false;
		}


		public override void Get_Input()
		{
			// Switch
			if (Input.GetKeyDown (KeyCode.Tab)) {
				rcCameraScript.Switch_RC_Camera();
			}

			if (rcCameraScript.RC_Camera.enabled == false) { // The RC camera is disabled.
				return;
			}

			// Zoom
			rcCameraScript.Zoom_Input = -Input.GetAxis ("Mouse ScrollWheel") * 2.0f;

			// Turn
			if (Input.GetKey (KeyCode.Z)) {
				rcCameraScript.Is_Turning = true;
				multiplier = Mathf.Lerp (0.2f, 4.0f, rcCameraScript.RC_Camera.fieldOfView / 50.0f); // Change the rotation speed according to the FOV of the RC camera.
				rcCameraScript.Horizontal_Input = Input.GetAxis ("Mouse X") * multiplier;
				rcCameraScript.Vertical_Input = Input.GetAxis ("Mouse Y") * multiplier;
			}
			else {
				rcCameraScript.Is_Turning = false;
			}
		}

	}

}

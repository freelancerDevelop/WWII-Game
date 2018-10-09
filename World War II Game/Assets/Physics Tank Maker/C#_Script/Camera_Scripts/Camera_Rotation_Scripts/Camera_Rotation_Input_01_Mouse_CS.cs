using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Camera_Rotation_Input_01_Mouse_CS : Camera_Rotation_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			// Cancel while the cursor is displayed. 
			if (Cursor.lockState == CursorLockMode.None) {
				rotationScript.Horizontal_Input = 0.0f;
				rotationScript.Vertical_Input = 0.0f;
				return;
			}

			multiplier = Mathf.Lerp(0.1f, 2.0f, rotationScript.Main_Camera.fieldOfView / 15.0f); // Change the rotation speed according to the FOV of the main camera.
			rotationScript.Horizontal_Input = Input.GetAxis("Mouse X") * multiplier;
			rotationScript.Vertical_Input = Input.GetAxis("Mouse Y") * multiplier;
		}

	}

}

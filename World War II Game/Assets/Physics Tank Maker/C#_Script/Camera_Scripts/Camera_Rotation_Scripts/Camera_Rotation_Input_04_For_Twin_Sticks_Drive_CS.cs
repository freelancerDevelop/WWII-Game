﻿using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Camera_Rotation_Input_04_For_Twin_Sticks_Drive_CS : Camera_Rotation_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			// Look forward.
			if (Input.GetButtonDown("Fire1")) {
				rotationScript.Look_Forward();
			}

			// Rotation.
			multiplier = Mathf.Lerp(0.1f, 2.0f, rotationScript.Main_Camera.fieldOfView / 15.0f); // Change the rotation speed according to the FOV of the main camera.
			rotationScript.Horizontal_Input = Input.GetAxis("Horizontal") * multiplier;
			if (Input.GetButton("Bumper L")) { // Cancel for rotating the cannon.
				rotationScript.Vertical_Input = 0.0f;
			}
			else {
				rotationScript.Vertical_Input = Input.GetAxis("Vertical2") * multiplier;
			}
		}

	}

}

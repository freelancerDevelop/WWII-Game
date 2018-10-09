using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Gun_Camera_Input_02_For_Single_Stick_Drive_CS : Gun_Camera_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			if (Input.GetButtonDown("Bumper L")) {
				gunCameraScript.Switch_Mode(2); // On
			}

			else if (Input.GetButton("Bumper L")) {
				gunCameraScript.Zoom_Input = (Input.GetAxis("Trigger L") - Input.GetAxis("Trigger R")) *  0.05f;
			}

			else if (Input.GetButtonUp("Bumper L")) {
				gunCameraScript.Switch_Mode(1); // Off
			}
		}

	}

}

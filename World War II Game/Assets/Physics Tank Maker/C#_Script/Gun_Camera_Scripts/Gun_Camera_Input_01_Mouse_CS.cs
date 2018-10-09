using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Gun_Camera_Input_01_Mouse_CS : Gun_Camera_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			if (Input.GetKeyDown(KeyCode.Space)) {
				gunCameraScript.Switch_Mode(2); // On
			}

			else if (Input.GetKey(KeyCode.Space)) {
				gunCameraScript.Zoom_Input = -Input.GetAxis("Mouse ScrollWheel") * 2.0f;
			}

			else if (Input.GetKeyUp(KeyCode.Space)) {
				gunCameraScript.Switch_Mode(1); // Off
			}
		}

	}

}

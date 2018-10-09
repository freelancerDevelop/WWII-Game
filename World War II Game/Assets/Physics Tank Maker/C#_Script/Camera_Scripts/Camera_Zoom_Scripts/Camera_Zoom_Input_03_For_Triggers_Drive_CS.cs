using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Camera_Zoom_Input_03_For_Triggers_Drive_CS : Camera_Zoom_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			if (Input.GetButton("Jump")) {
				zoomScript.Zoom_Input = -0.05f;
			}
			else if (Input.GetButton("Fire2")) {
				zoomScript.Zoom_Input = 0.05f;
			}
			else {
				zoomScript.Zoom_Input = 0.0f;
			}
		}

	}

}

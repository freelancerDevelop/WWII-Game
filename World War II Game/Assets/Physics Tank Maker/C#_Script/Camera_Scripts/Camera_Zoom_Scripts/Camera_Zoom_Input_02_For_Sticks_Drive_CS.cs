using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Camera_Zoom_Input_02_For_Sticks_Drive_CS : Camera_Zoom_Input_00_Base_CS
	{

		public override void Get_Input()
		{
			zoomScript.Zoom_Input = (Input.GetAxis("Trigger L") - Input.GetAxis("Trigger R")) * 0.05f;
		}

	}

}

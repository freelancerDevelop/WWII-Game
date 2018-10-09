using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Drive_Control_Input_04_Twin_Sticks_CS : Drive_Control_Input_03_Single_Stick_CS
	{
		
		public override void Drive_Input()
		{
			vertical = Input.GetAxis("Vertical");

			if (Input.GetButton("Bumper L")) { // Cancel for rotating the turret.
				horizontal = 0.0f;
			}
			else {
				horizontal = Input.GetAxis("Horizontal2");
			}

			Set_Values();
		}

	}

}

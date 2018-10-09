using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Drive_Control_Input_02_Keyboard_Pressing_CS : Drive_Control_Input_01_Keyboard_Stepwise_CS
	{
		
		public override void Drive_Input()
		{
			// Set "vertical".
			if (Input.GetKey(KeyCode.W)) { // Forward
				vertical = 1.0f;
			} else if (Input.GetKey(KeyCode.S)) { // Backward
				vertical = -1.0f;
			} else {
				vertical = 0.0f;
			}

			// Set "horizontal".
			if (Input.GetKey(KeyCode.A)) { // Left
				horizontal = -1.0f;
			} else if (Input.GetKey(KeyCode.D)) { // Right
				horizontal = 1.0f;
			} else {
				horizontal = 0.0f;
			}

			// Set the "Stop_Flag", "L_Input_Rate", "R_Input_Rate" and "Turn_Brake_Rate".
			Set_Values();
		}

	}

}

using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Steer_Wheel_Input_04_For_Triggers_Drive_CS : Steer_Wheel_Input_00_Base_CS
	{
		
		public override void Get_Input()
		{
			float leftRate = Input.GetAxis("Trigger L") - Input.GetAxis("Bumper L");
			float rightRate = Input.GetAxis("Trigger R") - Input.GetAxis("Bumper R");
			float tempHorizontal = Mathf.Clamp(leftRate - rightRate, -1.0f, 1.0f);
			if ((leftRate == 0.0f || rightRate == 0.0f) && (leftRate < 0.0f || rightRate < 0.0f)) { // Backward turn.
				tempHorizontal = -tempHorizontal;
			}
			steerScript.Horizontal_Input = tempHorizontal;
		}

	}

}

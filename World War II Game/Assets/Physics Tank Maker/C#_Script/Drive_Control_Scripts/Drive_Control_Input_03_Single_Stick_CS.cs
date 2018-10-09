using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Drive_Control_Input_03_Single_Stick_CS : Drive_Control_Input_01_Keyboard_Stepwise_CS
	{
		
		public override void Drive_Input()
		{
			vertical = Input.GetAxis("Vertical");
			horizontal = Input.GetAxis("Horizontal");
			Set_Values();
		}


		protected override void Brake_Turn ()
		{
			// Clamp the "horizontal" value to reduce the turning speed.
			float clamp = Mathf.Abs (vertical);
			horizontal = Mathf.Clamp (horizontal, -clamp, clamp);

			controlScript.L_Input_Rate = Mathf.Clamp (-vertical - horizontal * Mathf.Sign (vertical), -1.0f, 1.0f); // (Note.) "* Mathf.Sign (vertical)" is attached for turning backward.
			controlScript.R_Input_Rate = Mathf.Clamp (vertical - horizontal * Mathf.Sign (vertical), -1.0f, 1.0f);
			controlScript.Turn_Brake_Rate = horizontal;
		}

	}

}

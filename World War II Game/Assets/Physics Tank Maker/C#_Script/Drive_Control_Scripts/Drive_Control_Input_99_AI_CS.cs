using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Drive_Control_Input_99_AI_CS : Drive_Control_Input_01_Keyboard_Stepwise_CS
	{
		
		AI_CS aiScript;


		public override void Prepare(Drive_Control_CS controlScript)
		{
			// Store the reference to "Drive_Control_CS".
			this.controlScript = controlScript;

			// Store the reference to "AI_CS".
			aiScript = GetComponentInChildren <AI_CS>();
		}


		public override void Drive_Input()
		{
			vertical = aiScript.Speed_Order;
			horizontal = aiScript.Turn_Order;

			Set_Values();
		}


		protected override void Brake_Turn ()
		{
			controlScript.L_Input_Rate = Mathf.Clamp (-vertical - horizontal, -1.0f, 1.0f);
			controlScript.R_Input_Rate = Mathf.Clamp (vertical - horizontal, -1.0f, 1.0f);
			controlScript.Turn_Brake_Rate = horizontal;
		}

	}

}

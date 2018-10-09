using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Drive_Control_Input_01_Keyboard_Stepwise_CS : Drive_Control_Input_00_Base_CS
	{
		
		protected float vertical;
		protected float horizontal;
		float brakingTime = 1.0f;


		public override void Drive_Input()
		{
			// Set "vertical".
			if (Input.GetKeyDown (KeyCode.W)) { // Forward
				vertical += 0.25f;
			} else if (Input.GetKeyDown (KeyCode.S)) { // Backward
				vertical -= 0.25f;
			} else if (Input.GetKey (KeyCode.X)) { // Stop
				vertical = 0.0f;
			}
			vertical = Mathf.Clamp (vertical, -0.5f, 1.0f);

			// Set "horizontal".
			if (Input.GetKey(KeyCode.A)) { // Left
				horizontal = -1.0f;
			} else if (Input.GetKey(KeyCode.D)) { // Right
				horizontal = 1.0f;
			} else { // No turn.
				horizontal = 0.0f;
			}

			// Set the "Stop_Flag", "L_Input_Rate", "R_Input_Rate" and "Turn_Brake_Rate".
			Set_Values();
		}


		protected void Set_Values ()
		{
			// In case of stopping.
			if (vertical == 0.0f && horizontal == 0.0f) { // The tank should stop.
				controlScript.Stop_Flag = true;
				controlScript.L_Input_Rate = 0.0f;
				controlScript.R_Input_Rate = 0.0f;
				controlScript.Turn_Brake_Rate = 0.0f;
				return;
			}
			else { // The tank should be driving.
				controlScript.Stop_Flag = false;
			}

			// In case of going straight.
			if (horizontal == 0.0f) { // The tank should be going straight.
				controlScript.L_Input_Rate = -vertical;
				controlScript.R_Input_Rate = vertical;
				controlScript.Turn_Brake_Rate = 0.0f;
				return;
			}

			// In case of pivot-turn.
			if (vertical == 0.0f) { // The tank should be doing pivot-turn.
				horizontal *= controlScript.Pivot_Turn_Rate;
				controlScript.L_Input_Rate = -horizontal;
				controlScript.R_Input_Rate = -horizontal;
				controlScript.Turn_Brake_Rate = 0.0f;
				return;
			}

			// In case of brake-turn.
			Brake_Turn();
		}


		protected virtual void Brake_Turn ()
		{
			if (horizontal < 0.0f) { // Left turn.
				controlScript.L_Input_Rate = 0.0f;
				controlScript.R_Input_Rate = vertical;
			}
			else { // Right turn.
				controlScript.L_Input_Rate = -vertical;
				controlScript.R_Input_Rate = 0.0f;
			}

			// Increase the "Turn_Brake_Rate" with the lapse of time.
			controlScript.Turn_Brake_Rate += (1.0f / brakingTime / Mathf.Abs (controlScript.Speed_Rate)) * Time.deltaTime * Mathf.Sign(horizontal);
			controlScript.Turn_Brake_Rate = Mathf.Clamp (controlScript.Turn_Brake_Rate, -1.0f, 1.0f);
		}

	}

}

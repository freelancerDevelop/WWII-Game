using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Aiming_Control_Input_04_For_Twin_Sticks_Drive_CS : Aiming_Control_Input_00_Base_CS
	{

		public override void Prepare(Aiming_Control_CS aimingScript)
		{
			this.aimingScript = aimingScript;
			bulletGeneratorScripts = GetComponentsInChildren <Bullet_Generator_CS>();

			aimingScript.Use_Auto_Turn = false;
		}


		public override void Get_Input()
		{
			// Rotate the turret and the cannon manually.
			if (Input.GetButton ("Bumper L")) {
				aimingScript.Turret_Turn_Rate = Input.GetAxis ("Horizontal2");
				aimingScript.Cannon_Turn_Rate = -Input.GetAxis ("Vertical2");
			}
			else {
				aimingScript.Turret_Turn_Rate = 0.0f;
				aimingScript.Cannon_Turn_Rate = 0.0f;
			}

			// Switch the bullet type.
			if (Input.GetButtonDown("Back")) {
				for (int i = 0; i < bulletGeneratorScripts.Length; i++) {
					if (bulletGeneratorScripts[i] == null) {
						continue;
					}
					bulletGeneratorScripts[i].Switch_Bullet_Type();
				}
			}
		}

	}

}

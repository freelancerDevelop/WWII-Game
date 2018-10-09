using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Cannon_Fire_Input_99_AI_CS : Cannon_Fire_Input_00_Base_CS
	{

		AI_CS aiScript;
		Turret_Horizontal_CS turretScript;
		Cannon_Vertical_CS cannonScript;
		Aiming_Control_CS aimingScript;
		float obstacleCount;
		float aimingCount;


		public override void Prepare(Cannon_Fire_CS cannoFireScript)
		{
			this.cannonFireScript = cannoFireScript;

			aiScript = transform.root.GetComponentInChildren <AI_CS>();
			turretScript = GetComponentInParent <Turret_Horizontal_CS>();
			cannonScript = GetComponent <Cannon_Vertical_CS>();
			aimingScript = GetComponentInParent <Aiming_Control_CS>();
		}


		public override void Get_Input()
		{
			// Check the AI gives an oder to fire.
			if (aiScript.OpenFire_Flag == false) { // The AI does not give an oder to fire.
				return;
			}

			// Check all the "Bullet_Generator" in the children can aim the target.
			if (aiScript.Direct_Fire == true) { // The tank aims a target directly.
				for (int i = 0; i < cannonFireScript.Bullet_Generator_Scripts.Length; i++) {
					if (cannonFireScript.Bullet_Generator_Scripts[i].Can_Aim == false) { // At least one of the "Bullet_Generator" in the children cannot aim the target.
						if (turretScript.Is_Ready && cannonScript.Is_Ready) { // The turret and the cannon are ready to fire >> The target might be behind an obstacle.
							obstacleCount += Time.deltaTime;
							if (obstacleCount > 1.0f) {
								obstacleCount = 0.0f;
								aimingScript.AI_Random_Offset();
							}
							return;
						} // The turret and the cannon are not ready to fire.
						obstacleCount = 0.0f;
						return;
					}
					obstacleCount = 0.0f;
				}
			}

			// Check the turret and the cannon are ready to fire.
			if (turretScript.Is_Ready && cannonScript.Is_Ready) { // The turret and the cannon are ready to fire.
				aimingCount += Time.deltaTime;
				if (aimingCount > aiScript.Fire_Count) {
					// Fire.
					cannonFireScript.Fire();
					aimingCount = 0.0f;
					aimingScript.AI_Random_Offset();
				}
			}
			else { // The turret and the cannon are not ready to fire.
				aimingCount = 0.0f;
			}
		}

	}

}

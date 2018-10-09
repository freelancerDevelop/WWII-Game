using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Cannon_Fire_Input_01_Mouse_CS : Cannon_Fire_Input_00_Base_CS
	{

		Turret_Horizontal_CS turretScript;


		public override void Prepare(Cannon_Fire_CS cannonFireScript)
		{
			this.cannonFireScript = cannonFireScript;

			turretScript = GetComponentInParent <Turret_Horizontal_CS>();
		}


		public override void Get_Input()
		{
			if (turretScript.Is_Ready && Input.GetMouseButton(0)) {
				cannonFireScript.Fire();
			}
		}

	}

}

using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Damage_Control_01_MainBody_CS : Damage_Control_00_Base_CS
	{

		public override bool Get_Damage(float damage, int bulletType)
		{ // Called from "Bullet_Control_CS", when the bullet hits this collider.
			// Send the damage value to the "Damage_Control_Center_CS".
			return centerScript.Receive_Damage(damage, 0, 0); // type = 0 (MainBody), index =0 (useless)
		}


		void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
			Destroy (this);
		}

	}

}

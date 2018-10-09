using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Aiming_Control_Input_00_Base_CS : MonoBehaviour
	{

		protected Aiming_Control_CS aimingScript;
		protected Bullet_Generator_CS[] bulletGeneratorScripts;


		public virtual void Prepare(Aiming_Control_CS aimingScript)
		{
			this.aimingScript = aimingScript;
			bulletGeneratorScripts = GetComponentsInChildren <Bullet_Generator_CS>();
		}


		public virtual void Get_Input()
		{
		}

	}

}

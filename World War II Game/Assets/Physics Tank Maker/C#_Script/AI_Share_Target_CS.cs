using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{
	
	public class AI_Share_Target_CS : MonoBehaviour
	{
		/*
		 * This script is automatically attached to "AI_Core" object in the AI tank by the "AI_CS" script, only when the "Share_Target" is set in the "AI_Settings_CS" script.
		 * This script works in combination with "AI_CS" in this AI tank, and "Aiming_Control_CS" in the share target tank.
		 * When the share target tank locks on the target, this AI tank also tries to aim the same target.
		 * (Note.) The position of the target is not shared. So the AI tank needs to find the target itself.
		*/


		public AI_CS AI_Script; // Set by "AI_CS".

		Aiming_Control_CS shareAimingScript;
		Transform currentTargetTransform;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			// Get "Aiming_Control_CS" in the share target tank.
			shareAimingScript = AI_Script.Settings_Script.Share_Target.GetComponentInChildren <Aiming_Control_CS>();
			if (shareAimingScript == null) {
				AI_Script.Is_Sharing_Target = false;
				Destroy (this);
			}
		}


		void Update()
		{
			Share_Target();
		}


		void Share_Target()
		{
			// Check the share target tank exists.
			if (AI_Script.Settings_Script.Share_Target == null) { // The share target tank might have been removed from the scene.
				// Stop sharing the target.
				AI_Script.Is_Sharing_Target = false;
				Destroy (this);
				return;
			}

			// Check the share target tank is living.
			if (shareAimingScript == null) {
				if (AI_Script.Settings_Script.Share_Target.root.tag == "Finish") { // The share target tank has been destroyed.
					// Stop sharing the target.
					AI_Script.Is_Sharing_Target = false;
					return;
				}
				else { // The share target tank has been respawned.
					shareAimingScript = AI_Script.Settings_Script.Share_Target.GetComponentInChildren <Aiming_Control_CS>();
				}
			}

			// Check the share target tank has changed the target or not.
			if (currentTargetTransform != shareAimingScript.Target_Transform) { // The share target tank has changed the target.
				currentTargetTransform = shareAimingScript.Target_Transform;
				if (currentTargetTransform == null) { // The share target tank is not locking on any target.
					// Stop sharing the target.
					AI_Script.Is_Sharing_Target = false;
					AI_Script.Lost_Target();
				}
				else { // The share target tank is locking on the target.
					// Get the target information, and send it to the "AI_CS".
					AI_Headquaters_Helper_CS targetAIHelperScript = currentTargetTransform.GetComponentInParent <AI_Headquaters_Helper_CS>();
					if (targetAIHelperScript) {
						// Restart sharing the target.
						AI_Script.Is_Sharing_Target = true;
						// Send the target information to the "AI_CS".
						AI_Script.Set_Target(targetAIHelperScript);
					}
				}
			}
		}

	}

}
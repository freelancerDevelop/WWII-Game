using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{


	public class Input_Type_Settings_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the top object of the tank.
		 * This script changes all the "Input_Type" values in the tank, and "Use_Auto_Lead" value in the "Aiming_Control_CS" at the opening.
		 * When there is "Input_Type_Manager_CS" in the scene (usually attached in the "Game_Controller"), these values are overwritten with that value.
		 * 
		 * [Input_Type]
		 * 0 => Mouse + Keyboard (Stepwise)
		 * 1 => Mouse + Keyboard (Pressing)
		 * 2 => Mouse + Keyboard (Legacy)
		 * 3 => GamePad (Single stick)
		 * 4 => GamePad (Twin sticks)
		 * 5 => GamePad (Triggers)
		 * 10 => AI
		*/

		// User options >>
		public int Input_Type;
		public bool Use_Auto_Lead = false;
		// << User options


		void Awake()
		{ // The "Input_Type" value must be prepared before "Start()", because several scripts use "Input_Type" value in the "Start()".
			if (GetComponent <AI_Settings_CS>() && GetComponentInChildren <AI_CS>()) { // This tank should be an AI tank.
				Input_Type = 10; // AI
			}
			else { // This tank should not be an AI tank.
				// Overwrite the "Input_Type" and "Use_Auto_Lead" values, when there is "Input_Type_Manager_CS" in the scene.
				Input_Type_Manager_CS managerScript = FindObjectOfType <Input_Type_Manager_CS>();
				if (managerScript) {
					Input_Type = managerScript.Input_Type;
					Use_Auto_Lead = managerScript.Use_Auto_Lead;
				}
			}

			// Broadcast the "Input_Type" to all the children.
			BroadcastMessage ("Get_Input_Type", Input_Type, SendMessageOptions.DontRequireReceiver);
		}


		void Start()
		{
			// Overwrite "Use_Auto_Lead" value in the "Aiming_Control_CS" in the tank.
			if (Input_Type == 10) { // AI
				return; // In the case of AI, the "Use_Auto_Lead" is set to ture forcibly in the "Aiming_Control_CS".
			}
			Aiming_Control_CS aimingScript = GetComponentInChildren <Aiming_Control_CS>();
			if (aimingScript) {
				aimingScript.Use_Auto_Lead = Use_Auto_Lead;
			}
		}

	}

}
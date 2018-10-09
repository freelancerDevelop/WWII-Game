using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	public class Input_Type_Manager_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Game_Controller" in the scene.
		 * This "Input_Type" value is referred to from "Input_Type_Settings_CS" in all the tanks in the scene.
		 * User can change "Input_Type" values of all the tanks in the scene from this script.
		 * 
		 * When "Option_Manager_CS" has come from the menu scene, this "Input_Type" value is overwritten with that value.
		 * (Note.) All the tanks in the scene must be spawned by "Event_Controller_CS" to overwrite this "Input_Type" value using "Option_Manager_CS".
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
		public int Input_Type = 0; // Referred to from "Input_Type_Settings_CS" in tanks in the scene. Also overwritten by "Option_Manager_CS" that has come from the menu scene.
		public bool Use_Auto_Lead = false;
		public bool Show_Cursor_Forcibly = false;
		// << User options


		void Start()
		{
			Set_Cursor_State();
		}


		void Set_Cursor_State()
		{
			if (Show_Cursor_Forcibly == true) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				return;
			}

			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					break;
				case 2: // Mouse + Keyboard (Legacy)
				case 3: // GamePad (Single stick)
				case 4: // GamePad (Twin sticks)
				case 5: // GamePad (Triggers)
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					break;
			}
		}

	}

}
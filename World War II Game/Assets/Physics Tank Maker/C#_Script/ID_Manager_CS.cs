using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChobiAssets.PTM
{
	
	public class ID_Manager_CS : MonoBehaviour
	{
		/*
		 * This script is attached to "Game_Controller" in the scene.
		 * This script works in combination with "ID_Settings_CS" in tanks in the scene.
		 * This script gives proper ID number to each "ID_Settings_CS".
		 * The player can select the tank by pressing numpad "+", "-" and "Enter" keys.
		*/

		List <bool> selectableTanksList = new List<bool>();
		int currentID = 1;


		public int Get_Proper_ID(int tempID)
		{ // Called from "ID_Settings_CS" in tanks in the scene, when the tank is spawned.
			if (tempID == 0) { // The tank is not selectable. >> It it not neccesary to give a new number.
				return tempID;
			}

			// Check the size of "selectableTanksList", and increase the size if needed.
			if (selectableTanksList.Count < tempID) { // The list size is small.
				int additionalElementCount = tempID - selectableTanksList.Count;
				for (int i = 0; i < additionalElementCount; i++) {
					selectableTanksList.Add(false);
				}
			}

			// Check duplication of the ID numbers.
			if (selectableTanksList[tempID - 1] == true) { // The number is already used.
				Debug.LogWarning("'Tank ID' is duplicated. It will be changed to proper value by 'ID_Manager_CS' script.");
				// Get an empty number.
				for (int i = 0; i < selectableTanksList.Count; i++) {
					if (selectableTanksList[i] == false) { // Empty number is found.
						selectableTanksList[i] = true;
						// Give the new number.
						return i + 1;
					}
				} // The list is full. >> The list should be small.
				// Add one element, and set it to true.
				selectableTanksList.Add(true);
				// Give the new number.
				return selectableTanksList.Count;
			}
			else { // The number is not used yet. >> It it not neccesary to give a new number.
				selectableTanksList[tempID - 1] = true;
				return tempID;
			}
		}


		void Update()
		{
			Select_Tank();
		}


		void Select_Tank()
		{
			// Change "currentID" to "1".
			if (Input.GetKeyDown (KeyCode.KeypadEnter)){
				if (selectableTanksList[0] == true){
					currentID = 1;
					Broadcast_Current_ID();
				}
				return;
			}

			// Increase "currentID".
			if (Input.GetKeyDown (KeyCode.KeypadPlus)){
				for (int i = 0; i < selectableTanksList.Count; i++) {
					currentID += 1;
					if (currentID > selectableTanksList.Count) {
						currentID = 1;
					}
					if (selectableTanksList[currentID - 1] == true) {
						Broadcast_Current_ID();
						break;
					}
				}
				return;
			}

			// Decrease "currentID".
			if (Input.GetKeyDown (KeyCode.KeypadMinus)){
				for (int i = 0; i < selectableTanksList.Count; i++) {
					currentID -= 1;
					if (currentID < 1) {
						currentID = selectableTanksList.Count;
					}
					if (selectableTanksList[currentID - 1] == true) {
						Broadcast_Current_ID();
						break;
					}
				}
				return;
			}
		}


		void Broadcast_Current_ID()
		{
			// Get all the "ID_Settings_CS" in the scene.
			var idScripts = FindObjectsOfType <ID_Settings_CS>();

			// Send "currentID" to them.
			for (int i = 0; i < idScripts.Length; i++) {
				idScripts[i].Receive_Current_ID(currentID);
			}
		}


		public void Remove_ID(int removedID)
		{ // Called from "ID_Settings_CS", just before the tank is removed from the scene.
			// Set the element to false.
			selectableTanksList[removedID - 1] = false;

			// Check the ID.
			if (removedID == currentID) { // The current selected tank will be removed soon.
				// Change "currentID" to "1".
				// (Note.) Please do not remove the tank that its "Tank_ID" is set to "1".
				currentID = 1;
				Broadcast_Current_ID();
			}
		}

	}

}
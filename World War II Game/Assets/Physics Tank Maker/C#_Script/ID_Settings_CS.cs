using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class ID_Settings_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the top object of the tank.
		 * This script is used for giving an individual ID number to the tank. 
		 * When "ID_Settings_CS" script is put in the scene, this script can receive a proper ID number from it even if the number was overlapping with other tanks.
		*/


		// User options >>
		public int Tank_ID = 1; // "0" = Not selectable.
		public int Relationship;
		// << User options


		public bool Is_Selected; // Referred to from "UI_PosMarker_Control_CS" in the scene.

		int currentID = 1;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{ // This function must be called in Start(). Because this variables might be overwritten by "Event_Controller_CS" in the Awake().

			// Call "ID_Manager_CS" in the scene to get the proper "Tank_ID".
			var managerScript = FindObjectOfType <ID_Manager_CS>();
			if (managerScript) {
				Tank_ID = managerScript.Get_Proper_ID(Tank_ID);
			}

			// Set the "Is_Selected" value.
			Is_Selected = (Tank_ID == currentID);

			// Broadcast whether this tank is selected or not.
			Broadcast_Selection_Condition();

			// Call "UI_PosMarker_Control_CS" in the scene to create a new position marker for this tank.
			UI_PosMarker_Control_CS posMarkerScript = FindObjectOfType <UI_PosMarker_Control_CS>();
			if (posMarkerScript) {
				posMarkerScript.Receive_ID_Script(this);
			}
		}


		public void Receive_Current_ID(int currentID)
		{ // Called from "ID_Manager_CS" in the scene, when the player changes the tank.
			this.currentID = currentID;
			Is_Selected = (Tank_ID == currentID);
			Broadcast_Selection_Condition();
		}


		void Broadcast_Selection_Condition()
		{
			// Broadcast whether this tank is selected or not to all the children.
			BroadcastMessage ("Selected", Is_Selected, SendMessageOptions.DontRequireReceiver);
		}


		void Respawned()
		{ // Called from "Respawn_Controller_CS", when the tank has been respawned.
			// Broadcast whether this tank is selected or not.
			Broadcast_Selection_Condition();
		}


		void Prepare_Removing()
		{ // Called from "Respawn_Controller_CS", just before the tank is removed.
			if (Tank_ID == 0) { // This tank is not selectable.
				return;
			}

			// Call "ID_Manager_CS" in the scene to remove this tank from the selectable tanks list.
			var managerScript = FindObjectOfType <ID_Manager_CS>();
			if (managerScript) {
				managerScript.Remove_ID(Tank_ID);
			}
		}

	}

}

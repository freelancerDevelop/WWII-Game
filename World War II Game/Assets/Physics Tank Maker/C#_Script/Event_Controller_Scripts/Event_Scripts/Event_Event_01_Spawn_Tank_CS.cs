using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{
	
	public class Event_Event_01_Spawn_Tank_CS : Event_Event_00_Base_CS
	{

		public override void Prepare_Event(Event_Controller_CS eventControllerScript)
		{
			// Store the reference to "Event_Controller_CS".
			this.eventControllerScript = eventControllerScript;

			// Check the "Prefab_Object".
			if (eventControllerScript.Prefab_Object == null) {
				Debug.LogWarning("The event (" + this.name + ") cannot be executed, because the 'Tank Prefab' is not assigned.");
				Destroy(eventControllerScript);
				Destroy(this);
			}
		}


		public override void Execute_Event()
		{
			// Spawn the tank, and make it a child of the event GameObject.
			// (Note.) In this event system, the spawned tank must be placed under the event GameObject as its child in the hierarchy.
			GameObject newTank = Instantiate(eventControllerScript.Prefab_Object, transform.position, transform.rotation, eventControllerScript.transform) as GameObject;

			// Overwrite "ID_Settings_CS".
			var idScript = newTank.GetComponent <ID_Settings_CS>();
			if (idScript) {
				idScript.Tank_ID = eventControllerScript.Tank_ID;
				idScript.Relationship = eventControllerScript.Relationship;
			}

			// Overwrite "Special_Settings_CS".
			var specialSettingsScript = newTank.GetComponent <Special_Settings_CS>();
			if (specialSettingsScript) {
				specialSettingsScript.Attack_Multiplier = eventControllerScript.Attack_Multiplier;
				specialSettingsScript.Defence_Multiplier = eventControllerScript.Defence_Multiplier;
			}

			// Overwrite "Respawn_Controller_CS".
			var respawnScript = newTank.GetComponent <Respawn_Controller_CS>();
			if (respawnScript) {
				respawnScript.This_Prefab = eventControllerScript.Prefab_Object; // (Note.) The current prefab reference is linked to the new tank spawned in the scene. So it must be overwritten.
				respawnScript.Respawn_Times = eventControllerScript.Respawn_Times;
				respawnScript.Auto_Respawn_Interval = eventControllerScript.Auto_Respawn_Interval;
				respawnScript.Remove_After_Death = eventControllerScript.Remove_After_Death;
				if (respawnScript.Remove_After_Death) {
					respawnScript.Remove_Interval = eventControllerScript.Remove_Interval;
				}
			}

			// Overwrite "AI_Settings_CS".
			var aiSettingsScript = newTank.GetComponent <AI_Settings_CS>();
			if (aiSettingsScript) {
				aiSettingsScript.WayPoint_Pack = eventControllerScript.WayPoint_Pack;
				aiSettingsScript.Patrol_Type = eventControllerScript.Patrol_Type;
				aiSettingsScript.Follow_Target = eventControllerScript.Follow_Target;
				aiSettingsScript.No_Attack = eventControllerScript.No_Attack;
				aiSettingsScript.Breakthrough = eventControllerScript.Breakthrough;
				aiSettingsScript.Share_Target = eventControllerScript.Share_Target;
				aiSettingsScript.Visibility_Radius = eventControllerScript.Visibility_Radius;
				aiSettingsScript.Approach_Distance = eventControllerScript.Approach_Distance;
				aiSettingsScript.OpenFire_Distance = eventControllerScript.OpenFire_Distance;
				aiSettingsScript.Lost_Count = eventControllerScript.Lost_Count;
				aiSettingsScript.Face_Offest_Angle = eventControllerScript.Face_Offest_Angle;
				aiSettingsScript.AI_State_Text = eventControllerScript.AI_State_Text;
				aiSettingsScript.Tank_Name = eventControllerScript.Tank_Name;
			}

			// End the event.
			Destroy (eventControllerScript); // (Note.) Do not destroy the event GameObject. Because the spawned tank is placed under this object in the hierarchy.
			Destroy (this);
		}

	}

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	public class Tank_ID_Control_CS: MonoBehaviour
	{
		/*
		* This script is obsolete.
		* Please update your tank referring to the manual.
		*/

		public GameObject This_Prefab;
		public int Tank_ID = 1;
		public int Relationship;
		public int ReSpawn_Times = 0;
		public float ReSpawn_Interval = 10.0f;
		public float Attack_Multiplier = 1.0f;
		public int Input_Type;
		public bool Auto_Lead = false;
		public string Marker_Name = "Pos_Marker";
		// AI patrol settings.
		public GameObject WayPoint_Pack;
		public int Patrol_Type = 1; // 0 = Order, 1 = Random.
		public Transform Follow_Target;
		// AI combat settings.
		public bool No_Attack = false;
		public bool Breakthrough = false;
		public float Visibility_Radius = 512.0f;
		public float Approach_Distance = 256.0f;
		public float OpenFire_Distance = 512.0f;
		public float Lost_Count = 20.0f;
		public bool Face_Enemy = false;
		public float Face_Offest_Angle = 0.0f;
		// AI text settings.
		public Text AI_State_Text;
		public string Tank_Name;
		// AI remove settings.
		public float Remove_Time = 30.0f;

		// Used for updating.
		public GameObject Camera_Pivot_Prefab;
	}

}
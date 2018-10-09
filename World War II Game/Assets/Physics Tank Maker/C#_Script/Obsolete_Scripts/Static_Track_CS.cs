using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Static_Track_CS : MonoBehaviour
	{
		/*
		 * This script is obsolete.
		 * Please update your Static Track referring to the manual.
		*/


		// User options >>
		public int Type;
		public Transform Front_Transform;
		public Transform Rear_Transform;
		public Static_Track_CS Front_Script;
		public Static_Track_CS Rear_Script;
		public string Anchor_Name;
		public string Anchor_Parent_Name;
		public Transform Anchor_Transform;
		public Transform Reference_L;
		public Transform Reference_R;
		public string Reference_Name_L;
		public string Reference_Name_R;
		public string Reference_Parent_Name_L;
		public string Reference_Parent_Name_R;
		public float Length;
		public float Radius_Offset;
		public float Mass = 30.0f;
		public Mesh Track_L_Shadow_Mesh;
		public Mesh Track_R_Shadow_Mesh;
		public float RoadWheel_Effective_Range = 0.4f;
		public float SwingBall_Effective_Range = 0.15f;
		public float Anti_Stroboscopic_Min = 0.125f;
		public float Anti_Stroboscopic_Max = 0.375f;
		// << User options


		// Set by "Create_TrackBelt_CSEditor".
		public RoadWheelsProp [] RoadWheelsProp_Array;
		public float Stored_Body_Mass;
		public float Stored_Torque;
		public float Stored_Turn_Brake_Drag;
	}

}
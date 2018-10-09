using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
	
	public class Sound_Control_CS : MonoBehaviour
	{
		/*
		 * This script is obsolete.
		 * Please update the sound system in your tank referring to the manual.
		*/


		public int Type = 0;

		// Engine Sound.
		public float Min_Engine_Pitch = 1.0f;
		public float Max_Engine_Pitch = 2.0f;
		public float Min_Engine_Volume = 0.1f;
		public float Max_Engine_Volume = 0.3f;
		public float Max_Velocity = 7.0f;
		public Rigidbody Left_Wheel_Rigidbody;
		public Rigidbody Right_Wheel_Rigidbody;

		// Impact Sound from MainBody.
		public float Min_Impact = 0.25f;
		public float Max_Impact = 0.5f;
		public float Min_Impact_Pitch = 0.3f;
		public float Max_Impact_Pitch = 1.0f;
		public float Min_Impact_Volume = 0.1f;
		public float Max_Impact_Volume = 0.5f;

		// Turret Motor Sound.
		public float Max_Motor_Volume = 0.5f;

	}

}
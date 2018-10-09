using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Damage_Control_CS : MonoBehaviour
	{
	
		/*
		 * This script is obsolete.
		 * Please update the damage system in your tank referring to the manual.
		*/

		public int Type = 1; // 1=Armor_Collider , 2=Turret, 3=Cannon, 4=Barrel, 5=MainBody, 6=Physics Track, 8=Physics_Wheel, 9=Track_Collider.
		public float Mass = 200.0f; // for Turret.
		public int Direction; // for Physics_Track piece, Physics_Wheel, TrackCollider. (0=Left, 1=Right)
		public float Durability = 130000.0f;
		public bool Coming_Off = true; // for Turret
		public GameObject Damage_Effect_Object; // for Turret
		public Transform Linked_Transform; // for Track_Collider.

	}

}
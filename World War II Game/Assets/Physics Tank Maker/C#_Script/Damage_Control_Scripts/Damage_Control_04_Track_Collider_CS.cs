using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Damage_Control_04_Track_Collider_CS : Damage_Control_00_Base_CS
	{
		
		public int Track_Index; // 0 = Left, 1 = Right.
		public Static_Track_Piece_CS Linked_Piece_Script;


		void Awake()
		{ // (Note.) The hierarchy must be changed before "Start()", because this gameobject might be inactivated by the "Track_LOD_Control_CS" in the "Start()".
			// Change the hierarchy. (Make this Track_Collider a child of the MainBody.)
			transform.parent = transform.parent.parent;
		}


		public override void Start()
		{
			centerScript = GetComponentInParent <Damage_Control_Center_CS>();

			// Make this collider a trigger.
			GetComponent <Collider>().isTrigger = true;

			// Make this invisible.
			MeshRenderer meshRenderer = GetComponent <MeshRenderer>();
			if (meshRenderer) {
				meshRenderer.enabled = false;
			}
		}


		public override bool Get_Damage(float damage, int bulletType)
		{ // Called from "Bullet_Control_CS", when the bullet hits this collider.
			// Send the damage value to the "Damage_Control_Center_CS".
			if (centerScript.Receive_Damage(damage, 3, Track_Index) == true) { // type = 3 (Track_Collider), index = Track_Index (0 = Left, 1 = Right). true = The track has been destroyed.
				// Breaking the track by calling the "Linked_Piece_Script". ("Static_Track_Piece_CS" script in the track piece.)
				if (Linked_Piece_Script) {
					Linked_Piece_Script.Start_Breaking ();
				}
				return true;
			}
			else { // The track has not been destroyed.
				return false;
			}
		}


		void Track_Destroyed_Linkage(bool isLeft)
		{ // Called from "Damage_Control_Center_CS", when the track has broken.
			if ((isLeft && Track_Index != 0) || (isLeft == false && Track_Index != 1)) { // The direction is different.
				return;
			}
			Destroy(this);
		}
	}

}

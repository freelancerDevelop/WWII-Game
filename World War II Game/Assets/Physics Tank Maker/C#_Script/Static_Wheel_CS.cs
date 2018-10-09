using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
	
	public class Static_Wheel_CS : MonoBehaviour
	{
		/*
		 * This script rotates the Static_Wheel in combination with "Static_Wheel_Parent_CS" in the parent object.
		*/


		// User options >>
		public bool Is_Left;
		public Static_Wheel_Parent_CS Parent_Script;
		// << User optionsss

		Transform thisTransform;
		Vector3 currentAng;


		void Start ()
		{
			thisTransform = transform;
		}


		void Update ()
		{
			if (Parent_Script.Is_Visible == false) { // MainBody is not visible by any camera.
				return;
			}

			Rotate();
		}


		void Rotate ()
		{
			currentAng = thisTransform.localEulerAngles;
			if (Is_Left) { // Left
				currentAng.y = Parent_Script.Left_Angle_Y;
			}
			else { // Right
				currentAng.y = Parent_Script.Right_Angle_Y;
			}
			thisTransform.localEulerAngles = currentAng;
		}


		void Track_Destroyed_Linkage(bool isLeft)
		{ // Called from "Damage_Control_Center_CS".
			if (isLeft != Is_Left) { // The direction is different.
				return;
			}

			// Resize the SphereCollider.
			SphereCollider sphereCollider = GetComponent <SphereCollider>();
			if (sphereCollider) {
				MeshFilter meshFilter = GetComponent <MeshFilter>();
				if (meshFilter && meshFilter.mesh) {
					sphereCollider.radius = meshFilter.mesh.bounds.extents.x;
				}
			}

			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}
		
}
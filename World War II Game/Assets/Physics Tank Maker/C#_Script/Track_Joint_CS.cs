using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
	public class Track_Joint_CS : MonoBehaviour
	{
		/*
		 * This script controls the position and rotation of the joint piece in the track.
		*/

		// User options >>
		public Transform Base_Transform;
		public Transform Front_Transform;
		public float Joint_Offset;
		public bool Is_Left;
		// << User options


		Transform thisTransform;
		MainBody_Setting_CS bodyScript;


		void Start()
		{
			thisTransform = transform;
			bodyScript = GetComponentInParent <MainBody_Setting_CS>();
		}


		void LateUpdate ()
		{
			if (bodyScript.Visible_Flag) { // MainBody is visible by any camera.
				Vector3 basePos = Base_Transform.position + (Base_Transform.forward * Joint_Offset);
				Vector3 frontPos = Front_Transform.position - (Front_Transform.forward * Joint_Offset);
				thisTransform.SetPositionAndRotation (Vector3.Slerp (basePos, frontPos, 0.5f), Quaternion.Slerp (Base_Transform.rotation, Front_Transform.rotation, 0.5f));
			}
		}
	

		void Track_Destroyed_Linkage(bool isLeft)
		{ // Called from "Damage_Control_Center_CS".
			if (isLeft != Is_Left) { // The direction is different.
				return;
			}

			// Set the position and rotation.
			thisTransform.position = Base_Transform.position + (Base_Transform.forward * (Joint_Offset * 2.0f));
			thisTransform.localEulerAngles = Vector3.zero;

			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
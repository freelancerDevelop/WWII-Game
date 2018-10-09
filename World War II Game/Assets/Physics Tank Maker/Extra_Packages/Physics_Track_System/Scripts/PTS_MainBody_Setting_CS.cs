﻿using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTS
{

	[ RequireComponent (typeof(MeshFilter))]
	[ RequireComponent (typeof(MeshRenderer))]
	[ RequireComponent (typeof(Rigidbody))]

	public class PTS_MainBody_Setting_CS : MonoBehaviour
	{

		public float Body_Mass = 2000.0f;
		public Mesh Body_Mesh;

		public int Materials_Num = 1;
		public Material[] Materials;
		public Material Body_Material;

		public Mesh Collider_Mesh;
		public Mesh Sub_Collider_Mesh;

		public int SIC = 35;
		public bool Soft_Landing_Flag;
		public float Landing_Drag = 20.0f;
		public float Landing_Time = 1.5f;
		public bool Mass_Center_Zero = false;

		// Referred from children.
		public bool Visible_Flag; // Referred to from "PTS_Static_Track_CS", "PTS_Static_Wheel_CS", "PTS_Track_Deform_CS", "PTS_Track_Scroll_CS", "PTS_Track_Joint_CS".

		void Awake ()
		{
			Layer_Collision_Settings ();
			Rigidbody thisRigidbody = GetComponent < Rigidbody > ();
			thisRigidbody.solverIterations = SIC;
			// Set the center of mass to zero.
			if (Mass_Center_Zero) {
				thisRigidbody.centerOfMass = Vector3.zero;
			}
			// Soft Landing.
			if (Soft_Landing_Flag) {
				StartCoroutine ("Soft_Landing", thisRigidbody);
			}
		}

		void Start ()
		{
			// Broadcast this reference.
			BroadcastMessage ("Get_Body_Script", this, SendMessageOptions.DontRequireReceiver); // Do not move this line into Awake ().
		}

		void Layer_Collision_Settings ()
		{
			/*
			Layer Collision Settings.
			Layer9 >> for wheels.
			Layer10 >> for Suspensions and Track Reinforce.
			Layer11 >> for MainBody.
			*/
			for (int i = 0; i <= 11; i++) {
				Physics.IgnoreLayerCollision (9, i, false);
				Physics.IgnoreLayerCollision (11, i, false);
			}
			Physics.IgnoreLayerCollision (9, 9, true); // Wheels ignore each other.
			Physics.IgnoreLayerCollision (9, 11, true); // Wheels ignore MainBody.
			for (int i = 0; i <= 11; i++) {
				Physics.IgnoreLayerCollision (10, i, true); // Suspensions and Track Reinforce ignore all.
			}
		}

		IEnumerator Soft_Landing (Rigidbody tempRigidbody)
		{
			float defaultDrag = tempRigidbody.drag;
			tempRigidbody.drag = Landing_Drag;
			tempRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			yield return new WaitForSeconds (Landing_Time);
			tempRigidbody.drag = defaultDrag;
			tempRigidbody.constraints = RigidbodyConstraints.None;
		}


		void OnBecameVisible ()
		{
			Visible_Flag = true;
		}

		void OnBecameInvisible ()
		{
			Visible_Flag = false;
		}

	}

}
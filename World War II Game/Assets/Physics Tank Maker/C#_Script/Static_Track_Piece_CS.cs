using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Static_Track_Piece_CS : MonoBehaviour
	{
		/*
		 * This script controls the position and rotation of Static_Track pieces.
		 * This script works in combination with "Static_Track_Parent_CS" in the parent object.
		*/


		// User options >>
		public Transform This_Transform;
		public Static_Track_Parent_CS Parent_Script;
		public int Type; // 0=Static, 1=Anchor, 2=Dynamic.
		public Transform Front_Transform;
		public Transform Rear_Transform;
		public Static_Track_Piece_CS Front_Script;
		public Static_Track_Piece_CS Rear_Script;
		public string Anchor_Name;
		public string Anchor_Parent_Name;
		public Transform Anchor_Transform;
		public bool Simple_Flag;
		public bool Is_Left;
		public float Invert_Angle; // Lower piece => 0.0f, Upper pieces => 180.0f.
		public float Half_Length;
		public int Pieces_Count;
		// << User options


		Vector3 invisiblePos;
		float invisibleAngY;
		float initialPosX; // Only for anchor type.
		float anchorInitialPosX; // Only for anchor type.
		Vector3 currentAngles = new Vector3 (0.0f, 0.0f, 270.0f);


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			switch (Type) {
				case 0: // Static.
					Basic_Settings ();
					break;
				case 1: // Anchor.
					Find_Anchor ();
					Basic_Settings ();
					break;
				case 2: // Dynamic.
					Basic_Settings ();
					break;
			}
		}


		void Basic_Settings()
		{
			// Set the initial position and angle.
			invisiblePos = This_Transform.localPosition;
			invisibleAngY = This_Transform.localEulerAngles.y;
		}


		void Find_Anchor()
		{
			if (Anchor_Transform == null) { // The "Anchor_Transform" should have been lost by modifying.
				// Get the "Anchor_Transform" with reference to the name.
				if (string.IsNullOrEmpty (Anchor_Name) == false && string.IsNullOrEmpty (Anchor_Parent_Name) == false) {
					Anchor_Transform = This_Transform.parent.parent.Find (Anchor_Parent_Name + "/" + Anchor_Name);
				}
			}

			// Set the initial hight. (Axis X = hight)
			if (Anchor_Transform) {
				initialPosX = This_Transform.localPosition.x;
				anchorInitialPosX = Anchor_Transform.localPosition.x;
			} else {
				Debug.LogWarning("'Anchor_Transform' cannot be found in " + This_Transform.name + ".");
				// Change this piece to "Dynamic" type.
				Type = 2;
			}
		}


		void LateUpdate ()
		{
			if (Parent_Script.Is_Visible == false) { // The MainBody is not visible by any camera.
				return;
			}

			switch (Type) {
				case 0: // Static.
					Slide_Control ();
					break;
				case 1: // Anchor.
					Anchor_Control ();
					Slide_Control ();
					break;
				case 2: // Dynamic.
					Dynamic_Control ();
					Slide_Control ();
					break;
			}
		}


		void Slide_Control()
		{
			if (Is_Left) { // Left
				This_Transform.localPosition = Vector3.Lerp (invisiblePos, Rear_Script.invisiblePos, Parent_Script.Rate_L);
				currentAngles.y = Mathf.LerpAngle(invisibleAngY, Rear_Script.invisibleAngY, Parent_Script.Rate_L);
				This_Transform.localRotation = Quaternion.Euler (currentAngles);
			}
			else { // Right
				This_Transform.localPosition = Vector3.Lerp (invisiblePos, Rear_Script.invisiblePos, Parent_Script.Rate_R);
				currentAngles.y = Mathf.LerpAngle(invisibleAngY, Rear_Script.invisibleAngY, Parent_Script.Rate_R);
				This_Transform.localRotation = Quaternion.Euler (currentAngles);
			}
		}


		void Dynamic_Control()
		{
			if (Simple_Flag == true) {
				invisiblePos = Vector3.Lerp(Rear_Script.invisiblePos, Front_Script.invisiblePos, 0.5f);
				invisibleAngY = Mathf.LerpAngle(Rear_Script.invisibleAngY, Front_Script.invisibleAngY, 0.5f);
			}
			else {
				// Calculate the end positions.
				float tempRad = Rear_Script.invisibleAngY * Mathf.Deg2Rad;
				Vector3 rearEndPos = Rear_Script.invisiblePos + new Vector3(Half_Length * Mathf.Sin(tempRad), 0.0f, Half_Length * Mathf.Cos(tempRad));
				tempRad = Front_Script.invisibleAngY * Mathf.Deg2Rad;
				Vector3 frontEndPos = Front_Script.invisiblePos - new Vector3(Half_Length * Mathf.Sin(tempRad), 0.0f, Half_Length * Mathf.Cos(tempRad));
				// Set the position and angle.
				invisiblePos = Vector3.Lerp(rearEndPos, frontEndPos, 0.5f);
				invisibleAngY = Mathf.Rad2Deg * Mathf.Atan((frontEndPos.x - rearEndPos.x) / (frontEndPos.z - rearEndPos.z)) + Invert_Angle;
			}
		}


		void Anchor_Control()
		{
			// Set the position. (Axis X = hight)
			invisiblePos.x = initialPosX + (Anchor_Transform.localPosition.x - anchorInitialPosX);

			if (Simple_Flag == true) {
				return;
			}
			// Calculate end positions.
			float tempRad = Rear_Script.invisibleAngY * Mathf.Deg2Rad;
			Vector3 rearEndPos = Rear_Script.invisiblePos + new Vector3 (Half_Length * Mathf.Sin (tempRad), 0.0f, Half_Length * Mathf.Cos (tempRad));
			tempRad = Front_Script.invisibleAngY * Mathf.Deg2Rad;
			Vector3 frontEndPos = Front_Script.invisiblePos - new Vector3 (Half_Length * Mathf.Sin (tempRad), 0.0f, Half_Length * Mathf.Cos (tempRad));
			// Set the angle.
			invisibleAngY = Mathf.Rad2Deg * Mathf.Atan ((frontEndPos.x - rearEndPos.x) / (frontEndPos.z - rearEndPos.z)) + Invert_Angle;
		}


		public void Start_Breaking ()
		{ // Called from "Damage_Control_03_Track_Collider_CS" in the TrackCollider.
			if (this.enabled == false) {
				return;
			}

			// Reset the rate values in the parent script.
			if (Parent_Script) {
				Parent_Script.Rate_L = 0.0f;
				Parent_Script.Rate_R = 0.0f;
			}

			// Add components into this piece.
			Add_Components (This_Transform);

			// Add components into other pieces.
			Static_Track_Piece_CS pieceScript = this;
			for (int i = 0; i < Pieces_Count; i++) {
				Add_Components (pieceScript.Front_Transform);
				pieceScript.Disable_and_Destroy ();
				pieceScript = pieceScript.Front_Script;
			}

			// Add "HingeJoint" into each front piece.
			pieceScript = this;
			for (int i = 0; i < Pieces_Count - 1; i++) { // "Pieces_Count - 1" >> Except for this piece.
				Add_HingeJoint (pieceScript.Front_Transform, pieceScript.Front_Script.Front_Transform);
				pieceScript = pieceScript.Front_Script;
			}
		}


		void Add_Components (Transform tempTransform)
		{
			// Add rigidbody.
			if (tempTransform.GetComponent <Rigidbody>() == null) {
				Rigidbody rigidbody = tempTransform.gameObject.AddComponent <Rigidbody>();
				rigidbody.mass = Parent_Script.Mass;
			}

			// Enable the colliders.
			Collider[] colliders = tempTransform.GetComponents <Collider>();
			for (int i = 0; i < colliders.Length; i++) {
				colliders [i].enabled = true;
			}
		}


		void Disable_and_Destroy ()
		{
			this.enabled = false;
			This_Transform.parent = null;
			Destroy (this.gameObject, 20.0f);
		}


		void Add_HingeJoint (Transform baseTransform, Transform connectedTransform)
		{
			HingeJoint hingeJoint = baseTransform.gameObject.AddComponent <HingeJoint>();
			hingeJoint.connectedBody = connectedTransform.GetComponent <Rigidbody>();
			hingeJoint.anchor = new Vector3 (0.0f, 0.0f, Half_Length);
			hingeJoint.axis = new Vector3 (1.0f, 0.0f, 0.0f);
			hingeJoint.breakForce = 200000.0f;
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
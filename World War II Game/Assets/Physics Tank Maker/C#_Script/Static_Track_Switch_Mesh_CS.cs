using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
	public class Static_Track_Switch_Mesh_CS : MonoBehaviour
	{
		/*
		 * This script switches the mesh of Static Track pieces while drivning.
		 * This script works in combination with "Static_Track_Parent_CS" in the parent object, and "Static_Track_Piece_CS" in the piece.
		*/

		// User options >>
		public Static_Track_Piece_CS Piece_Script;
		public Static_Track_Parent_CS Parent_Script;
		public MeshFilter This_MeshFilter;
		public Mesh Mesh_A;
		public Mesh Mesh_B;
		public bool Is_Left;
		// << User options

		Mesh currentMesh;


		void Start ()
		{
			currentMesh = Mesh_A;
		}
	

		void LateUpdate ()
		{
			if (Piece_Script.enabled == false) {
				return;
			}

			if (Is_Left) { // Left
				if (Parent_Script.Switch_Mesh_L) {
					currentMesh = Mesh_A;
				}
				else {
					currentMesh = Mesh_B;
				}
			}
			else { // Right
				if (Parent_Script.Switch_Mesh_R) {
					currentMesh = Mesh_A;
				}
				else {
					currentMesh = Mesh_B;
				}
			}
			This_MeshFilter.mesh = currentMesh;
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}
}

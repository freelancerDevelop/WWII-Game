using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	[ System.Serializable]
	public class IntArray
	{
		public int[] intArray;
		public IntArray (int[] newIntArray)
		{
			intArray = newIntArray;
		}
	}


	public class Track_Deform_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Scroll_Track" in the tank.
		 * This script controls the deforming of the Scroll_Track.
		*/


		// User options >>
		public int Anchor_Num = 1;
		public Transform[] Anchor_Array;
		public string [] Anchor_Names;
		public string [] Anchor_Parent_Names;
		public float[] Width_Array;
		public float[] Height_Array;
		public float[] Offset_Array;
		public float[] Initial_Pos_Array;
		public Vector3[] Initial_Vertices;
		public IntArray[] Movable_Vertices_List;
		// << User options


		Mesh thisMesh;
		MainBody_Setting_CS bodyScript;
		Vector3[] currentVertices;
		bool isVisible;


		void Start()
		{
			Initialize();
		}


		void Initialize ()
		{
			// Check the anchor wheels.
			for (int i = 0; i < Anchor_Array.Length; i++) {
				if (Anchor_Array [i] == null) {
					// Find the anchor wheel with reference to the name.
					if (string.IsNullOrEmpty (Anchor_Names [i]) == false && string.IsNullOrEmpty (Anchor_Parent_Names [i]) == false) {
						Anchor_Array [i] = transform.parent.Find (Anchor_Parent_Names [i] + "/" + Anchor_Names [i]);
					} else {
						Debug.LogError ("Anchor wheels of Scroll Track are not assigned.");
						Destroy (this);
						return;
					}
				}
			}

			thisMesh = GetComponent <MeshFilter>().mesh;
			currentVertices = new Vector3[Initial_Vertices.Length];
			bodyScript = GetComponentInParent <MainBody_Setting_CS>();
		}


		void Update ()
		{
			if (bodyScript) {
				isVisible = bodyScript.Visible_Flag; // The MainBody is visible or not by any camera.
			}
			else {
				isVisible = true;
			}

			if (isVisible) {
				Deform();
			}
		}


		void Deform()
		{
			Initial_Vertices.CopyTo (currentVertices, 0);
			float tempDist = 0.0f;
			for (int i = 0; i < Anchor_Array.Length; i++) {
				if (Anchor_Array[i] == null) {
					continue;
				}
				tempDist = Anchor_Array [i].localPosition.x - Initial_Pos_Array [i];
				for (int j = 0; j < Movable_Vertices_List [i].intArray.Length; j++) {
					currentVertices [Movable_Vertices_List [i].intArray [j]].y += tempDist;
				}
			}
			thisMesh.vertices = currentVertices;
		}


		void OnDrawGizmosSelected ()
		{
			if (Anchor_Array.Length != 0 && Offset_Array.Length != 0) {
				Gizmos.color = Color.green;
				for (int i = 0; i < Anchor_Array.Length; i++) {
					if (Anchor_Array [i] != null) {
						Vector3 tempSize = new Vector3 (0.0f, Height_Array [i], Width_Array [i]);
						Vector3 tempCenter = Anchor_Array [i].position;
						tempCenter.y += Offset_Array [i];
						Gizmos.DrawWireCube (tempCenter, tempSize);
					}
				}
			}

			MeshFilter thisFilter = GetComponent <MeshFilter> ();
			if (thisFilter && thisFilter.sharedMesh) {
				// Draw vertices.
				Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.localScale);
				Vector3[] vertices = thisFilter.sharedMesh.vertices;
				for (int i = 0; i < vertices.Length; i++) {
					Gizmos.color = Color.red;
					Gizmos.DrawSphere (vertices [i] + transform.position, 0.01f);
				}
			}
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
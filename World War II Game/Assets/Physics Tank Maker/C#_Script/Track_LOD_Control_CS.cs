using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Track_LOD_Control_CS : MonoBehaviour
	{
		/*
		 * This script controls the texture scrolling of Scroll_Track.
		*/


		// User options >>
		public GameObject Static_Track;
		public GameObject Scroll_Track_L;
		public GameObject Scroll_Track_R;
		public float Threshold = 15.0f;
		// << User options


		Transform thisTransform;
		MainBody_Setting_CS bodyScript;
		float frustumHeight;
		bool tankIsNear;
		bool isVisible;


		void Start ()
		{
			Initialize();
		}


		void Initialize ()
		{
			thisTransform = transform;
			bodyScript = GetComponent <MainBody_Setting_CS>();

			// Check the tracks.
			if (Static_Track == null || Scroll_Track_L == null || Scroll_Track_R == null) {
				Debug.LogWarning ("Track LOD system cannot work, because the tracks are not assigned.");
				Destroy (this);
				return;
			}

			// Set the tracks activations at the first time.
			frustumHeight = 2.0f * Vector3.Distance (thisTransform.position, Camera.main.transform.position) * Mathf.Tan (Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
			tankIsNear = (frustumHeight < Threshold);
			if (tankIsNear) {
				Static_Track.SetActive(true);
				Scroll_Track_L.SetActive(false);
				Scroll_Track_R.SetActive(false);
			}
			else {
				Static_Track.SetActive(false);
				Scroll_Track_L.SetActive(true);
				Scroll_Track_R.SetActive(true);
			}
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
				Tracks_LOD();
			}
		}


		void Tracks_LOD()
		{
			frustumHeight = 2.0f * Vector3.Distance (thisTransform.position, Camera.main.transform.position) * Mathf.Tan (Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
			if (tankIsNear) {
				if (frustumHeight > Threshold) { // The tank has been far from the camera.
					tankIsNear = false;
					Static_Track.SetActive(false);
					Scroll_Track_L.SetActive(true);
					Scroll_Track_R.SetActive(true);
				}
			}
			else {
				if (frustumHeight < Threshold) { // The tank has been near the camera.
					tankIsNear = true;
					Static_Track.SetActive(true);
					Scroll_Track_L.SetActive(false);
					Scroll_Track_R.SetActive(false);
				}
			}
		}


		void Track_Destroyed_Linkage ()
		{ // Called from "Damage_Control_Center_CS".
			// Activate the Static_Track.
			if (Static_Track) {
				Static_Track.SetActive (true);
			}

			// Remove the Scroll_Tracks.
			if (Scroll_Track_L) {
				Destroy (Scroll_Track_L);
			}
			if (Scroll_Track_R) {
				Destroy (Scroll_Track_R);
			}

			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}
}

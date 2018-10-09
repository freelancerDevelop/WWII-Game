using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Track_Scroll_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Scroll_Track" in the tank.
		 * This script controls the texture scrolling of the Scroll_Track.
		*/


		// User options >>
		public bool Is_Left;
		public Transform Reference_Wheel; // Referred to from "Static_Wheel_Parent_CS".
		public string Reference_Name;
		public string Reference_Parent_Name;
		public float Scroll_Rate = 0.005f;
		public string Tex_Name = "_MainTex";
		// << User options


		Material thisMaterial;
		public float Delta_Ang; // Referred to from "Static_Wheel_Parent_CS".
		float previousAng;
		Vector2 offset;
		MainBody_Setting_CS bodyScript;
		bool isVisible;


		void Start()
		{
			Initialize();
		}


		void Initialize ()
		{
			// Find the reference wheels.
			if (Reference_Wheel == null) {
				if (string.IsNullOrEmpty (Reference_Name) == false && string.IsNullOrEmpty (Reference_Parent_Name) == false) {
					Reference_Wheel = transform.parent.Find (Reference_Parent_Name + "/" + Reference_Name);
				}
			}
			if (Reference_Wheel == null) {
				Debug.LogError ("'Reference wheel' for Scroll_Track cannot be found.");
				Destroy (this);
				return;
			}

			// Send this reference to all the "Static_Wheel_Parent_CS" in the tank.
			Static_Wheel_Parent_CS[] staticWheelParentScripts = transform.parent.GetComponentsInChildren <Static_Wheel_Parent_CS>();
			for (int i = 0; i < staticWheelParentScripts.Length; i++) {
				staticWheelParentScripts[i].Prepare_With_Scroll_Track(this);
			}

			thisMaterial = GetComponent <Renderer>().material;
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
				Scroll();
			}
		}


		void Scroll()
		{
			float currentAng = Reference_Wheel.localEulerAngles.y;
			Delta_Ang = Mathf.DeltaAngle (currentAng, previousAng);
			offset.x += Scroll_Rate * Delta_Ang;
			thisMaterial.SetTextureOffset (Tex_Name, offset);
			previousAng = currentAng;
		}


		void Track_Destroyed_Linkage(bool isLeft)
		{ // Called from "Damage_Control_Center_CS".
			if (isLeft != Is_Left) { // The direction is different.
				return;
			}

			Destroy (gameObject);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
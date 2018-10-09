using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Emission_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Dust_#" objects in the tank.
		 * This script controls the particle emission used for generating the dust or exhaust from the tank.
		*/


		// User options >>
		public Rigidbody Reference_Rigidbody;
		public string Reference_Name;
		public string Reference_Parent_Name;
		public float Max_Velocity = 7.0f;
		public int Emission_Type = 1; // 0=Time, 1=Distance,.
		public AnimationCurve Curve;
		public float Max_Rate = 2.0f;
		public bool Adjsut_Alpha = true;
		public float Standard_Light_Intensity = 1.4f;
		// << User options

		ParticleSystem thisParticleSystem;
		ParticleSystem.EmissionModule emission;
		ParticleSystem.MinMaxCurve rate;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			thisParticleSystem = GetComponent <ParticleSystem> ();
			emission = thisParticleSystem.emission;
			rate = emission.rateOverDistance;

			// Find the reference rigidbody.
			Transform rootTransform = transform.root;
			if (Reference_Rigidbody == null) { // The reference rigidbody has been lost by modifying.
				if (string.IsNullOrEmpty(Reference_Name) == false && string.IsNullOrEmpty(Reference_Parent_Name) == false) {
					Transform bodyTransform = GetComponentInParent <Rigidbody>().transform;
					Transform referenceTransform = bodyTransform.Find (Reference_Parent_Name + "/" + Reference_Name);
						if (referenceTransform) {
							Reference_Rigidbody = referenceTransform.GetComponent <Rigidbody>();
						}
					}
				if (Reference_Rigidbody == null) {
						Debug.LogWarning("'Reference_Rigidbody' is not assigned in " + this.name + " in " + rootTransform.name);
						Destroy(this.gameObject);
						return;
					}
				}

			if (Adjsut_Alpha) {
				Adjust_Start_Color_Alpha ();
			}
		}


		void Adjust_Start_Color_Alpha ()
		{
			Light light = null;
			Light [] lights = FindObjectsOfType <Light> ();
			for (int i = 0; i < lights.Length; i++) {
				if (lights [i].type == LightType.Directional) {
					light = lights [i];
					break;
				}
			}
			if (light == null) {
				return;
			}

			Color tempColor = thisParticleSystem.main.startColor.color;
			tempColor.a = Mathf.Lerp (0.0f, tempColor.a, light.intensity / Standard_Light_Intensity);
			ParticleSystem.MainModule main = thisParticleSystem.main;
			main.startColor = tempColor;
		}


		void Update ()
		{
			if (Reference_Rigidbody == null) {
				return;
			}

			rate.constantMax = Curve.Evaluate (Reference_Rigidbody.velocity.magnitude / Max_Velocity) * Max_Rate;
			switch (Emission_Type) {
				case 0: // Time
					emission.rateOverTime = rate;
					break;
				case 1: // Distance
					emission.rateOverDistance = rate;
					break;
			}
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
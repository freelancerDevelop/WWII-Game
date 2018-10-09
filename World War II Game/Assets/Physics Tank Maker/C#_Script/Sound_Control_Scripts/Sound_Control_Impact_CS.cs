using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	[ RequireComponent (typeof(AudioSource))]

	public class Sound_Control_Impact_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the MainBody in the tank.
		 * This script controls the impact sound of the tank.
		*/


		// User options >>
		public float Min_Impact = 1.0f;
		public float Max_Impact = 3.0f;
		public float Min_Impact_Pitch = 0.5f;
		public float Max_Impact_Pitch = 1.0f;
		public float Min_Impact_Volume = 0.5f;
		public float Max_Impact_Volume = 1.0f;
		// << User options


		float currentVelocity;
		float previousVelocity;
		bool isPrepared = true;
		Rigidbody bodyRigidbody;
		float clipLength;

		AudioSource thisAudioSource;

		bool isSelected;


		void Start() {
			Initial_Settings();
		}


		void Initial_Settings()
		{
			thisAudioSource = GetComponent <AudioSource>();
			thisAudioSource.playOnAwake = false;
			thisAudioSource.loop = false;
			clipLength = thisAudioSource.clip.length;
			bodyRigidbody = GetComponent<Rigidbody>();
		}


		void Update ()
		{
			if (isSelected) {
				Impact_Sound();
			}
		}


		void Impact_Sound ()
		{
			currentVelocity = bodyRigidbody.velocity.y;
			if (isPrepared) {
				float impact = Mathf.Abs (previousVelocity - currentVelocity);
				if (impact > Min_Impact) {
					StartCoroutine ("Produce_Impact_Sound", impact);
				}
			}
			previousVelocity = currentVelocity;
		}


		IEnumerator Produce_Impact_Sound (float impact)
		{
			isPrepared = false;
			float rate = impact / Max_Impact;
			thisAudioSource.pitch = Mathf.Lerp (Min_Impact_Pitch, Max_Impact_Pitch, rate);
			thisAudioSource.volume = Mathf.Lerp (Min_Impact_Volume, Max_Impact_Volume, rate);
			thisAudioSource.Play ();
			yield return new WaitForSeconds (clipLength);
			isPrepared = true;
		}


		void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			thisAudioSource.Stop ();
			Destroy (this);
		}


		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			this.isSelected = isSelected;
		}

	}

}
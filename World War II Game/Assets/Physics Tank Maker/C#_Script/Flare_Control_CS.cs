using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Flare_Control_CS : MonoBehaviour
	{
		/*
		 * This script controls the flares placed in some demo scenes.
		*/


		// User options >>
		public float Min_Interval = 1.0f;
		public float Max_Interval = 10.0f;
		// << User options

		ParticleSystem thisParticleSystem;
		float lifeTime;
		Light thisLight;
		float targetIntensity;
		float currentIntensity;
		bool isPrepared = true;


		void Start()
		{
			Initialize();
		}


		void Initialize ()
		{
			thisParticleSystem = GetComponent <ParticleSystem>();
			if (thisParticleSystem) {
				lifeTime = thisParticleSystem.main.startLifetime.constant;
			}
			else {
				Destroy (this);
			}

			thisLight = GetComponent <Light>();
			if (thisLight) {
				targetIntensity = thisLight.intensity;
				thisLight.intensity = 0.0f;
			}
			else {
				Destroy (this);
			}
		}


		void Update ()
		{
			if (isPrepared) {
				isPrepared = false;
				StartCoroutine ("Light_Control");
			}
		}


		IEnumerator Light_Control ()
		{
			yield return new WaitForSeconds (Random.Range (Min_Interval, Max_Interval));

			thisParticleSystem.Play ();

			// Increase the light intensity.
			currentIntensity = 0.0f;
			while (currentIntensity < targetIntensity) {
				currentIntensity = Mathf.MoveTowards (currentIntensity, targetIntensity, targetIntensity * Time.deltaTime * 2.0f);
				thisLight.intensity = currentIntensity;
				yield return null;
			}

			// Decrease the light intensity.
			while (currentIntensity > 0.0f) {
				currentIntensity = Mathf.MoveTowards (currentIntensity, 0.0f, targetIntensity / lifeTime * Time.deltaTime);
				thisLight.intensity = currentIntensity;
				yield return null;
			}

			isPrepared = true;
		}

	}

}
using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Particle_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the explosion effects prefabs.
		 * This script controls the sound and the light in the effects.
		*/


		// User options >>
		public bool Use_Random_Pitch;
		public float Random_Pitch_Min = 0.5f;
		public float Random_Pitch_Max = 1.0f;
		// << User options


		Transform thisTransform;
		ParticleSystem thisParticleSystem;
		AudioSource audioSource;
		Light thisLight;


		void Start()
		{
			thisTransform = transform;
			thisParticleSystem = GetComponent <ParticleSystem>();

			audioSource = GetComponent <AudioSource>();
			if (audioSource) {
				audioSource.playOnAwake = false;
			}

			thisLight = GetComponent < Light > ();
			if (thisLight) {
				StartCoroutine ("Flash");
			}

			if (audioSource && Camera.main) {
				// Get the distance to the camera.
				float distance = Vector3.Distance (thisTransform.position, Camera.main.transform.position);
				if (Use_Random_Pitch) {
					// Change the pitch randomly.
					audioSource.pitch = Random.Range (Random_Pitch_Min, Random_Pitch_Max);
				}
				else {
					// Change the pitch according to the distance to the camera. 
					audioSource.pitch = Mathf.Lerp (1.0f, 0.1f, distance / audioSource.maxDistance);
				}

				// Delay playing the sound according to the distance to the camera. 
				audioSource.PlayDelayed (distance / 340.29f * Time.timeScale);
			}
		}


		void Update ()
		{
			if (thisParticleSystem.isStopped) {
				if (audioSource && audioSource.isPlaying) {
					return;
				}
				Destroy (this.gameObject);
			}
		}


		IEnumerator Flash ()
		{
			thisLight.enabled = true;
			yield return new WaitForSeconds (0.08f);
			thisLight.enabled = false;
		}


		IEnumerator Flash_HQ ()
		{
			thisLight.enabled = true;
			float count = 0.0f;
			while (count < 0.2f) {
				thisLight.range = Mathf.Sin (count / 0.2f * 3.14f) * 30.0f;
				count += Time.deltaTime;
				yield return null;
			}
			thisLight.enabled = false;
		}

	}

}
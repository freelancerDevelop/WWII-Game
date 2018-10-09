using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace ChobiAssets.PTM
{
	
	[ RequireComponent (typeof(Image))]
	public class UI_PosMarker_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to "Pos_Marker" object in the scene.
		 * This script controls the appearance of the position-marker such as the color, position and angle.
		 * This script works in combination with "ID_Settings_CS" and "AI_CS" in the tank.
		*/


		// User options >>
		public Color Friend_Color = Color.blue;
		public Color Hostile_Color = Color.red;
		public Color Landmark_Color = Color.green;
		public float Defensive_Alpha = 0.25f;
		public float Offensive_Alpha = 1.0f;
		public float Upper_Offset = 14.0f;
		public float Side_Offset = 28.0f;
		public bool Show_All = true;
		// << User options


		Transform thisTransform;
		Image thisImage;
		CanvasScaler canvasScaler;
		ID_Settings_CS idScript;
		Transform rootTransform;
		Transform bodyTransform;
		AI_CS aiScript;
		Quaternion leftRot = Quaternion.Euler (new Vector3 (0.0f, 0.0f, -90.0f));
		Quaternion rightRot = Quaternion.Euler (new Vector3 (0.0f, 0.0f, 90.0f));


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			thisTransform = transform;
			thisImage = GetComponent <Image>();
			canvasScaler = thisTransform.GetComponentInParent <CanvasScaler>();
		}


		void Get_Tank_Components()
		{ // This function is called at the fisrst time, and also when the tank is respawned.
			rootTransform = idScript.transform.root;
			bodyTransform = idScript.GetComponentInChildren <Rigidbody>().transform;
			aiScript = idScript.GetComponentInChildren <AI_CS>();
		}


		void LateUpdate ()
		{
			if (idScript == null) { // The tank has been removed from the scene.
				Destroy (this.gameObject);
				return;
			}

			if (bodyTransform == null) { // The tank has been respawned.
				Get_Tank_Components();
			}

			Set_Enabled_And_Color();

			if (thisImage.enabled == true) {
				Set_Position();
			}
		}


		void Set_Enabled_And_Color()
		{
			if (idScript.Is_Selected || rootTransform.tag == "Finish") { // The tank is selected now, or has been dead.
				thisImage.enabled = false;
				return;
			}

			switch (idScript.Relationship) {
				case 0: // Friendly.
					thisImage.enabled = true;
					if (aiScript) { // AI tank.
						// Set the alpha.
						switch (aiScript.Action_Type) {
							case 0: // Defensive.
								Friend_Color.a = Defensive_Alpha;
								break;
							case 1: // Offensive.
								Friend_Color.a = Offensive_Alpha;
								break;
						}
						thisImage.color = Friend_Color;
					}
					else { // Not AI tank.
						thisImage.enabled = true;
						thisImage.color = Friend_Color;
					}
					break;

				case 1: // Hostile.
					if (aiScript) { // AI tank.
						// Set the alpha.
						switch (aiScript.Action_Type) {
							case 0: // Defensive.
								if (Show_All) { // The marker is always displayed.
									thisImage.enabled = true;
									Hostile_Color.a = Defensive_Alpha;
									thisImage.color = Hostile_Color;
								}
								else { // The marker is not displayed when AI is defensive.
									thisImage.enabled = false;
								}
								break;
							case 1: // Offensive.
								thisImage.enabled = true;
								// Set the alpha.
								Hostile_Color.a = Offensive_Alpha;
								thisImage.color = Hostile_Color;
								break;
						}
					}
					else { // Not AI tank.
						thisImage.enabled = true;
						thisImage.color = Hostile_Color;
					}
					break;

				case 2: // Landmark.
					thisImage.enabled = true;
					thisImage.color = Landmark_Color;
					break;
			}
		}


		void Set_Position ()
		{
			float resolutionOffset = Screen.width / canvasScaler.referenceResolution.x;
			float dist = Vector3.Distance (Camera.main.transform.position, bodyTransform.position);
			Vector3 currentPos = Camera.main.WorldToScreenPoint (bodyTransform.position);
			if (currentPos.z > 0.0f) { // In front of the camera.
				currentPos.z = 100.0f;
				if (currentPos.x < 0.0f) { // Over the left end.
					currentPos.x = Side_Offset * resolutionOffset;
					currentPos.y = Screen.height * Mathf.Lerp (0.2f, 0.9f, dist / 500.0f);
					thisTransform.localRotation = leftRot;
				}
				else if (currentPos.x > Screen.width) { // Over the right end.
					currentPos.x = Screen.width - (Side_Offset * resolutionOffset);
					currentPos.y = Screen.height * Mathf.Lerp (0.2f, 0.9f, dist / 500.0f);
					thisTransform.localRotation = rightRot;
				}
				else { // Within the screen.
					currentPos.y = Screen.height - (Upper_Offset * resolutionOffset);
					thisTransform.localRotation = Quaternion.identity;
				}
			}
			else { // Behind of the camera.
				currentPos.z = -100.0f;
				if (currentPos.x > Screen.width * 0.5f) { // Left side.
					currentPos.x = Side_Offset * resolutionOffset;
					thisTransform.localRotation = leftRot;
				}
				else { // Right side.
					currentPos.x = Screen.width - (Side_Offset * resolutionOffset);
					thisTransform.localRotation = rightRot;
				}
				currentPos.y = Screen.height * Mathf.Lerp (0.2f, 0.9f, dist / 500.0f);
			}
			thisTransform.position = currentPos;
		}


		public void Receive_ID_Script (ID_Settings_CS tempIDScript)
		{ // Called from "ID_Settings_CS" in the top object of the tank.
			if (idScript == null) { // This marker is not used yet.
				idScript = tempIDScript;
				Get_Tank_Components();
			}
			else { // This marker is already used by other tank.
				// Duplicate this object.
				GameObject newMarkerObject = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity, transform.parent) as GameObject;
				newMarkerObject.name = this.name;
				UI_PosMarker_Control_CS newMarkerScript = newMarkerObject.GetComponent <UI_PosMarker_Control_CS>();
				newMarkerScript.idScript = tempIDScript;
				newMarkerScript.Get_Tank_Components();
			}
		}

	}

}
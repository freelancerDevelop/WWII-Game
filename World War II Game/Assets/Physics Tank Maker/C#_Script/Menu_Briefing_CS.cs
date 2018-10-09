using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{
	
	public class Menu_Briefing_CS : MonoBehaviour
	{
		/*
		 * This script controls the briefing text in the menu scenes.
		*/

		public float scrollSpeed = 256.0f;


		RectTransform thisTransform;
		CanvasScaler canvasScaler;
		float scaleOffset;

		void Start()
		{
			thisTransform = GetComponent <RectTransform>();
			canvasScaler = GetComponentInParent <CanvasScaler>();

			// Set the first position.
			Vector3 currentPos = thisTransform.position;
			Calculate_Scale_Offset();
			float endPosX = Calculate_End_Pos();
			currentPos.x = Screen.width + endPosX;
			thisTransform.position = currentPos;
		}


		void Update ()
		{
			// Move the position.
			Vector3 currentPos = thisTransform.position;
			Calculate_Scale_Offset();
			currentPos.x -= scrollSpeed * Time.deltaTime * scaleOffset;

			// Check the position.
			float endPosX = Calculate_End_Pos();
			if (currentPos.x < -endPosX) {
				currentPos.x = Screen.width + endPosX;
			}

			// Set the position.
			thisTransform.position = currentPos;
		}


		void Calculate_Scale_Offset()
		{
			if (canvasScaler) {
				scaleOffset = Screen.width / canvasScaler.referenceResolution.x;
			}
			else {
				scaleOffset = 1.0f;
			}
		}


		float Calculate_End_Pos()
		{
			float endPosX = (thisTransform.rect.width * scaleOffset) * 0.5f;
			return endPosX;
		}
	
	}
}

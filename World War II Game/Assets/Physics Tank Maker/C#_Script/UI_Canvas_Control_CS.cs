using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class UI_Canvas_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to a Canvas in the scene, and turns on / off it by pressing Delete key.
		 * In the demo scenes, this script is attached to "Canvas_States" for displaying the AI tanks state.
		*/

		Canvas thisCanvas;
		bool isEnabled;


		void Start ()
		{
			thisCanvas = GetComponent <Canvas>();
			isEnabled = thisCanvas.enabled;
		}


		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Delete)) {
				isEnabled = !isEnabled;
				thisCanvas.enabled = isEnabled;
			}
		}
	}

}
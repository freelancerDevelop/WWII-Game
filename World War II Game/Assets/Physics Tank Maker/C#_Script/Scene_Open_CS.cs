using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{
	
	public class Scene_Open_CS : MonoBehaviour
	{
		/*
		 * This script is attached to buttons in the scene for opening the specified scene.
		*/


		// User options >>
		public string Scene_Name;
		// << User options


		public void Button_Push ()
		{ // Called from the button.
			// Reset the time speed to return from a pause.
			Time.timeScale = 1.0f;

			// Disable all the button.
			Button thisButton = GetComponent <Button> ();
			thisButton.enabled = false;
			Button [] buttons = FindObjectsOfType <Button> ();
			for (int i = 0; i < buttons.Length; i++) {
				Button button = buttons [i];
				if (button != thisButton) {
					if (button.targetGraphic) {
						button.targetGraphic.enabled = false;
					}
					Text tempText = button.GetComponentInChildren<Text> ();
					if (tempText) {
						tempText.enabled = false;
					}
					button.enabled = false;
				}
			}

			StartCoroutine ("Open_Scene");
		}


		IEnumerator Open_Scene()
		{
			Fade_Control_CS fadeScript = FindObjectOfType <Fade_Control_CS>();
			if (fadeScript) {
				fadeScript.StartCoroutine("Fade_Out");
				yield return new WaitForSeconds(fadeScript.Fade_Time);
			}
			SceneManager.LoadScene (Scene_Name);
		}

	}

}

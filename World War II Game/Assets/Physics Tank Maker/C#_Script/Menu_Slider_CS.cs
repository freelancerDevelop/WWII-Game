using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	[ RequireComponent (typeof(Slider))]
	public class Menu_Slider_CS : MonoBehaviour {
		/*
		 * This script is attached to the slider for adjusting the 'Attack Multiplier' value in the menu scene opend before the battle scene.
		 * This script is used for creating the dictionary used for overwriting the 'Attack Multiplier' value of the tanks in the battle scene by "Menu_Dictionary_CS".
		*/

		// User options >>
		public Slider This_Slider;
		public Text Value_Text;
		public string Key_Name = "";
		public int Type; // 0 = Attack_Multiplier, 1 = Defence_Multiplier.
		public float Initial_Value;
		// << User options


		// Set by "Menu_Dictionary_CS" in the menu scene.
		public Menu_Dictionary_CS Dictionary_Script;


		void Awake()
		{ // The slider values must be prepared before "OnSceneLoaded()", because the "Menu_Dictionary_CS" will use the values in the "OnSceneLoaded()".
			if (This_Slider == null) {
				This_Slider = GetComponent <Slider>();
			}
			// Set the initial value.
			On_Value_Changed(Initial_Value);
		}


		public void On_Value_Changed (float value)
		{ // Called from the slider, when the value has been changed.
			This_Slider.value = Mathf.Round (value * 10.0f) / 10.0f;
			if (Value_Text) {
				Value_Text.text = This_Slider.value.ToString ();
			}

			// Send the new value to "Menu_Dictionary_CS".
			if (Dictionary_Script) {
				Dictionary_Script.Slider_Value_Changed(this);
			}
		}
	}

}

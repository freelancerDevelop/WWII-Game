using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	[ RequireComponent (typeof(Dropdown))]
	public class Menu_Dropdown_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the dropdown for selecting the tank prefab in the menu scene opend before the battle scene.
		 * This script is used for creating the dictionary used for changing the tanks in the battle scene by "Menu_Dictionary_CS".
		*/

		// User options >>
		public Dropdown This_Dropdown;
		public Text Title_Text;
		public string Key_Name = "";
		public int Num;
		public GameObject[] Prefabs_Array;
		public int Default_Value = 0;
		public Transform Symbol_Transform;
		public float Offset;
		// << User options


		public Menu_Dictionary_CS Dictionary_Script; // Set by "Menu_Dictionary_CS".

		Transform thisTransform;


		void Awake()
		{ // The dropdown values must be prepared before "OnSceneLoaded()", because the "Menu_Dictionary_CS" will use the values in the "OnSceneLoaded()".
			thisTransform = transform;
			This_Dropdown = GetComponent <Dropdown> ();

			// Set the dropdown.
			This_Dropdown.ClearOptions ();
			for (int i = 0; i < Prefabs_Array.Length; i++) {
				GameObject prefab = Prefabs_Array [i];
				if (prefab) {
					This_Dropdown.options.Add (new Dropdown.OptionData { text = prefab.name });
				} else {
					This_Dropdown.options.Add (new Dropdown.OptionData { text = "Empty" });
				}
			}
			This_Dropdown.RefreshShownValue();

			// Set the initial selection.
			if (Prefabs_Array.Length > Default_Value) {
				This_Dropdown.value = Default_Value;
			}

			// Set the title text.
			if (Title_Text) {
				Title_Text.text = this.name;
			}
		}
		

		public void On_Value_Changed ()
		{ // Called from the dropdown, when the value has been changed.
			// Send the new value to "Menu_Dictionary_CS".
			if (Dictionary_Script) {
				Dictionary_Script.Tank_Dropdown_Value_Changed(this);
			}
		}


		void LateUpdate ()
		{
			// Set the position.
			if (Symbol_Transform) {
				Vector3 tempPos = Camera.main.WorldToScreenPoint (Symbol_Transform.position + Symbol_Transform.forward * Offset);
				thisTransform.position = tempPos;
			}
		}

	}
}

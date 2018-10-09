using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ChobiAssets.PTM
{

	public class Menu_Dictionary_CS : MonoBehaviour
	{
		/*
		 * This script creates the dictionaries of selected tank prefabs and Attack_Multiplier values in the menu scene, and sends them to the next battle scene.
		 * The dictionaries are made with "Menu_Dropdown_CS" and "Menu_Slider_CS" in the menu scene.
		 * The dictionaries are used for overwriting the values of "Event_Controller_CS" scripts for spawning the tanks in the battle scene.
		 * 
		 * This script must be put in the menu scenes opend before the battle scene. And this object moves to the next scenes by using 'DontDestroyOnLoad()'.
		*/

		// User options >>
		public string Battle_Scene_Name;
		// << User options


		Dictionary <string, GameObject> selectedTankDictionary;
		Dictionary <string, float> attackMultiplierDictionary;
		Dictionary <string, float> defenceMultiplierDictionary;
		string birthplaceSceneName;


		void Awake()
		{ // This function is called only once at the first time. It is not called after the this object moves to other scenes by using 'DontDestroyOnLoad()'.
			string currentSceneName = SceneManager.GetActiveScene().name;

			// Find another "Menu_Dictionary_CS" in the scene, and check the birthplace scene.
			Menu_Dictionary_CS[] dictionaryScripts = FindObjectsOfType <Menu_Dictionary_CS>();
			for (int i = 0; i < dictionaryScripts.Length; i++) {
				if (dictionaryScripts[i] == this) {
					continue;
				}
				// Check the birthplace scene of another one.
				if (dictionaryScripts[i].birthplaceSceneName == currentSceneName) { // Another one should have been put in the current scene. >> Another one should have returned from the battle scene.
					Destroy(this.gameObject); // This GameObject should be useless.
					return;
				}
			} // The another "Menu_Dictionary_CS" is not found, or it is not suitable for the current scene. >> It will destroy itself in "OnSceneLoaded()".

			// Store the name of the birthplace scene.
			birthplaceSceneName = currentSceneName;

			// Keep this object even if the scene has been changed.
			DontDestroyOnLoad(this.gameObject);
		}


		void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}


		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}


		void OnSceneLoaded (Scene scene, LoadSceneMode sceneMode)
		{ // This function is called after "Awake()" at the first time, and each time the scnene has been changed.
			// Check the current scene is related to this script.
			string currentSceneName = scene.name;

			if (currentSceneName == birthplaceSceneName) {  // The current scene is this birthplace scene.
				// Create new dictionaries, or set the values of the dropdowns and sliders.
				Control_Menu_Components();
				return;
			}

			if (currentSceneName == Battle_Scene_Name) { // The current scene is the specified battle scene.
				// Overwrite the values in "Event_Controller_CS" scripts in the scene.
				Overwrite_Values();
				return;
			}

			// The current scene is not related to this script.
			Destroy(this.gameObject); // This GameObject should be useless.
		}


		void Control_Menu_Components()
		{
			// For dropdowns used for selecting the tanks in the scene.
			if (selectedTankDictionary == null) { // The "selectedTankDictionary" has not been created yet.
				// Create the disctionary.
				Create_Tank_Dictionary();
			}
			else { // The "selectedTankDictionary" has already been created. >> This GameObject must have returned to the menu scene from the battle scene.
				// Set the default value to the stored value in the dictionary.
				Set_Tank_Dropdown();
			}

			// For sliders used for changing "Attack_Multiplier" value.
			if (attackMultiplierDictionary == null) { // The "attackMultiplierDictionary" has not been created yet.
				// Create the disctionary.
				Create_Attack_Multiplier_Dictionary();
			}
			else { // The "attackMultiplierDictionary" has already been created. >> This GameObject must have returned to the menu scene from the battle scene.
				// Set the default value to the stored value in the dictionary.
				Set_Attack_Multiplier_Slider();
			}

			// For sliders used for changing "Defence_Multiplier" value.
			if (defenceMultiplierDictionary == null) { // The "defenceMultiplierDictionary" has not been created yet.
				// Create the disctionary.
				Create_Defence_Multiplier_Dictionary();
			}
			else { // The "defenceMultiplierDictionary" has already been created. >> This GameObject must have returned to the menu scene from the battle scene.
				// Set the default value to the stored value in the dictionary.
				Set_Defence_Multiplier_Slider();
			}
		}


		void Create_Tank_Dictionary()
		{
			// Find all the "Menu_Dropdown_CS" in the scene.
			Menu_Dropdown_CS[] dropdownScripts = GameObject.FindObjectsOfType <Menu_Dropdown_CS>();
			if (dropdownScripts.Length == 0) {
				return;
			}

			// Create "selectedTankDictionary".
			selectedTankDictionary = new Dictionary <string, GameObject>();
			for (int i = 0; i < dropdownScripts.Length; i++) {
				if (string.IsNullOrEmpty(dropdownScripts[i].Key_Name)) { // The "Key_Name" is not assigned in the "Menu_Dropdown_CS".
					// Assign its name to the "Key_Name".
					dropdownScripts[i].Key_Name = dropdownScripts[i].name;
				}
				selectedTankDictionary.Add (dropdownScripts[i].Key_Name, dropdownScripts[i].Prefabs_Array[dropdownScripts[i].This_Dropdown.value]);

				// Send this reference to the "Menu_Dropdown_CS".
				dropdownScripts[i].Dictionary_Script = this;
			}
		}


		void Create_Attack_Multiplier_Dictionary()
		{
			// Find all the "Menu_Slider_CS" in the scene.
			Menu_Slider_CS[] sliderScripts = GameObject.FindObjectsOfType <Menu_Slider_CS>();
			if (sliderScripts.Length == 0) {
				return;
			}

			// Create "attackMultiplierDictionary".
			attackMultiplierDictionary = new Dictionary <string, float>();
			for (int i = 0; i < sliderScripts.Length; i++) {
				if (sliderScripts[i].Type != 0) { // The slider is not for Attack_Multiplier.
					continue;
				}

				if (string.IsNullOrEmpty(sliderScripts[i].Key_Name)) { // The "Key_Name" is not assigned in the "Menu_Slider_CS".
					// Assign its name to the "Key_Name".
					sliderScripts[i].Key_Name = sliderScripts[i].name;
				}
				attackMultiplierDictionary.Add (sliderScripts[i].Key_Name, sliderScripts[i].This_Slider.value);

				// Send this reference to "Menu_Slider_CS".
				sliderScripts[i].Dictionary_Script = this;
			}
		}


		void Create_Defence_Multiplier_Dictionary()
		{
			// Find all the "Menu_Slider_CS" in the scene.
			Menu_Slider_CS[] sliderScripts = GameObject.FindObjectsOfType <Menu_Slider_CS>();
			if (sliderScripts.Length == 0) {
				return;
			}

			// Create "defenceMultiplierDictionary".
			defenceMultiplierDictionary = new Dictionary <string, float>();
			for (int i = 0; i < sliderScripts.Length; i++) {
				if (sliderScripts[i].Type != 1) { // The slider is not for Defence_Multiplier.
					continue;
				}

				if (string.IsNullOrEmpty(sliderScripts[i].Key_Name)) { // The "Key_Name" is not assigned in the "Menu_Slider_CS".
					// Assign its name to the "Key_Name".
					sliderScripts[i].Key_Name = sliderScripts[i].name;
				}
				defenceMultiplierDictionary.Add (sliderScripts[i].Key_Name, sliderScripts[i].This_Slider.value);

				// Send this reference to "Menu_Slider_CS".
				sliderScripts[i].Dictionary_Script = this;
			}
		}


		void Set_Tank_Dropdown()
		{
			// Set the value of "Menu_Dropdown_CS" in the scene.
			Menu_Dropdown_CS[] dropdownScripts = GameObject.FindObjectsOfType <Menu_Dropdown_CS>();
			for (int i = 0; i < dropdownScripts.Length; i++) {
				// Find the index of "Prefabs_Array" that matches the prefab in the dictionary.
				if (string.IsNullOrEmpty(dropdownScripts[i].Key_Name)) { // The "Key_Name" is not assigned in the "Menu_Dropdown_CS".
					// Assign its name to the "Key_Name".
					dropdownScripts[i].Key_Name = dropdownScripts[i].name;
				}
				for (int j = 0; j < dropdownScripts[i].Prefabs_Array.Length; j++) {
					if (dropdownScripts[i].Prefabs_Array[j] == selectedTankDictionary[dropdownScripts[i].Key_Name]) { // The both tank prefabs are the same.
						// Set the dropdown value.
						dropdownScripts[i].This_Dropdown.value = j;
						continue;
					}
				}

				// Send this reference.
				dropdownScripts[i].Dictionary_Script = this;
			}
		}


		void Set_Attack_Multiplier_Slider()
		{
			// Set the value in the "Menu_Slider_CS" in the scene.
			Menu_Slider_CS[] sliderScripts = GameObject.FindObjectsOfType <Menu_Slider_CS>();
			for (int i = 0; i < sliderScripts.Length; i++) {
				if (sliderScripts[i].Type != 0) { // The slider is not for Attack_Multiplier.
					continue;
				}

				// Set the slider value.
				if (string.IsNullOrEmpty(sliderScripts[i].Key_Name)) { // The "Key_Name" is not assigned in the "Menu_Slider_CS".
					// Assign its name to the "Key_Name".
					sliderScripts[i].Key_Name = sliderScripts[i].name;
				}
				sliderScripts[i].This_Slider.value = attackMultiplierDictionary[sliderScripts[i].Key_Name];

				// Send this reference.
				sliderScripts[i].Dictionary_Script = this;
			}
		}


		void Set_Defence_Multiplier_Slider()
		{
			// Set the value in the "Menu_Slider_CS" in the scene.
			Menu_Slider_CS[] sliderScripts = GameObject.FindObjectsOfType <Menu_Slider_CS>();
			for (int i = 0; i < sliderScripts.Length; i++) {
				if (sliderScripts[i].Type != 1) { // The slider is not for Defence_Multiplier.
					continue;
				}

				// Set the slider value.
				if (string.IsNullOrEmpty(sliderScripts[i].Key_Name)) { // The "Key_Name" is not assigned in the "Menu_Slider_CS".
					// Assign its name to the "Key_Name".
					sliderScripts[i].Key_Name = sliderScripts[i].name;
				}
				sliderScripts[i].This_Slider.value = defenceMultiplierDictionary[sliderScripts[i].Key_Name];

				// Send this reference.
				sliderScripts[i].Dictionary_Script = this;
			}
		}


		public void Tank_Dropdown_Value_Changed(Menu_Dropdown_CS dropdownScript)
		{ // Called from "Menu_Dropdown_CS", when the dropdown value has been changed.
			// Set the new value into the "selectedTankDictionary".
			if (selectedTankDictionary.ContainsKey(dropdownScript.Key_Name)) {
				selectedTankDictionary[dropdownScript.Key_Name] = dropdownScript.Prefabs_Array[dropdownScript.This_Dropdown.value];
			}
		}


		public void Slider_Value_Changed(Menu_Slider_CS sliderScript)
		{ // Called from "Menu_Slider_CS", when the slider value has been changed.
			switch (sliderScript.Type) {
				case 0: // Attack_Multiplier
					// Set the new value into the "attackMultiplierDictionary".
					if (attackMultiplierDictionary.ContainsKey(sliderScript.Key_Name)) {
						attackMultiplierDictionary[sliderScript.Key_Name] = sliderScript.This_Slider.value;
					}
					break;

				case 1: // Defence_Multiplier
					// Set the new value into the "defenceMultiplierDictionary".
					if (defenceMultiplierDictionary.ContainsKey(sliderScript.Key_Name)) {
						defenceMultiplierDictionary[sliderScript.Key_Name] = sliderScript.This_Slider.value;
					}
					break;
			}
		}


		void Overwrite_Values()
		{
			// Overwrite values in "Event_Controller_CS" in the battle scene.
			Event_Controller_CS[] eventScripts = FindObjectsOfType <Event_Controller_CS>();
			for (int i = 0; i < eventScripts.Length; i++) {
				if (eventScripts[i].Event_Type == 0) { // Spawn Tank
					// Overwrite "Prefab_Object".
					if (selectedTankDictionary != null && selectedTankDictionary.ContainsKey(eventScripts[i].Key_Name)) {
						eventScripts[i].Prefab_Object = selectedTankDictionary[eventScripts[i].Key_Name];
					}

					// Overwrite "Attack_Multiplier".
					if (attackMultiplierDictionary != null && attackMultiplierDictionary.ContainsKey(eventScripts[i].Key_Name)) {
						eventScripts[i].Attack_Multiplier = attackMultiplierDictionary[eventScripts[i].Key_Name];
					}

					// Overwrite "Defence_Multiplier".
					if (defenceMultiplierDictionary != null && defenceMultiplierDictionary.ContainsKey(eventScripts[i].Key_Name)) {
						eventScripts[i].Defence_Multiplier = defenceMultiplierDictionary[eventScripts[i].Key_Name];
					}
				}
			}
		}

	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{
	
	public class Option_Manager_CS : MonoBehaviour
	{
		/*
		 * This script overwrites the values of "Input_Type_Manager_CS" in the scene when the scene has chanaged.
		 * Usually, this script is put in the title scene and menu scenes opend before the battle scene. And this object moves to the next scenes by using 'DontDestroyOnLoad()'.
		*/

		// User options >>
		public int Input_Type = 0;
		public bool Use_Auto_Lead = false;
		public Dropdown Input_Dropdown;
		public Toggle Lead_Toggle;
		// << User options


		void Awake()
		{ // This function is called only once at the first time. It is not called after the this object moves to other scenes by using 'DontDestroyOnLoad()'.
			// Find the old one in the scene.
			Option_Manager_CS[] optionScripts = FindObjectsOfType <Option_Manager_CS>();
			for (int i = 0; i < optionScripts.Length; i++) {
				if (optionScripts [i] != this) {
					// Copy the values from the old one.
					Input_Type = optionScripts [i].Input_Type;
					Use_Auto_Lead = optionScripts [i].Use_Auto_Lead;

					// Destroy the old one.
					Destroy (optionScripts [i].gameObject);
				}
			}

			// Keep this object even if the scene has been changed.
			DontDestroyOnLoad (this.gameObject);
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
		{ // This function is called after "Awake()" at the first time, and when the scnene has been changed.
			// Find "Input_Type_Manager_CS" in the scene, and overwrite the values.
			Input_Type_Manager_CS managerScript = FindObjectOfType <Input_Type_Manager_CS>();
			if (managerScript) {
				// Set values in the "Game_Controller".
				managerScript.Input_Type = Input_Type;
				managerScript.Use_Auto_Lead = Use_Auto_Lead;
			}

			// Set the dropdown for "Input_Type".
			if (Input_Dropdown) {
				Input_Dropdown.value = Input_Type;
			}

			// Set the toggle for "Use_Auto_Lead".
			if (Lead_Toggle) {
				Lead_Toggle.isOn = Use_Auto_Lead;
			}
		}


		public void On_Value_Changed_Input (int value)
		{ // Called from the dropdown for "Input_Type".
			Input_Type = value;
		}
	

		public void On_Value_Changed_Lead (bool toggle)
		{ // Called from the toggle for "Use_Auto_Lead".
			Use_Auto_Lead = toggle;
		}
	
	}

}

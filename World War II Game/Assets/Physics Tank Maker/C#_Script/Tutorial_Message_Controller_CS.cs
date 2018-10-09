using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChobiAssets.PTM
{
	
	public class Tutorial_Message_Controller_CS : MonoBehaviour {


		public Event_Controller_CS[] MessageEvents_0_1;
		public Event_Controller_CS[] MessageEvents_2;
		public Event_Controller_CS[] MessageEvents_3;


		void Start ()
		{
			var inputTypeManagerScript =  FindObjectOfType <Input_Type_Manager_CS>();
			if (inputTypeManagerScript == null) {
				return;
			}

			switch (inputTypeManagerScript.Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 1: // Mouse + Keyboard (Pressing)
					Destroy_MessageEvents_2();
					Destroy_MessageEvents_3();
					break;

				case 2: // Mouse + Keyboard (Legacy)
					Destroy_MessageEvents_0_1();
					Destroy_MessageEvents_3();
					break;
				case 3: // GamePad (Single stick)
					Destroy_MessageEvents_0_1();
					Destroy_MessageEvents_2();
					break;
			}
		}


		void Destroy_MessageEvents_0_1()
		{
			for (int i = 0; i < MessageEvents_0_1.Length; i++) {
				Destroy (MessageEvents_0_1 [i].gameObject);
			}
		}


		void Destroy_MessageEvents_2()
		{
			for (int i = 0; i < MessageEvents_2.Length; i++) {
				Destroy (MessageEvents_2 [i].gameObject);
			}
		}


		void Destroy_MessageEvents_3()
		{
			for (int i = 0; i < MessageEvents_3.Length; i++) {
				Destroy (MessageEvents_3 [i].gameObject);
			}
		}

	}

}

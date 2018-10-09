using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{
	
	[DefaultExecutionOrder (+1)] // (Note.) This script is executed after other scripts, in order to move the bars smoothly.
	[ RequireComponent (typeof(Canvas))]
	public class UI_HP_Bars_Target_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Canvas_HP_Bars (Target)" in the scene.
		 * This script controls the Hit Points Bars for displaying the target tank's hit points values.
		 * This script works in combination with "Aiming_Control_CS" in the current selected tank, and "Damage_Control_Center_CS" in the target tank.
		*/


		// User options >>
		public Canvas This_Canvas;
		public CanvasScaler This_Canvas_Scaler;
		public Transform Bars_Parent_Transform;
		public Image Body_Bar;
		public Image Turret_Bar;
		public Image Left_Track_Bar;
		public Image Right_Track_Bar;
		public float Flash_Time = 1.0f;
		// << User options


		Aiming_Control_CS aimingScript;
		Image[] bodyBarImages;
		Image[] turretBarImages;
		Image[] leftTrackBarImages;
		Image[] rightTrackBarImages;
		float previousBodyHP;
		float previousTurretHP;
		float previousLeftTrackHP;
		float previousRightTrackHP;
		Color initialColor;
		int flashCancelID;
		Transform currentTargetTransform;
		Damage_Control_Center_CS targetDamageScript;


		void Start()
		{
			Initialize();
		}


		void Initialize()
		{
			if (This_Canvas == null) {
				This_Canvas = GetComponent <Canvas>();
			}

			if (This_Canvas_Scaler == null) {
				This_Canvas_Scaler = GetComponent <CanvasScaler>();
			}

			if (Bars_Parent_Transform == null) {
				Debug.LogWarning("'Bars_Parent_Transform'' is not assigned in '" + this.name + "'.");
				Destroy(this);
				return;
			}

			// Store the initial color.
			initialColor = Body_Bar.color;

			// Get all the child images.
			bodyBarImages = Body_Bar.GetComponentsInChildren <Image>();
			turretBarImages = Turret_Bar.GetComponentsInChildren <Image>();
			leftTrackBarImages = Left_Track_Bar.GetComponentsInChildren <Image>();
			rightTrackBarImages = Right_Track_Bar.GetComponentsInChildren <Image>();
		}


		void LateUpdate ()
		{
			if (aimingScript == null) { // The tank has been destroyed.
				Enable_Canvas(false);
				return;
			}

			if (aimingScript.Is_Selected == false || aimingScript.Target_Transform == null) { // The tank is not selected now, or does not lock on any target.
				Enable_Canvas(false);
				return;
			}

			if (aimingScript.Target_Transform != currentTargetTransform) { // The target has been changed.
				Update_Traget_Information();
				return;
			}

			if (targetDamageScript == null) { // The target has no "Damage_Control_Center_CS".
				Enable_Canvas(false);
				return;
			}
			 
			// Set the appearance and the position of all the bars.
			Set_Appearance_And_Position();

			// Control the appearance of each bar.
			Control_Bars();
		}


		void Enable_Canvas (bool isEnabled)
		{
			if (This_Canvas.enabled != isEnabled) {
				This_Canvas.enabled = isEnabled;
			}
		}


		void Update_Traget_Information()
		{
			currentTargetTransform = aimingScript.Target_Transform;

			// Get the "Damage_Control_Center_CS" in the target.
			targetDamageScript = currentTargetTransform.GetComponentInParent <Damage_Control_Center_CS>();
			if (targetDamageScript == null) {
				return;
			}

			// Store the HP values.
			previousBodyHP = targetDamageScript.MainBody_HP;
			previousTurretHP = targetDamageScript.Turret_Props[0].hitPoints;
			previousLeftTrackHP = targetDamageScript.Left_Track_HP;
			previousRightTrackHP = targetDamageScript.Right_Track_HP;
		}


		void Set_Appearance_And_Position()
		{
			// Set the appearance and the position.
			Vector3 currentPosition = Camera.main.WorldToScreenPoint (aimingScript.Target_Position);
			if (currentPosition.z < 0.0f) { // Behind of the camera.
				Enable_Canvas (false);
			}
			else {
				Enable_Canvas (true);

				// Set the scale.
				float frustumHeight  = 2.0f * Vector3.Distance (Camera.main.transform.position, aimingScript.Target_Position) * Mathf.Tan (Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
				Bars_Parent_Transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.5f, 1.0f / frustumHeight);

				// Set the position.
				float resolutionOffset = Screen.width / This_Canvas_Scaler.referenceResolution.x;
				currentPosition.y += Mathf.Lerp(128.0f, 640.0f, 1.0f / frustumHeight) * resolutionOffset;
				Bars_Parent_Transform.position = currentPosition;
			}
		}


		void Control_Bars()
		{
			// MainBody
			Body_Bar.fillAmount = targetDamageScript.MainBody_HP / targetDamageScript.Initial_Body_HP;
			if (previousBodyHP != targetDamageScript.MainBody_HP) {
				flashCancelID = 1;
				StartCoroutine (Flash (bodyBarImages, 1));
			}

			// Turret
			Turret_Bar.fillAmount = targetDamageScript.Turret_Props[0].hitPoints / targetDamageScript.Initial_Turret_HP;
			if (previousTurretHP != targetDamageScript.Turret_Props[0].hitPoints) {
				flashCancelID = 2;
				StartCoroutine (Flash(turretBarImages, 2));
			}

			// Left Track
			Left_Track_Bar.fillAmount = targetDamageScript.Left_Track_HP / targetDamageScript.Initial_Left_Track_HP;
			if (previousLeftTrackHP != targetDamageScript.Left_Track_HP) {
				flashCancelID = 3;
				StartCoroutine (Flash(leftTrackBarImages, 3));
			}

			// Right Track
			Right_Track_Bar.fillAmount = targetDamageScript.Right_Track_HP / targetDamageScript.Initial_Right_Track_HP;
			if (previousRightTrackHP != targetDamageScript.Right_Track_HP) {
				flashCancelID = 4;
				StartCoroutine (Flash(rightTrackBarImages, 4));
			}

			// Store the HP values.
			previousBodyHP = targetDamageScript.MainBody_HP;
			previousTurretHP = targetDamageScript.Turret_Props[0].hitPoints;
			previousLeftTrackHP = targetDamageScript.Left_Track_HP;
			previousRightTrackHP = targetDamageScript.Right_Track_HP;
		}


		IEnumerator Flash(Image[] images, int id)
		{
			flashCancelID = 0;
			float count = 0.0f;
			while (count < Flash_Time) {
				if (id == flashCancelID) {
					yield break;
				}
				Color currentColor = initialColor;
				for (int i = 0; i < images.Length; i++) {
					currentColor.a = Mathf.Lerp(initialColor.a, 1.0f, Mathf.Sin(count / Flash_Time * Mathf.PI));
					images[i].color = currentColor;
				}
				count += Time.deltaTime;
				yield return null;
			}
		}
			

		public void Get_Aiming_Script(Aiming_Control_CS tempAimingScript)
		{ // Called from "Aiming_Control_CS".
			aimingScript = tempAimingScript;

			if (aimingScript.Target_Transform == null) { // The tank does not lock on any target.
				currentTargetTransform = null;
				return;
			}

			Update_Traget_Information();
		}

	}

}
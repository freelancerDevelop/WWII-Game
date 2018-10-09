using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Drive_Control_CS : MonoBehaviour
	{

		/*
		 * This script is attached to the "MainBody" of the tank.
		 * This script controls the driving of the tank, such as speed, torque, acceleration and so on.
		 * This script works in combination with "Drive_Wheel_Parent_CS" in the 'Create_##Wheels objects', and "Drive_Wheel_CS" in the drive wheels.
		*/
	
		// User options >>
		public float Torque = 2000.0f;
		public float Max_Speed = 8.0f;
		public float Turn_Brake_Drag = 150.0f;
		public float MaxAngVelocity_Limit = 45.0f;
		public float Pivot_Turn_Rate = 0.3f;

		public bool Acceleration_Flag = false;
		public float Acceleration_Time = 4.0f;
		public float Deceleration_Time = 0.1f;

		public bool Torque_Limitter = false;
		public float Max_Slope_Angle = 45.0f;

		public float ParkingBrake_Velocity = 0.5f;
		public float ParkingBrake_Lag = 0.5f;

		public bool Use_AntiSlip = false;
		public float Ray_Distance = 1.0f;
		// << User options


		// Set by "Input_Type_Settings_CS".
		public int Input_Type = 0;

		// Set by "Drive_Control_Input_##_##_CS" scripts.
		public bool Stop_Flag = true; // Referred to from "Steer_Wheel_CS".
		public float L_Input_Rate;
		public float R_Input_Rate;
		public float Turn_Brake_Rate;

		// Referred to from "Drive_Wheel_Parent_CS".
		public float Speed_Rate; // Referred to from also "Input_Type_Settings_CS".
		public float L_Brake_Drag;
		public float R_Brake_Drag;
		public float Left_Torque;
		public float Right_Torque;

		// Referred to from "Fix_Shaking_Rotation_CS".
		public bool Parking_Brake;

		Transform thisTransform;
		Rigidbody thisRigidbody;
		float leftSpeedRate;
		float rightSpeedRate;
		float defaultTorque;
		float acceleRate;
		float deceleRate;
		float stoppingTime;


		bool isSelected;

		Drive_Control_Input_00_Base_CS inputScript;


		void Start()
		{
			Initialize();
		}


		void Initialize ()
		{
			thisTransform = transform;
			thisRigidbody = GetComponent <Rigidbody>();
			defaultTorque = Torque;
			if (Acceleration_Flag) {
				acceleRate = 1.0f / Acceleration_Time;
				deceleRate = 1.0f / Deceleration_Time;
			}

			// Set the input script.
			switch (Input_Type) {
				case 0: // Mouse + Keyboard (Stepwise)
				case 2: // Mouse + Keyboard (Legacy)
					inputScript = gameObject.AddComponent <Drive_Control_Input_01_Keyboard_Stepwise_CS>();
					break;
				case 1: // Mouse + Keyboard (Pressing)
					inputScript = gameObject.AddComponent <Drive_Control_Input_02_Keyboard_Pressing_CS>();
					break;
				case 3: // Gamepad (Single stick)
					inputScript = gameObject.AddComponent <Drive_Control_Input_03_Single_Stick_CS>();
					break;
				case 4: // Gamepad (Twin sticks)
					inputScript = gameObject.AddComponent <Drive_Control_Input_04_Twin_Sticks_CS>();
					break;
				case 5: // Gamepad (Triggers)
					inputScript = gameObject.AddComponent <Drive_Control_Input_05_Triggers_CS>();
					break;

				case 10: // AI
					inputScript = gameObject.AddComponent <Drive_Control_Input_99_AI_CS>();
					break;
			}

			// Prepare the input script.
			if (inputScript != null) {
				inputScript.Prepare(this);
			}
		}


		void Update()
		{
			if (isSelected || Input_Type == 10) { // The tank is selected, or AI.
				inputScript.Drive_Input();
			}

			// Set "leftSpeedRate" and "rightSpeedRate".
			Set_Speed_Rate();
		}


		void FixedUpdate ()
		{
			// Control the automatic parking brake.
			Control_Parking_Brake ();

			// Call anti-spinning function for high speed tanks.
			Anti_Spin ();

			// Call anti-slipping function.
			if (Use_AntiSlip) {
				Anti_Slip ();
			}

			// Limit the torque in slope.
			if (Torque_Limitter) {
				Limit_Torque ();
			}
		}


		void Set_Speed_Rate ()
		{
			// Set the "leftSpeedRate" and "rightSpeedRate".
			if (Acceleration_Flag) {
				Acceleration_And_Deceleration();
			}
			else {
				Constant_Speed();
			}

			// Set the "Speed_Rate" value.
			if (Mathf.Abs (leftSpeedRate) > Mathf.Abs (rightSpeedRate)) {
				Speed_Rate = Mathf.Abs (leftSpeedRate);
			}
			else {
				Speed_Rate = Mathf.Abs (rightSpeedRate);
			}

			// Set the "L_Brake_Drag" and "R_Brake_Drag".
			L_Brake_Drag = Mathf.Clamp (Turn_Brake_Drag * -Turn_Brake_Rate, 0.0f, Turn_Brake_Drag);
			R_Brake_Drag = Mathf.Clamp (Turn_Brake_Drag * Turn_Brake_Rate, 0.0f, Turn_Brake_Drag);

			// Set the "Left_Torque" and "Right_Torque".
			Left_Torque = Torque * -Mathf.Sign(leftSpeedRate) * Mathf.Ceil (Mathf.Abs (leftSpeedRate)); // (Note.) When the "leftSpeedRate" is zero, the torque will be set to zero.
			Right_Torque = Torque * Mathf.Sign(rightSpeedRate) * Mathf.Ceil (Mathf.Abs (rightSpeedRate));
		}


		void Acceleration_And_Deceleration ()
		{
			// Synchronize the left and right speed rates to increase the straightness.
			if (Stop_Flag == false && L_Input_Rate == -R_Input_Rate) { // Not stopping && Going straight.
				// Set the average value to the both sides.
				float averageRate = (leftSpeedRate + rightSpeedRate) * 0.5f;
				leftSpeedRate = averageRate;
				rightSpeedRate = averageRate;
			}

			// Set the speed rates.
			leftSpeedRate = Calculate_Speed_Rate (leftSpeedRate, -L_Input_Rate);
			rightSpeedRate = Calculate_Speed_Rate (rightSpeedRate, R_Input_Rate);
		}


		float Calculate_Speed_Rate (float currentRate, float targetRate)
		{
			float moveRate;
			if (Mathf.Sign (targetRate) == Mathf.Sign (currentRate)) { // The both rates have the same direction.
				if (Mathf.Abs (targetRate) > Mathf.Abs (currentRate)) { // It should be in acceleration.
					moveRate = acceleRate;
				} else { // It should be in deceleration.
					moveRate = deceleRate;
				}
			} else { // The both rates have different directions. >> It should be in deceleration until the currentRate becomes zero.
				moveRate = deceleRate;
			}
			return Mathf.MoveTowards (currentRate, targetRate, moveRate * Time.deltaTime);
		}


		void Constant_Speed ()
		{
			leftSpeedRate = -L_Input_Rate;
			rightSpeedRate = R_Input_Rate;
		}


		void Control_Parking_Brake ()
		{
			if (Stop_Flag) { // The tank should stop.
				// Get the velocities of the Rigidbody.
				float currentVelocity = thisRigidbody.velocity.magnitude;
				float currentAngularVelocity = thisRigidbody.angularVelocity.magnitude;

				// 
				if (Parking_Brake == true) { // The parking brake is working now.
					// Check the Rigidbody velocities.
					if (currentVelocity > ParkingBrake_Velocity && currentAngularVelocity > ParkingBrake_Velocity) { // The Rigidbody should have been moving by receiving external force.
						// Release the parking brake.
						Parking_Brake = false;
						thisRigidbody.constraints = RigidbodyConstraints.None;
						stoppingTime = 0.0f;
						return;
					} // The Rigidbody almost stops.
					return;
				}
				else { // The parking brake is not working.
					// Check the Rigidbody velocities.
					if (currentVelocity < ParkingBrake_Velocity && currentAngularVelocity < ParkingBrake_Velocity) { // The Rigidbody almost stops.
						// Count the stopping time.
						stoppingTime += Time.fixedDeltaTime;
						if (stoppingTime > ParkingBrake_Lag) { // The stopping time has been over the "ParkingBrake_Lag".
							// Put on the parking brake.
							Parking_Brake = true;
							thisRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
							leftSpeedRate = 0.0f;
							rightSpeedRate = 0.0f;
							return;
						}
						else { // The stopping time has not over yet.
							return;
						}
					} // The Rigidbody almost stops.
					return;
				}
			}
			else { // The tank should be driving now.
				if (Parking_Brake == true) { // The parking brake is still working.
					// Release parking brake.
					Parking_Brake = false;
					thisRigidbody.constraints = RigidbodyConstraints.None;
					stoppingTime = 0.0f;
				}
			}
		}


		void Anti_Spin ()
		{
			// Reduce the spinning motion by controling the angular velocity of the Rigidbody.
			if (L_Input_Rate != R_Input_Rate && Turn_Brake_Rate == 0.0f) { // The tank should not be doing pivot-turn or brake-turn.
				// Control the angular velocity of the Rigidbody.
				Vector3 currentAngularVelocity = thisRigidbody.angularVelocity;
				currentAngularVelocity.y *= 0.9f; // Reduce the angular velocity on Y-axis.
				// Set the new angular velocity.
				thisRigidbody.angularVelocity = currentAngularVelocity;
			}
		}


		Ray ray = new Ray ();
		int layerMask = ~((1 << 10) + (1 << 2) + (1 << 11)); // Layer 2 = Ignore Ray, Layer 10 = Ignore All,  Layer 11 = MainBody.
		void Anti_Slip ()
		{
			// Reduce the slippage by controling the velocity of the Rigidbody.
			// Cast the ray downward to detect the ground.
			ray.origin = thisTransform.position;
			ray.direction = -thisTransform.up;
			if (Physics.Raycast(ray, Ray_Distance, layerMask) == true) { // The ray hits the ground.
				// Control the velocity of the Rigidbody.
				Vector3 currentVelocity = thisRigidbody.velocity;
				if (leftSpeedRate == 0.0f && rightSpeedRate == 0.0f) { // The tank should stop.
					// Reduce the Rigidbody velocity gradually.
					currentVelocity.x *= 0.9f;
					currentVelocity.z *= 0.9f;
				} else { // The tank should been driving.
					float sign;
					if (leftSpeedRate == rightSpeedRate) { // The tank should be going straight forward or backward.
						sign = Mathf.Sign(leftSpeedRate);
					}
					else if (leftSpeedRate == -rightSpeedRate) { // The tank should be doing pivot-turn.
						sign = 1.0f;
					}
					else { // The tank should be doing brake-turn.
						sign = Mathf.Sign(leftSpeedRate + rightSpeedRate);
					}
					// Change the velocity of the Rigidbody forcibly.
					currentVelocity = Vector3.MoveTowards(currentVelocity, thisTransform.forward * sign * currentVelocity.magnitude, 50.0f * Time.fixedDeltaTime);
				}
				// Set the new velocity.
				thisRigidbody.velocity = currentVelocity;
			}
		}


		void Limit_Torque ()
		{
			// Reduce the torque according to the angle of the slope.
			float torqueRate = Mathf.DeltaAngle (thisTransform.eulerAngles.x, 0.0f) / Max_Slope_Angle;
			if (leftSpeedRate > 0.0f && rightSpeedRate > 0.0f) { // The tank should be going forward.
				Torque = Mathf.Lerp (defaultTorque, 0.0f, torqueRate);
			} else { // The tank should be going backward.
				Torque = Mathf.Lerp (defaultTorque, 0.0f, -torqueRate);
			}
		}


		void Get_Input_Type(int inputType)
		{ // Called from "Input_Settings_CS".
			Input_Type = inputType;
		}
			

		void Selected(bool isSelected)
		{ // Called from "ID_Settings_CS".
			this.isSelected = isSelected;
		}


		void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			Destroy (inputScript as Object);
			Destroy (this);
		}


		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
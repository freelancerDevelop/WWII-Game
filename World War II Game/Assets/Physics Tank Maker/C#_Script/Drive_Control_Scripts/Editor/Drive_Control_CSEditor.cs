using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Drive_Control_CS))]
	public class Drive_Control_CSEditor : Editor
	{
	
		SerializedProperty TorqueProp;
		SerializedProperty Max_SpeedProp;
		SerializedProperty Turn_Brake_DragProp;
		SerializedProperty MaxAngVelocity_LimitProp;
		SerializedProperty Pivot_Turn_RateProp;

		SerializedProperty Acceleration_FlagProp;
		SerializedProperty Acceleration_TimeProp;
		SerializedProperty Deceleration_TimeProp;

		SerializedProperty Torque_LimitterProp;
		SerializedProperty Max_Slope_AngleProp;

		SerializedProperty ParkingBrake_VelocityProp;
		SerializedProperty ParkingBrake_LagProp;

		SerializedProperty Use_AntiSlipProp;
		SerializedProperty Ray_DistanceProp;


		void OnEnable ()
		{
			TorqueProp = serializedObject.FindProperty ("Torque");
			Max_SpeedProp = serializedObject.FindProperty ("Max_Speed");
			Turn_Brake_DragProp = serializedObject.FindProperty ("Turn_Brake_Drag");
			MaxAngVelocity_LimitProp = serializedObject.FindProperty ("MaxAngVelocity_Limit");
			Pivot_Turn_RateProp = serializedObject.FindProperty ("Pivot_Turn_Rate");

			Acceleration_FlagProp = serializedObject.FindProperty ("Acceleration_Flag");
			Acceleration_TimeProp = serializedObject.FindProperty ("Acceleration_Time");
			Deceleration_TimeProp = serializedObject.FindProperty ("Deceleration_Time");

			Torque_LimitterProp = serializedObject.FindProperty ("Torque_Limitter");
			Max_Slope_AngleProp = serializedObject.FindProperty ("Max_Slope_Angle");

			ParkingBrake_VelocityProp = serializedObject.FindProperty ("ParkingBrake_Velocity");
			ParkingBrake_LagProp = serializedObject.FindProperty ("ParkingBrake_Lag");

			Use_AntiSlipProp = serializedObject.FindProperty ("Use_AntiSlip");
			Ray_DistanceProp = serializedObject.FindProperty ("Ray_Distance");
		}


		public override void OnInspectorGUI ()
		{
			if (EditorApplication.isPlaying) {
				return;
			}

			serializedObject.Update();

			GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Driving Wheels settings", MessageType.None, true);
			EditorGUILayout.Slider(TorqueProp, 0.0f, 100000.0f, "Torque");
			EditorGUILayout.Slider(Max_SpeedProp, 0.0f, 30.0f, "Maximum Speed");
			EditorGUILayout.Slider(Turn_Brake_DragProp, 0.0f, 1000.0f, "Turn Brake Drag");
			EditorGUILayout.Slider(MaxAngVelocity_LimitProp, 1.0f, 100.0f, "MaxAngularVelocity Limit");
			EditorGUILayout.Slider(Pivot_Turn_RateProp, 0.01f, 1.0f, "Pivot Turn Rate");
			
			EditorGUILayout.Space();
			Acceleration_FlagProp.boolValue = EditorGUILayout.Toggle("Acceleration", Acceleration_FlagProp.boolValue);
			if (Acceleration_FlagProp.boolValue) {
				EditorGUILayout.Slider(Acceleration_TimeProp, 0.01f, 30.0f, "Acceleration Time");
				EditorGUILayout.Slider(Deceleration_TimeProp, 0.01f, 30.0f, "Deceleration Time");
			}

			EditorGUILayout.Space();
			Torque_LimitterProp.boolValue = EditorGUILayout.Toggle("Torque Limitter", Torque_LimitterProp.boolValue);
			if (Torque_LimitterProp.boolValue) {
				EditorGUILayout.Slider(Max_Slope_AngleProp, 0.0f, 90.0f, "Max Slope Angle");
			}
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Parking Brake settings", MessageType.None, true);
			EditorGUILayout.Slider(ParkingBrake_VelocityProp, 0.0f, 10.0f, "Work Velocity");
			EditorGUILayout.Slider(ParkingBrake_LagProp, 0.0f, 5.0f, "Lag Time");
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Anti Slipping settings", MessageType.None, true);
			Use_AntiSlipProp.boolValue = EditorGUILayout.Toggle("Use Anti-Slipping", Use_AntiSlipProp.boolValue);
			if (Use_AntiSlipProp.boolValue) {
				EditorGUILayout.Slider(Ray_DistanceProp, 0.0f, 10.0f, "Ray Distance");
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
				
			serializedObject.ApplyModifiedProperties();
		}
	
	}

}
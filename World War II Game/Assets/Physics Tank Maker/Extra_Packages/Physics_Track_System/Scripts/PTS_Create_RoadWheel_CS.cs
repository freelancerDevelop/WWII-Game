using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTS
{
	
	[ System.Serializable]
	public class RoadWheelsProp
	{
		public string parentName;
		public float baseRadius;
		public float [] angles;
	}

	public class PTS_Create_RoadWheel_CS : MonoBehaviour
	{

		public bool Fit_ST_Flag = false;

		public float Sus_Distance = 2.06f;
		public int Num = 6;
		public float Spacing = 0.88f;
		public float Sus_Length = 0.5f;
		public bool Set_Individually = false;
		public float Sus_Angle = 0.0f;
		public float [] Sus_Angles;
		public float Sus_Anchor = 0.0f;
		public float Sus_Mass = 100.0f;
		public float Sus_Spring = 20000.0f;
		public float Sus_Damper = 1500.0f;
		public float Sus_Target = -25.0f;
		public float Sus_Forward_Limit = 0.0f;
		public float Sus_Backward_Limit = 30.0f;
		public Mesh Sus_L_Mesh;
		public Mesh Sus_R_Mesh;
		public int Sus_Materials_Num = 1;
		public Material[] Sus_Materials;
		public Material Sus_L_Material;
		public Material Sus_R_Material;
		public float Reinforce_Radius = 0.5f;

		public float Wheel_Distance = 2.7f;
		public float Wheel_Mass = 100.0f;
		public float Wheel_Radius = 0.3f;
		public PhysicMaterial Collider_Material;
		public Mesh Wheel_Mesh;
		public int Wheel_Materials_Num = 1;
		public Material[] Wheel_Materials;
		public Material Wheel_Material;

		public bool Drive_Wheel = true;
		public bool Wheel_Resize = false;
		public float ScaleDown_Size = 0.5f;
		public float Return_Speed = 0.05f;

		public bool RealTime_Flag = true;

		public RoadWheelsProp Get_Current_Angles ()
		{
			RoadWheelsProp currentProp = new RoadWheelsProp ();
			currentProp.parentName = transform.name;
			currentProp.baseRadius = Wheel_Radius;
			currentProp.angles = new float [Num];
			for (int i = 0; i < Num; i++) {
				Transform susTransform = transform.Find ("Suspension_L_" + (i + 1));
				if (susTransform) {
					float currentAngle = susTransform.localEulerAngles.y;
					if (currentAngle > 180.0f) {
						currentAngle = -(360.0f - currentAngle);
					} else if (currentAngle < -180.0f) {
						currentAngle = 360.0f - currentAngle;
					}
					currentProp.angles [i] = currentAngle;
				}
			}
			return currentProp;
		}

		void OnDrawGizmosSelected ()
		{
			for (int i = 0; i < transform.childCount; i++) {
				Transform childTransform = transform.GetChild(i);
				if (childTransform.gameObject.layer == 10) { // Suspension Arm.
					HingeJoint joint = childTransform.GetComponent <HingeJoint>();
					Transform bodyTransform = joint.connectedBody.transform;
					Vector3 anchorPos = bodyTransform.position + (bodyTransform.right * joint.connectedAnchor.x) + (bodyTransform.up * joint.connectedAnchor.y) + (bodyTransform.forward * joint.connectedAnchor.z);
					float currentAng = joint.limits.max - Sus_Forward_Limit;
					Vector3 tempPos;
					tempPos.x = 0.0f;
					// Limits Min
					tempPos.y = Mathf.Sin(Mathf.Deg2Rad * (joint.limits.min - currentAng)) * Sus_Length;
					tempPos.z = -Mathf.Cos(Mathf.Deg2Rad * (joint.limits.min - currentAng)) * Sus_Length;
					Gizmos.color = Color.green;
					Gizmos.DrawLine(anchorPos, anchorPos + (bodyTransform.up * tempPos.y) + (bodyTransform.forward * tempPos.z));
					// Limits Max
					tempPos.y = Mathf.Sin(Mathf.Deg2Rad * (joint.limits.max - currentAng)) * Sus_Length;
					tempPos.z = -Mathf.Cos(Mathf.Deg2Rad * (joint.limits.max - currentAng)) * Sus_Length;
					Gizmos.color = Color.green;
					Gizmos.DrawLine(anchorPos, anchorPos + (bodyTransform.up * tempPos.y) + (bodyTransform.forward * tempPos.z));
					// Target
					tempPos.y = Mathf.Sin(Mathf.Deg2Rad * (joint.spring.targetPosition - currentAng)) * Sus_Length;
					tempPos.z = -Mathf.Cos(Mathf.Deg2Rad * (joint.spring.targetPosition- currentAng)) * Sus_Length;
					Gizmos.color = Color.red;
					Gizmos.DrawLine(anchorPos, anchorPos + (bodyTransform.up * tempPos.y) + (bodyTransform.forward * tempPos.z));
					// Current
					currentAng = childTransform.localEulerAngles.y;
					tempPos.y = Mathf.Sin(Mathf.Deg2Rad * -currentAng) * Sus_Length;
					tempPos.z = -Mathf.Cos(Mathf.Deg2Rad * -currentAng) * Sus_Length;
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(anchorPos, anchorPos + (bodyTransform.up * tempPos.y) + (bodyTransform.forward * tempPos.z));
				}
			}
		}

	}

}
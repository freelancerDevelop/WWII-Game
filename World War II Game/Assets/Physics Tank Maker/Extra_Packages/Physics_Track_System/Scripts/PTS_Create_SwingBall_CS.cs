using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTS
{

	public class PTS_Create_SwingBall_CS : MonoBehaviour
	{
	
		public float Distance = 2.7f;
		public int Num = 1;
		public float Spacing = 1.7f;
		public bool Set_Individually = false;
		public Vector3 [] Pos_Array;
		public float Mass = 10.0f;
		public bool Gravity = false;
		public float Radius = 0.1f;
		public float Range = 0.3f;
		public float Spring = 500.0f;
		public float Damper = 100.0f;
		public int Layer = 0;
		public PhysicMaterial Collider_Material;

		void OnDrawGizmosSelected ()
		{
			Gizmos.color = Color.green;
			for (int i = 0; i < transform.childCount; i++) {
				Transform childTransform = transform.GetChild(i);
				Vector3 pos = childTransform.position;
				Gizmos.DrawSphere (pos, Radius);
				Gizmos.DrawLine (pos - (Vector3.up * Range), pos + (Vector3.up * Range));
			}
		}

	}

}
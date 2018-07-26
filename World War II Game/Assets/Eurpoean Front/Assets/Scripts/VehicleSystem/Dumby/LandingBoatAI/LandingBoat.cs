using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xrayhunter.LandingCraft
{
    public class LandingBoat : MonoBehaviour
    {

        public enum DropType
        {
            NoDrop,
            WhileMoving,
            DropAtFinalPoint
        }


        public float speedMin = 5.0f;
        public float speedMax = 10.0f;
        public float enginePower = 0.05f;
        public float brakingPower = 0.02f;

        public float rotationSpeed = 0.1f;

        public float paddingTime = 5.0f;

        public List<GameObject> payload;

        public Vector3 dropOffOffset;
        public Vector3 dropOffSize;
        public float dropSpacing;
        public bool unlimitedPayloads = true;
        public DropType dropWhileMoving = DropType.NoDrop;

        public Vector3[] waypoints;

        public bool destroyAtFinalWaypoint = false;

        public Transform door;

        public float doorOpenSpeed = 10.0f;

        private int waypointCounter = 0;

        private float speed = 0.0f;

        private float angle = 0.0f;

        // Use this for initialization
        void Start()
        {
            int counter = 0;
            int moveUp = 0;
            foreach (GameObject prefab in payload)
            {
                GameObject obj = Instantiate(prefab, this.transform);
                obj.transform.position = this.transform.position + dropOffOffset + new Vector3(dropSpacing * (counter % 3 == 0 ? -1 : 1), 0, moveUp);

                counter++;

                if (counter % 3 == 2)
                    moveUp++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (waypoints != null & waypoints.Length > 0)
            {
                if (waypointCounter < waypoints.Length && waypoints[waypointCounter] != null)
                {
                    speed = Mathf.Lerp(speed, speedMax, Time.deltaTime);

                    Quaternion rotation = Quaternion.LookRotation(waypoints[waypointCounter] - transform.position);

                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);


                    if (Vector3.Distance(this.transform.position, waypoints[waypointCounter]) <= 10)
                    {
                        waypointCounter++;
                    }
                }
                else
                {
                    speed = Mathf.Lerp(speed, 0, Time.deltaTime);
                }

                if (waypointCounter >= waypoints.Length)
                {
                    if (door != null)
                    {
                        door.eulerAngles = new Vector3(angle, door.eulerAngles.y, door.eulerAngles.z);
                        angle += Time.deltaTime * doorOpenSpeed;
                        angle = Mathf.Clamp(angle, -89, 89);
                    }

                    if (destroyAtFinalWaypoint)
                        DestroyImmediate(this.gameObject);
                }

                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            
            Gizmos.color = Color.yellow;

            foreach (Vector3 waypoint in waypoints)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(waypoint, new Vector3(1, 1, 1));
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(waypoint, (paddingTime * 2) + speed);
            }


            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(this.transform.position + dropOffOffset, dropOffSize);
        }
    }

}

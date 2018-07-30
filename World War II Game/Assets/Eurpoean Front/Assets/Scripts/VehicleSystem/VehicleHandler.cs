using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xrayhunter.VehicleSystem
{
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;

        public bool motor;
        public bool steering;
    }

    public class VehicleHandler : MonoBehaviour
    {
        public List<AxleInfo> axles;
        public float maxMotorTorque;
        public float maxSteeringAngle;

        private float motor;
        private float steering;

        // Use this for initialization
        void Start()
        {

        }

        // finds the corresponding visual wheel
        // correctly applies the transform
        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0)
            {
                return;
            }

            Transform visualWheel = collider.transform;

            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //float motor = maxMotorTorque * Input.GetAxis("Vertical");
            //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

            foreach (AxleInfo axle in axles)
            {
                if (axle.steering)
                {
                    axle.leftWheel.steerAngle = steering;
                    axle.rightWheel.steerAngle = steering;
                }

                if (axle.motor)
                {
                    axle.leftWheel.motorTorque = motor;
                    axle.rightWheel.motorTorque = motor;
                }

                ApplyLocalPositionToVisuals(axle.leftWheel);
                ApplyLocalPositionToVisuals(axle.rightWheel);
            }
        }

        public void SetMotor(float motor)
        {
            this.motor = motor;
        }
        public void SetSteering(float steering)
        {
            this.steering = steering;
        }
    }

}

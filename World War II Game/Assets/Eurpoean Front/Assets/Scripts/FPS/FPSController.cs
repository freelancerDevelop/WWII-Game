using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace xrayhunter.FPS
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class FPSController : MonoBehaviour
    {

        public float walkSpeed = 5.0f;
        public float runSpeed = 10.0f;
        public float jumpForce = 5.0f;

        public float aimSensitivity = 2.0f;

        public Transform target;

        public Vector3 offset;

        [HideInInspector]
        public bool locked = true;

        private CharacterController controller;
        private Animator animator;

        private Transform lowerBack;
        private Transform neck;

        private Vector3 gravity = Physics.gravity;
        private float pitch;
        private float yaw;
        private float jumpSpeed;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            if (animator != null)
            {
                lowerBack = animator.GetBoneTransform(HumanBodyBones.Chest);
                neck = animator.GetBoneTransform(HumanBodyBones.Neck);
            }
        }

        private void LateUpdate()
        {


            Vector3 rotation = new Vector3();
            if (lowerBack != null)
            {
                if (Camera.allCameras[0] != null)
                {
                    lowerBack.transform.LookAt(target);
                    lowerBack.rotation = lowerBack.rotation * Quaternion.Euler(offset);

                }
            }
            else
            {
                rotation = new Vector3(-pitch, yaw, 0);
            }

            transform.eulerAngles = rotation;
        }

        // Update is called once per frame
        void Update()
        {
            // Cursor Lock
            if (locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;


            // Interaction
            if (Input.GetButtonDown("Interaction"))
            {
                RaycastHit hit;

                if (Camera.allCameras[0] != null)
                {
                    if (Physics.Raycast(Camera.allCameras[0].ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {

                    }
                }
            }

            // Aiming
            pitch -= Input.GetAxis("Mouse Y") * aimSensitivity;
            yaw += Input.GetAxis("Mouse X") * aimSensitivity;

            //pitch = Mathf.Clamp(pitch, 25, 75);

            /*if (Input.GetButton("Free Look"))
            {
                if (headBone != null)
                {
                    headBone.transform.eulerAngles = Vector3.Lerp(lowerBackBone.transform.eulerAngles, new Vector3(pitch, yaw, 0), Time.deltaTime * aimSensitivity);
                }

                if (lowerBackBone != null)
                {
                    lowerBackBone.transform.eulerAngles = Vector3.Lerp(lowerBackBone.transform.eulerAngles, new Vector3(pitch, 0, 0), Time.deltaTime * aimSensitivity);
                }
            }
            else
            {
                if (headBone != null)
                {
                    headBone.transform.eulerAngles = new Vector3(0, 0, 0);
                }

            }*/

            // Movement

            float speed = walkSpeed;

            if (Input.GetButton("Run"))
            {
                speed = runSpeed;
            }

            Vector3 velocity = Vector3.forward * Input.GetAxis("Vertical") * speed;
            velocity += Vector3.right * Input.GetAxis("Horizontal") * speed;

            if (Input.GetButton("Jump"))
            {
                jumpSpeed = jumpForce;
            }

            jumpSpeed -= Mathf.Abs(gravity.y) * Time.deltaTime;
            velocity.y = jumpSpeed;

            controller.Move(velocity * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(target.position, 0.1f);
        }
    }
}
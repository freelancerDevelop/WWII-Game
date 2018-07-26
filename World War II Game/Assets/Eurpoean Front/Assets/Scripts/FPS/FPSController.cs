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

        public float bodyPitchLimitTop = 89;
        public float bodyPitchLimitBottom = -89;

        public float headPitchLimitTop = 89;
        public float headPitchLimitBottom = -89;

        public float headYawLimitTop = 89;
        public float headYawLimitBottom = -89;


        [HideInInspector]
        public bool locked = true;

        private CharacterController controller;
        private Animator animator;

        private Transform head;
        private Transform spine;
        
        private Vector3 gravity = Physics.gravity;
        private float spinePitch;
        private float spineYaw;
        private float headPitch;
        private float headYaw;
        private float jumpSpeed;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            if (animator != null)
            {
                head = animator.GetBoneTransform(HumanBodyBones.Head);
                spine = animator.GetBoneTransform(HumanBodyBones.Spine);
            }
        }

        private void LateUpdate()
        {
            // Looking around
            if (Input.GetButton("Free Look"))
            {
                headPitch += Input.GetAxis("Mouse Y") * (aimSensitivity * 100) * Time.deltaTime;
                headYaw += Input.GetAxis("Mouse X") * (aimSensitivity * 100) * Time.deltaTime;

                headPitch = Mathf.Clamp(headPitch, headPitchLimitBottom - spinePitch, headPitchLimitTop - spinePitch);
                headYaw = Mathf.Clamp(headYaw, headYawLimitBottom + spineYaw, headYawLimitTop + spineYaw);

                head.transform.eulerAngles = new Vector3(-headPitch, headYaw, 0);
            }
            else
            {
                headPitch = -spinePitch;
                headYaw = spineYaw;
                
                spinePitch -= Input.GetAxis("Mouse Y") * (aimSensitivity * 100) * Time.deltaTime;
                spineYaw += Input.GetAxis("Mouse X") * (aimSensitivity * 100) * Time.deltaTime;

                spinePitch = Mathf.Clamp(spinePitch, bodyPitchLimitBottom, bodyPitchLimitTop);

                head.transform.eulerAngles = new Vector3(spinePitch, spine.transform.eulerAngles.y - 90, 0);
                spine.transform.localRotation = Quaternion.Euler(-180, 0, spinePitch);
                transform.eulerAngles = new Vector3(0, spineYaw, 0);
            }
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
            // Movement

            float speed = walkSpeed;

            if (Input.GetButton("Run"))
            {
                speed = runSpeed;
            }

            Vector3 velocity = transform.forward * Input.GetAxis("Vertical") * speed;
            velocity += transform.right * Input.GetAxis("Horizontal") * speed;
            
            if (controller.isGrounded)
            {
                if (Input.GetButton("Jump"))
                {
                    jumpSpeed = jumpForce;
                }
            }

            jumpSpeed -= Mathf.Abs(gravity.y) * Time.deltaTime;
            velocity.y = jumpSpeed;

            controller.Move(velocity * Time.deltaTime);
            
        }
    }
}
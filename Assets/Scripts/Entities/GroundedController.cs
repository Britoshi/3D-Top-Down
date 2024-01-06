using UnityEngine;

namespace Game
{
    public class GroundedController : BritoBehavior
    {  
        public float sphereRadius = 0.5f; // Radius of the sphere used for checking
        public LayerMask groundLayer; // Layer that represents the ground
        [SerializeField]
        private bool isGrounded;
        public Transform center;

        void Update()
        {
            CheckGround();
        }

        void CheckGround()
        {
            // The center of the sphere just below the object's position
            Vector3 sphereCenter = center.position - new Vector3(0, sphereRadius, 0);

            // Check if any colliders are within the sphere area
            isGrounded = Physics.OverlapSphere(sphereCenter, sphereRadius, groundLayer).Length > 0;

            // Optional: Debugging to visualize the sphere in the scene view
            Color debugColor = isGrounded ? Color.green : Color.red;
            Debug.DrawRay(sphereCenter, Vector3.up * sphereRadius, debugColor);
            Debug.DrawRay(sphereCenter, Vector3.down * sphereRadius, debugColor);
        }

        public bool IsGrounded()
        {
            return isGrounded;
        }
    } 
}
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
        public Vector3 offset;

        void Update()
        {
            CheckGround();
        }

        void CheckGround()
        {
            // The center of the sphere just below the object's position
            Vector3 sphereCenter = center.position;

            // Check if any colliders are within the sphere area
            isGrounded = Physics.OverlapSphere(sphereCenter  + offset, sphereRadius, groundLayer).Length > 0;
        }

        public bool IsGrounded()
        {
            return isGrounded;
        }

        // Draw a gizmo to visualize the sphere
        void OnDrawGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(center.position + offset, sphereRadius);
        }
    }
}

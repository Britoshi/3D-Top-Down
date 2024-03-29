using Game.StateMachine.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Game.StateMachine
{
    public class HumanoidStateMachine : StateMachine
    {
        internal HumanoidEntity humanoidEntity;

        public bool IsCrouching { set; get; }
        public bool IsAiming { set; get; }
        public Vector3 aimingPoint;

        [SerializeField]
        internal bool sprintHold;

        //IK

        public override void AssignFactory()
        {
            Factory = new HumanoidStateFactory(this);
            humanoidEntity = entity as HumanoidEntity;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStart()
        {


        }


        public override void OnUpdate()
        {
            if (!IsMoving)
            {
                GetAnimator().SetFloat("movement state", 0);
                return;
            }
            var vec = inputVector3.magnitude;
            vec *= IsMoving && IsRunning ? 3 : 2;
            GetAnimator().SetFloat("movement state", vec);
        }

        protected virtual void RotateTowardsInputDirection()
        {  
            if (rotationOverride) return;
            //var angle = Quaternion.EulerAngles(inputVector2);
            var lookAt = lastInput3;

            //lookAt.y = transform.position.y;

            if (lookAt == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(lookAt);

            ForwardDirection.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime * 3f);
            Model.transform.rotation = ForwardDirection.rotation;
        }
        protected virtual void RotateTowardsAimPoint()
        {
            if (rotationOverride) return;
            Model.transform.LookAt(aimingPoint);
        }


        internal override void AirborneBehavior()
        {
            HandleMovement();
        }
        internal virtual void HandleRunMovement()
        {
            if (movementOverride) return;
            RotateTowardsInputDirection();
            var runMult = 1.5f;
            var speed = entity.status.MovementSpeed.GetValue() * runMult;
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }
        internal virtual void HandleMovement()
        {
            if (movementOverride) return;
            RotateTowardsInputDirection();
            var speed = entity.status.MovementSpeed.GetValue();
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }
        internal virtual void HandleIdleMovement()
        {

        }


        internal virtual void HandleAimIdle()
        {
            GetAnimator().SetFloat("x", 0);
            GetAnimator().SetFloat("y", 0);
        }
        internal virtual void HandleAimingBehavior()
        {
            HandleAimMovement();
            RotateTowardsAimPoint();
        }
        internal virtual void HandleAimMovement()
        {
            Vector3 forwardDirection = transform.forward;
            Vector3 relativeMovementDirection = forwardDirection - inputVector3;
            print(inputVector3);

            var pY = transform.rotation.eulerAngles.y % 360;
            float angle = Vector3.SignedAngle(inputVector3, transform.forward, Vector3.up);
            float rad = Mathf.Deg2Rad * angle;

            Vector2 dir = new(-Mathf.Sin(rad), Mathf.Cos(rad));
            print(dir);

            GetAnimator().SetFloat("x", dir.x);
            GetAnimator().SetFloat("y", dir.y);

            var speed = entity.status.MovementSpeed.GetValue() * .355f;
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }

        internal virtual void TriggerCrouch()
        {
            IsCrouching = !IsCrouching;
        }

        internal virtual void OnJumpTriggered()
        {
            if (IsCrouching)
            {
                IsCrouching = false;
                return;
            }
            if (currentState.LockJump) return;

            TriggerJump();
        }
    }
}
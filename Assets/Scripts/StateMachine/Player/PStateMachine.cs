using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.StateMachine.Player
{

    public class PStateMachine : StateMachine
    {
        internal PlayerEntity player;

        public bool IsCrouching { set; get; }
        public bool IsAiming { set; get; }
        public Vector3 aimingPoint;

        internal PlayerControl controls;
        [SerializeField]
        internal Vector2 inputVector2;
        [SerializeField]
        internal Vector3 inputVector3, lastInput3;
        [SerializeField]
        internal bool sprintHold;

        //IK

        public override void AssignFactory()
        {
            Factory = new PStateFactory(this);
            player = entity as PlayerEntity;
        }

        public override void OnFixedUpdate()
        {
            
        } 

        public override void OnStart()
        {
            InitializeControls();

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

        void InitializeControls()
        { 
            controls = new();
            controls.Player.Enable();

            /*
            controls.Player.Move.performed += ctx =>
            {
                var vector2D = ctx.ReadValue<Vector2>().normalized;
                inputVector2 = vector2D;
                inputVector3 = new Vector3(vector2D.x, 0, vector2D.y);
            };
            controls.Player.Move.performed += ctx => IsMoving = true;
            controls.Player.Move.canceled += ctx =>
            {
                //inputVector2 = new Vector2(0, 0);
                //inputVector3 = new Vector3(0, 0, 0);
                IsMoving = false;
            };*/

            controls.Player.Sprint.performed += ctx => sprintHold = true;
            controls.Player.Sprint.canceled += ctx => sprintHold = false;

            controls.Player.Crouch.performed += TriggerCrouch;
            controls.Player.Jump.performed += OnJumpPressed;
        }
        void RotateTowardsInputDirection()
        {

            //var angle = Quaternion.EulerAngles(inputVector2);
            var lookAt = lastInput3;

            //lookAt.y = transform.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(lookAt);
            
            ForwardDirection.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime * 3f);
            Model.transform.rotation = ForwardDirection.rotation;
        }
        void RotateTowardsAimPoint()
        {
            Model.transform.LookAt(aimingPoint);
        }


        internal override void AirborneBehavior()
        {
            HandleWalkMovement();
        }
        internal void HandleRunMovement()
        {
            RotateTowardsInputDirection();
            var speed = player.status.MovementSpeed.GetValue() * 1.25f; 
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }
        internal void HandleWalkMovement()
        {
            RotateTowardsInputDirection();
            var speed = player.status.MovementSpeed.GetValue(); 
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }
        internal void HandleIdleMovement()
        { 

        }

        internal void HandleAimingBehavior()
        { 
            RotateTowardsAimPoint();
        }
        internal void HandleAimMovement()
        { 
            Vector3 forwardDirection = transform.forward; 
            Vector3 relativeMovementDirection = forwardDirection - inputVector3; 

            var pY = transform.rotation.eulerAngles.y % 360;
            float angle = Vector3.SignedAngle(inputVector3, transform.forward, Vector3.up); 
            float rad = Mathf.Deg2Rad * angle;

            Vector2 dir = new(-Mathf.Sin(rad), Mathf.Cos(rad)); 

            GetAnimator().SetFloat("x", dir.x);
            GetAnimator().SetFloat("y", dir.y); 

            var speed = player.status.MovementSpeed.GetValue() * .3f;
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }

        internal void TriggerCrouch(InputAction.CallbackContext ctx)
        {
            IsCrouching = !IsCrouching;
        }

        internal void OnJumpPressed(InputAction.CallbackContext ctx)
        {
            if (IsCrouching)
            {
                IsCrouching = false;
                return;
            }
            if (currentState.LockJump) return;

            TriggerJump();
        }

        internal void HandleAimIdle()
        {
            GetAnimator().SetFloat("x", 0);
            GetAnimator().SetFloat("y", 0);
        }
    }
}
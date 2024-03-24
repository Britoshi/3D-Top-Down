using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.StateMachine.Player
{

    public class PStateMachine : HumanoidStateMachine
    {
        internal PlayerEntity player; 
        internal PlayerControl controls;

        public override void AssignFactory()
        {
            Factory = new PStateFactory(this);
            player = entity as PlayerEntity;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        } 

        public override void OnStart()
        {
            base.OnStart();
            InitializeControls();

        }


        public override void OnUpdate()
        {
            base.OnUpdate();
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
            controls.Player.Jump.performed += OnJumpTriggered;
        }  

        internal override void AirborneBehavior()
        {
            HandleMovement();
        }
        internal override void HandleRunMovement()
        {
            RotateTowardsInputDirection();
            var runMult = 1.5f;
            var speed = player.status.MovementSpeed.GetValue() * runMult; 
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }
        internal override void HandleMovement()
        {
            RotateTowardsInputDirection();
            var speed = player.status.MovementSpeed.GetValue(); 
            rigidbody.velocity = inputVector3 * speed + rigidbody.velocity.y * transform.up;
        }


        internal void TriggerCrouch(InputAction.CallbackContext ctx)
        {
            IsCrouching = !IsCrouching;
        }

        internal void OnJumpTriggered(InputAction.CallbackContext ctx)
        {
            if (IsCrouching)
            {
                IsCrouching = false;
                return;
            }
            if (currentState.LockJump) return;

            TriggerJump();
        }

        internal override void HandleAimIdle()
        {
            GetAnimator().SetFloat("x", 0);
            GetAnimator().SetFloat("y", 0);
        }
    }
}
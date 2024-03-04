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


        internal PlayerControl controls;
        [SerializeField]
        internal Vector2 inputVector2;
        [SerializeField]
        internal Vector3 inputVector3;
        [SerializeField]
        internal bool sprintHold;

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
            IsRunning = sprintHold;
            UpdateDebugText();
        }

        void InitializeControls()
        { 
            controls = new();
            controls.Player.Enable();
            controls.Player.Move.performed += ctx => inputVector2 = ctx.ReadValue<Vector2>();
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
            };

            controls.Player.Sprint.performed += ctx => sprintHold = true;
            controls.Player.Sprint.canceled += ctx => sprintHold = false;

            controls.Player.Crouch.performed += TriggerCrouch;
            controls.Player.Jump.performed += OnJumpPressed;
        }
        void RotateTowardsInputDirection()
        {
            
            //var angle = Quaternion.EulerAngles(inputVector2);
            Quaternion targetRotation = Quaternion.LookRotation(inputVector3);
            ForwardDirection.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
            Model.transform.rotation = ForwardDirection.rotation;
        }

        float Approach(float from, float to, float rate)
        {
            if (Mathf.Abs(to - from) <= rate)
                return to;

            return from + Mathf.Sign(to - from) * rate;
        }
        Vector3 Approach(Vector3 from, Vector3 to, float rate)
        {
            float deltaX = to.x - from.x;
            float deltaY = to.y - from.y;
            float deltaZ = to.z - from.z;

            float magnitude = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
            if (magnitude <= rate)
                return to;

            float factor = rate / magnitude;
            float newX = from.x + deltaX * factor;
            float newY = from.y + deltaY * factor;
            float newZ = from.z + deltaZ * factor;

            return new Vector3(newX, newY, newZ);
        }

        [SerializeField]
        Vector3 go;
        void DeRawInput()
        {

            go = Approach(go, inputVector3, 5f * Time.deltaTime);
        }

        internal override void AirborneBehavior()
        {
            HandleWalkMovement();
        }
        internal void HandleRunMovement()
        {
            DeRawInput();
            RotateTowardsInputDirection();
            var speed = player.status.MovementSpeed.GetValue() * 2.25f;
            rigidbody.velocity = ForwardDirection.forward * speed;
        }
        internal void HandleWalkMovement()
        {
            DeRawInput();
            RotateTowardsInputDirection();
            var speed = player.status.MovementSpeed.GetValue();
            rigidbody.velocity = go * speed + rigidbody.velocity.y * transform.up;
        }
        internal void HandleIdleMovement()
        { 
            go = Approach(go, Vector3.zero, 4f * Time.deltaTime);
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

            TriggerJump();
        }

    }
}
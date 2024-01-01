using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class StateMachine
    {  
        public Entity entity { get; private set; }

        //public Vector2 Velocity { get; set; }

        public bool IsBusy { get; set; }
        public bool IsGrounded => throw new NotImplementedException();
        public bool IsMoving { get; set; }
        public bool IsRunning { get; set; } 
        public float CurrentSpeed => Mathf.Abs(entity.rigidbody.velocity.magnitude);


        //JUMP
         
        public bool JumpTrigger { get; internal set; }  

        public bool JumpButton { get; private set; }  



        //Dir
        public int RawInputDir { set; get; }
        public int RawVerticalInput { set; get; } 

        protected BaseState _currentState;

        public StateFactory Factory { private set; get; }

        public Vector2 AdditionalVelocity, AdditionalVelocityAdaptive;
        public float HorizontalVelocity { get; set; }  


        public Rigidbody rigidbody => entity.rigidbody;
        
        public Animator animator;
        private int _jumpGracePeriod;

        public BaseState currentState { get => _currentState; internal set => _currentState = value; }

        public string CurrentAnimation { get; internal set; }
        public string LastAnimationChangeAttempt { get; internal set; }

        public void SwitchCurrentState(RootState newState)
        {
            if (_currentState != null)
                _currentState.ExitState();

            _currentState = newState;
            _currentState.EnterState();
        } 

        public void Initialize(Entity owner)
        {
            entity = owner; 
            animator ??= owner.GetComponentInChildren<Animator>();
            Factory = new StateFactory(this);
        }

        public void OnStart()
        {
            //SwitchCurrentState(Factory.Grounded()); 
        }

        public void OnUpdate()
        {
            //UpdateJumpVariables();
            UpdateMovementVariable();
            _currentState.UpdateStates();
            UpdateDebugText();
        }

        public void OnFixedUpdate()
        {

        }


        private void UpdateMovementVariable()
        {
           // IsRunning = Input.GetButton("Sprint");
            //RawInputDir = (int)Input.GetAxisRaw("Horizontal");
            //RawVerticalInput = (int)Input.GetAxisRaw("Vertical");
            IsMoving = RawInputDir != 0;
        }

        internal void HandleJump()
        {

        } 
        internal bool CanJump() => _jumpGracePeriod > 0; 
        internal void PurgeJumpGracePeriod() => _jumpGracePeriod = 0;

        void UpdateDebugText()
        {
            var output = "Current Animation: " + CurrentAnimation + '\n';
            output += "Last Animation Change Attempt: " + LastAnimationChangeAttempt + '\n';

            output += currentState.ToString();

            output += $"\n IsMoving: {IsMoving}";
            output += $"\n IsRunning: {IsRunning}";

           // if (debugText)
           //     debugText.text = output;
        } 

        public void ResetAnimation()
        {
            throw new NotImplementedException();
            /*
            if (IsGrounded)
                SwitchCurrentState(Factory.Grounded());
            else
                SwitchCurrentState(Factory.Airborne());*/
        }

    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public abstract class StateMachine : BritoBehavior
    {
        public Entity entity;

        //public Vector2 Velocity { get; set; }

        public bool IsBusy { get; set; }
        public bool IsGrounded { get => groundedController.IsGrounded(); }
        public GroundedController groundedController;

        public bool IsMoving { get; set; }
        public bool IsRunning { get; set; } 
        public float CurrentSpeed => Mathf.Abs(entity.rigidbody.velocity.magnitude);

        //Dir
        public int RawInputDir { set; get; }
        public int RawVerticalInput { set; get; } 

        protected BaseState _currentState;

        public StateFactory Factory { private set; get; }   
        public new Rigidbody rigidbody => entity.rigidbody;
        
        public Animator animator;
        private int _jumpGracePeriod;

        public BaseState currentState { get => _currentState; internal set => _currentState = value; }

        public string CurrentAnimation { get; internal set; }
        public string LastAnimationChangeAttempt { get; internal set; }

        public void SwitchCurrentState(RootState newState)
        {
            _currentState?.ExitState();

            _currentState = newState;
            _currentState.EnterState();
        } 

        public void Initialize(Entity owner)
        {
            entity = owner;
            AssignFactory(); 
        }

        /// <summary>
        /// Welp, makesure to assign a factory to the use.
        /// </summary>
        public abstract void AssignFactory(); 
        public abstract void OnStart();
        public void Start()
        {
            SwitchCurrentState(Factory.Default());
            OnStart();
        }

        public abstract void OnUpdate();
        public void Update()
        { 
            _currentState?.UpdateStates();
            OnUpdate();
        }

        public abstract void OnFixedUpdate();
        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

        internal bool CanJump() => _jumpGracePeriod > 0; 
        internal void PurgeJumpGracePeriod() => _jumpGracePeriod = 0;

        public string UpdateDebugText()
        {
            var output = "Current Animation: " + CurrentAnimation + '\n';
            output += "Last Animation Change Attempt: " + LastAnimationChangeAttempt + '\n';

            output += currentState.ToString();

            output += $"\n IsMoving: {IsMoving}";
            output += $"\n IsRunning: {IsRunning}"; 
            return output;
        } 

        public virtual void ResetAnimation()
        {
            SwitchCurrentState(Factory.Default());
        }

    }
}

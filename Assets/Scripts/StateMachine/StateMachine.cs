using Game.Abilities;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.StateMachine
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
        [field: SerializeField]
        public Transform ForwardDirection { set; get; }
        [field: SerializeField]
        public GameObject Model { set; get; }

        protected BaseState _currentState;

        public StateFactory Factory { protected set; get; }
        public new Rigidbody rigidbody;
        
        private Animator animator;
        private int _jumpGracePeriod;

        public BaseState currentState { get => _currentState; internal set => _currentState = value; }

        public string CurrentAnimation { get; internal set; }
        public string LastAnimationChangeAttempt { get; internal set; } 
        public void SwitchRootState(RootState newState)
        {
            _currentState?.GetRootState().ExitState();

            _currentState = newState;
            _currentState.EnterState();
        } 

        public void Initialize(Entity owner)
        {
            entity = owner;
            AssignFactory();
            animator = entity.animator;
            rigidbody = entity.rigidbody;

            
        }

        /// <summary>
        /// Welp, makesure to assign a factory to the use.
        /// </summary>
        public abstract void AssignFactory(); 
        public abstract void OnStart();
         

        public void Start()
        {
            SwitchRootState(Factory.Default());
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

        internal virtual bool CanJump()
        {
            //if (_jumpGracePeriod > 0) return false;
            if (!IsGrounded) return false;
            else if (currentState.GetType().IsSubclassOf(typeof(EntityGroundedSubStateBase))) return false;
            //if (currentState is not EntityGroundedSubStateBase) return false;
            else if (currentState is EntityJumpState) return false;
            return true;
        }
        internal void PurgeJumpGracePeriod() => _jumpGracePeriod = 0;

        public void UpdateDebugText()
        {
            var output = "Current Animation: " + CurrentAnimation + '\n';
            output += "Last Animation Change Attempt: " + LastAnimationChangeAttempt + '\n';

            output += currentState.ToString();

            output += $"\n IsMoving: {IsMoving}";
            output += $"\n IsRunning: {IsRunning}";
            DebugText.Log(output);
        } 

        public virtual void ResetState()
        {
            print("Resetting State");
            SwitchRootState(Factory.Default());
        }

        internal virtual void AirborneBehavior()
        {
            
            //throw new NotImplementedException();
        }

        internal virtual void ApplyJumpForce()
        {
            float jumpVelocity = Mathf.Sqrt(2f * Physics.gravity.magnitude * entity.status.JumpHeight);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpVelocity, rigidbody.velocity.z);
        }

        internal virtual void TriggerJump()
        {
            if (CanJump())
            {
                currentState.GetRootState().TriggerState(Factory.Jump());
            }
            //print("nay. you may not jump");
        }

        internal Animator GetAnimator() => animator;

        /// <summary>
        /// Only enter a state if the player is doing something
        /// </summary>
        internal virtual bool TryEnterNoneIdleState()
        {
            if (IsMoving)
            {
                ResetState(); 
                return true;
            }

            return false;
        }
    }
}

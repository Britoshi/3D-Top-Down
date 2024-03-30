using Game.Abilities;
using Game.Items;
using UnityEngine; 

namespace Game.StateMachine
{
    public abstract class StateMachine : BritoBehavior
    {
        public Entity entity;

        //public Vector2 Velocity { get; set; }
        public bool movementOverride, rotationOverride;
        public bool IsBusy { get; set; }
        public bool IsGrounded { get => groundedController.IsGrounded(); }
        public GroundedController groundedController;

        public bool IsMoving { get; set; }
        public bool IsRunning { get; set; }

        [SerializeField]
        internal Vector2 inputVector2;
        [SerializeField]
        internal Vector3 inputVector3, lastInput3;

        [field: SerializeField] public Transform ForwardDirection { set; get; }
        [field: SerializeField] public GameObject Model { set; get; }

        protected BaseState _currentState;

        public StateFactory Factory { protected set; get; }
        public new Rigidbody rigidbody;
        
        private Animator animator;
        private const string HURT_ANIMATION_NAME = "Hurt";

        public void AssertInput(float x, float y)
        {
            inputVector2 = new Vector2(x, y).normalized;
            inputVector3 = new(inputVector2.x, 0, inputVector2.y);
            lastInput3 = new(inputVector3.x, inputVector3.y, inputVector3.z); 
        }
        public WeapnHoldType? GetWeaponAnimationType()
        {
            var weapon = entity.inventory.weapon.equipment;
            if (weapon == null) return null;
            return (weapon as Weapon).holdType;
        }
        public string GetBasicAnimationNamePrefix()
        {
            var slot = entity.inventory.weapon;
            if (slot.IsEmpty) return "";
            return (slot.equipment as Weapon).AnimationPrefix + " ";
        }
        
        public BaseState currentState { get => _currentState; internal set => _currentState = value; }

        public string CurrentAnimation { get; internal set; }
        public string LastAnimationChangeAttempt { get; internal set; } 
        public void SwitchRootState(RootState newState)
        {
            _currentState?.GetRootState().ExitState();

            _currentState = newState;
            _currentState.EnterState();
        } 

        /// <summary>
        /// Changes state.
        /// </summary>
        /// <param name="state"></param>
        internal void Interrupt(BaseState state)
        {
            _currentState.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }

        public void Initialize(Entity owner)
        {
            entity = owner;
            AssignFactory();
            animator = entity.animator;
            rigidbody = entity.rigidbody;
            movementOverride = false;
            rotationOverride = false;  
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
            //print("Resetting State");
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

        public void PlayHurtAnimation()
        {
            animator.Play(HURT_ANIMATION_NAME, 4);
        }
    }
}

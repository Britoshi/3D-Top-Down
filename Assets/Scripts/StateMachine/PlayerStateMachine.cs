using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text debugText;


        private float _wallJumpGracePeriod;
        private float _jumpGracePeriod;

        public readonly float JUMP_GRACE_PERIOD_CONSTANT = .35f;
        public PlayerEntity Player { get; private set; }

        //public Vector2 Velocity { get; set; }

        public bool IsBusy { get; set; }
        public bool IsGrounded => throw new NotImplementedException();
        public bool IsMoving { get; set; }
        public bool IsRunning { get; set; }
        public int VelocityDir => (int)Mathf.Sign(Player.rb.velocity.x);
        public float CurrentSpeed => Mathf.Abs(Player.rb.velocity.x);


        public float SprintMovementMultiplier => 1.5f;
        //public float AdditionalMovement => 1f + sprinting * (MovementMultiplier - 1f);

        public float MaximumMovementSpeed => Player.Stats[StatID.MOVEMENT_SPEED].GetValue();
        public float MovementSpeedGainSensitivty => 15f;
        public float MovementForceGravity => 25f;
        public float MovementOppositeForceGravity => 30f;

        //JUMP

        public float JumpTargetShort { get; set; }
        public float JumpTargetLong { get; set; }
        public bool ButtonUpFailSafe { get; set; }

        public float JUMP_GRAVITY_CONSTANT => -2f * Physics2D.gravity.y * GravityScale;
        public bool JumpTrigger { get; internal set; }
        public bool PreventLongJump { get; set; }
        public int DoubleJumpCount { get; set; }

        public int MaximumDoubleJump = 1;

        public float DistanceToShortJump { get; set; }
        public float DistanceToLongJump { get; set; }
        public float JumpVelocityShort { get; set; }
        public float JumpVelocityLong { get; set; }

        public bool JumpButton { get; private set; }
        public bool JumpButtonUp { get; private set; }
        public bool JumpButtonDown { get; private set; }
        public float GravityScale => 1f;

        

        //Dir
        public int RawInputDir { set; get; }
        public int RawVerticalInput { set; get; }

        internal float AirborneApexThreshHold = -6f;
        internal float AirborneApexThreshHoldFromAscent = 3f;

        PlayerBaseState _currentState;
        public PlayerStateFactory Factory { private set; get; }

        public Vector2 AdditionalVelocity, AdditionalVelocityAdaptive;
        public float HorizontalVelocity { get; set; }


        //Wall 
        public bool IsTouchingWall => false;// Variable2D.Or(Player.PhysicsChecker.wallCheck);

        public bool IsTouchingWallGrip => false;//Variable2D.Or(Player.PhysicsChecker.wallJumpCheck);

        public bool IsWalkingIntoWall 
        { 
            get
            {
                return false; 
            } 
        } 
        [field: SerializeField] public Vector2 WallJumpVelocity { get; set; } 


        public Rigidbody rb => Player.rb;
        public Animator animator;
        internal SpriteRenderer spriteRenderer;

        public void SetVelocityX(float value)
        {
            if (AdditionalVelocity.x != 0)
                Player.rb.velocity = new Vector2(AdditionalVelocity.x, Player.rb.velocity.y);
            else
                Player.rb.velocity = new Vector2(value, Player.rb.velocity.y);
        }

        public void SetVelocityY(float value)
        {
            Player.rb.velocity = new Vector2(Player.rb.velocity.x, value);
        }

        public PlayerBaseState CurrentState { get => _currentState; internal set => _currentState = value; }

        public string CurrentAnimation { get; internal set; }
        public string LastAnimationChangeAttempt { get; internal set; }

        public void SwitchCurrentState(PlayerRootState newState)
        {
            if (_currentState != null)
                _currentState.ExitState();

            _currentState = newState;
            _currentState.EnterState(); 
        }

        private void Awake()
        {
            Player = GetComponent<PlayerEntity>();
            animator = GetComponentInChildren<Animator>();
            Factory = new PlayerStateFactory(this);
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        }

        public void Start()
        { 
            SwitchCurrentState(Factory.Grounded());
            _wallJumpGracePeriod = 0f;
        }

        private void Update()
        {
            UpdateJumpVariables();
            UpdateMovementVariable();
            _currentState.UpdateStates();
            UpdateDebugText();
        }

        private void FixedUpdate()
        {

        }

        internal void UpdateJumpVariables()
        {

            if (Input.GetKeyDown(KeyCode.T))
                HorizontalVelocity += 30;

            JumpButton = Input.GetButton("Jump");
            JumpButtonUp = Input.GetButtonUp("Jump");
            JumpButtonDown = Input.GetButtonDown("Jump");
            JumpTrigger = JumpButtonDown;

            DistanceToShortJump = JumpTargetShort - transform.position.y;
            DistanceToLongJump = JumpTargetLong - transform.position.y;

            JumpVelocityShort = Mathf.Sqrt(JUMP_GRAVITY_CONSTANT * DistanceToShortJump);
            JumpVelocityLong = Mathf.Sqrt(JUMP_GRAVITY_CONSTANT * DistanceToLongJump);

            if (CanWallJump()) _wallJumpGracePeriod -= Time.deltaTime;
            if (CanJump()) _jumpGracePeriod -= Time.deltaTime;
        }

        public bool IsHoldingDown, IsHoldingUp;

        private void UpdateMovementVariable()
        {
            IsRunning = Input.GetButton("Sprint");
            RawInputDir = (int)Input.GetAxisRaw("Horizontal");
            RawVerticalInput = (int)Input.GetAxisRaw("Vertical");
            IsMoving = RawInputDir != 0;
            IsHoldingDown = RawVerticalInput == -1;
            IsHoldingUp = RawVerticalInput == 1;
        }

        internal void HandleJump()
        {
            var yVelocity = Player.rb.velocity.y;
            if (JumpButtonUp)
            {

                PreventLongJump = true;
                if (ButtonUpFailSafe) return;

                if (yVelocity > 0)
                {
                    if (transform.position.y >= JumpTargetShort)
                        SetVelocityY(Mathf.Sign(yVelocity) * Mathf.Sqrt(Mathf.Abs(yVelocity)));
                    else
                        SetVelocityY(JumpVelocityShort);
                }
            }
            if (JumpButton && !PreventLongJump)
            {
                //This is to check if the player reached Apex
                if (DistanceToLongJump <= .1f)
                    PreventLongJump = true;

                if (yVelocity >= 0 || IsGrounded)
                {
                    if (float.IsNaN(JumpVelocityLong))
                    {
                        throw new NotImplementedException();
                        //JumpTargetLong = transform.position.y + 1;
                        //DistanceToLongJump = JumpTargetLong - transform.position.y;
                        //JumpVelocityLong = Mathf.Sqrt(JUMP_GRAVITY_CONSTANT * DistanceToLongJump);
                    }
                    SetVelocityY(JumpVelocityLong);
                }
            }
            if (!JumpButton && !PreventLongJump)
            {
                if (yVelocity >= 0)
                {
                    if (DistanceToShortJump > 0)
                    {
                        SetVelocityY(JumpVelocityShort);
                        PreventLongJump = true;
                    }
                    else
                        SetVelocityY(Mathf.Sign(yVelocity) * Mathf.Sqrt(Mathf.Abs(yVelocity)));
                }
            }
        } 

        internal void ResetWallJumpGracePeriod() => _wallJumpGracePeriod = JUMP_GRACE_PERIOD_CONSTANT;
        internal void ResetJumpGracePeriod() => _jumpGracePeriod = JUMP_GRACE_PERIOD_CONSTANT;

        internal bool CanDoubleJump() => DoubleJumpCount < MaximumDoubleJump;
        internal bool CanWallJump() => _wallJumpGracePeriod > 0;
        internal bool CanJump() => _jumpGracePeriod > 0;

        internal void PurgeJumpGracePeriod() => _jumpGracePeriod = 0;

        void UpdateDebugText()
        {
            var output = "Current Animation: " + CurrentAnimation + '\n';
            output += "Last Animation Change Attempt: " + LastAnimationChangeAttempt + '\n';

            output += CurrentState.ToString();

            output += $"\n IsMoving: {IsMoving}";
            output += $"\n IsRunning: {IsRunning}";

            if(debugText)
                debugText.text = output;
        }

        internal void PreventWallJump()
        {
            _wallJumpGracePeriod = 0;
        }

        public void ResetAnimation()
        {
            if (IsGrounded)
                SwitchCurrentState(Factory.Grounded());
            else 
                SwitchCurrentState(Factory.Airborne());
        }
         
    }
}

namespace Game.StateMachine.Player
{
    public class AimingIdleState : AimingSubState, IHasAnimation
    {
        /// <summary>
        /// This represents a toggle of whether the idle state will only inherit the idle functionalities.
        /// It seems useless right now.
        /// </summary>
        public AimingIdleState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, BaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            HandleMovement();
            return true;
        }
        public override bool FixedUpdateState()
        {
            return true;
        }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates()
        {
            if (Ctx.IsMoving)
                return SwitchState(Factory.AimMove(CurrentSuperState));
            return false; 
        }

        protected override void HandleMovement()
        {
            CTX.HandleAimIdle();
        }
    }
}
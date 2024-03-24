namespace Game.StateMachine.Player
{
    public class AimingMovingState : AimingSubState
    {
        public AimingMovingState(PStateMachine currentContext, PStateFactory entityStateFactory, BaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {

        }
        public override void EnterState()
        {
            base.EnterState();
        }
        public override bool FixedUpdateState()
        {
            return true;
        }

        public override bool UpdateState()
        {
            return base.UpdateState();
        }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates()
        {
            if (!Ctx.IsMoving)
                return SwitchState(P_Factory.AimIdle(CurrentSuperState)); 
            return false;
        }
        protected override void HandleMovement()
        {
            P_CTX.HandleAimMovement();
        }
    }
}

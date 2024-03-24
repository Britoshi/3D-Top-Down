namespace Game.StateMachine.Player
{
    public class HumanoidWalkSubState : HumanoidGroundedSubStateBase
    {
        public HumanoidWalkSubState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, BaseState superState) :
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
            /*
            if (Ctx.IsHoldingDown)
            {
                if (Ctx.IsMoving)
                    return SwitchState(Factory.Crawl(CurrentSuperState));
                return SwitchState(Factory.Crouch(CurrentSuperState));
            }*/
            if (H_CTX.IsAiming) 
                return SwitchState(H_Factory.Aiming(CurrentSuperState));
            else if (!Ctx.IsMoving)
                return SwitchState(Factory.Idle(CurrentSuperState));
            else if (Ctx.IsRunning)
                return SwitchState(Factory.Run(CurrentSuperState));
            return false;
        }
        protected override void HandleMovement()
        {
            H_CTX.HandleMovement();
        }
    }
}

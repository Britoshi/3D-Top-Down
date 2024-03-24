namespace Game.StateMachine.Player
{
    public class Aiming : HumanoidGroundedSubStateBase
    {
        public override string GetAnimationName()
        {
            return "Aim";
        }
        public Aiming(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, BaseState superState) :
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
            H_CTX.HandleAimingBehavior();
            return base.UpdateState();
        }
        public override void ExitState() { }
        public override void InitializeSubState()
        {
            /*
            if (Ctx.IsMoving)
                SetSubState(H_Factory.AimMove(this));
            else SetSubState(H_Factory.AimIdle(this));*/
        }
        public override bool CheckSwitchStates()
        {
            /*
            if (Ctx.IsHoldingDown)
            {
                if (Ctx.IsMoving)
                    return SwitchState(Factory.Crawl(CurrentSuperState));
                return SwitchState(Factory.Crouch(CurrentSuperState));
            }*/
            if (H_CTX.IsAiming) return false;

            if (!Ctx.IsMoving)
                return SwitchState(Factory.Idle(CurrentSuperState));
            else
            {
                if (Ctx.IsRunning)
                    return SwitchState(Factory.Run(CurrentSuperState));
                else return SwitchState(Factory.Walk(CurrentSuperState));
            } 
        }
        protected override void HandleMovement()
        {

        }
    }
}

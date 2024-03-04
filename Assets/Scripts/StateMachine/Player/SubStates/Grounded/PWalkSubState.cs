namespace Game.StateMachine.Player
{
    public class PWalkSubState : PGroundedSubStateBase, IHasAnimation
    {
        public virtual string GetAnimationName() => "Walk";
        public PWalkSubState(PStateMachine currentContext, PStateFactory entityStateFactory, BaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {
        }
        public override void EnterState()
        {
            //ChangeAnimation("Walk"); 
            //print("if animation dont change, this is the problem. animation not changing on state entering");
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
            if (!Ctx.IsMoving)
                return SwitchState(Factory.Idle(CurrentSuperState));
            else if (Ctx.IsRunning)
                return SwitchState(Factory.Run(CurrentSuperState));
            return false;
        }
        protected override void HandleMovement()
        {
            P_CTX.HandleWalkMovement();
        }
    }
}

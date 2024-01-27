namespace Game
{
    public class PlayerApexSubState : PlayerAirborneSubStateBase
    { 
        public PlayerApexSubState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
            base(currentContext, playerStateFactory)
        {
            ChangeAnimation("Air Apex");
        }

        
        public override void EnterState()
        {
        }
        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            return true;
        }
        public override bool FixedUpdateState()
        {
            return true;
        }
        public override void ExitState() { }
        public override void InitializeSubState()
        {
            base.InitializeSubState();
        }
        public override bool CheckSwitchStates()
        { 
            //if (Ctx.Player.rb.velocity.y > 0 &&
            //    Ctx.Player.rb.velocity.y > Ctx.AirborneApexThreshHoldFromAscent) 
            //    return SwitchState(Factory.AirborneAscend());

            //else if (Ctx.Player.rb.velocity.y < Ctx.AirborneApexThreshHold)
            //    return SwitchState(Factory.AirborneDescend()); 
            return false;
        }

    }
}

using Unity.VisualScripting;

namespace Game.StateMachine.Player
{
    public class HumanoidAirborneSubState : HumanoidAirborneSubStateBase, IHasAnimation
    {
        public virtual string GetAnimationName() => "Airborne";

        public HumanoidAirborneSubState(HumanoidStateMachine currentContext, HumanoidStateFactory playerStateFactory, BaseState superState) :
            base(currentContext, playerStateFactory, superState)
        {

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
            /*
            if (Ctx.rigidbody.velocity.y > 0 &&
                Ctx.rigidbody.velocity.y > Ctx.AirborneApexThreshHoldFromAscent) 
                return SwitchState(Factory.AirborneAscend());

            else if (Ctx.Player.rb.velocity.y < Ctx.AirborneApexThreshHold)
                return SwitchState(Factory.AirborneDescend()); */
            return false;
        }

    }
}

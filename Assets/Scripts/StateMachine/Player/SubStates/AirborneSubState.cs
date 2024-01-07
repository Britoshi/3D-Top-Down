namespace Game.StateMachine
{
    public class AirborneSubState : EntityAirborneSubState
    {
        public override string GetAnimationName() => "Airborne";
        public AirborneSubState(StateMachine currentContext, StateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {

        }
    }
}
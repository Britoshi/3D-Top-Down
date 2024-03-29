using Game.StateMachine.Player;

namespace Game.StateMachine
{
    public class HumanoidStateFactory : StateFactory
    {
        public HumanoidStateMachine context;
        public HumanoidStateFactory(HumanoidStateMachine humanoidContent) : base(humanoidContent)
        {
            this.context = humanoidContent;
        }
        public override RootState Default() => Grounded();
        public override RootState Airborne() => new AirborneState(context, this);

        public override RootState Grounded() => new GroundedState(context, this);

        public override BaseState Idle(BaseState superState) => new HumanoidIdleSubState(context, this, superState);

        public override BaseState Run(BaseState superState) => new HumanoidRunSubState(context, this, superState);

        public override BaseState Walk(BaseState superState) =>new HumanoidWalkSubState(context, this, superState);
        public override BaseState Airborne(BaseState superState) => new HumanoidAirborneSubState(context, this, superState);

        public override RootState Jump() => new HumanoidJumpState(context, this);

        public override BaseState AirborneAscend()
        {
            throw new System.NotImplementedException();
        }

        public override BaseState AirborneApex()
        {
            throw new System.NotImplementedException();
        }

        public override BaseState AirborneDescend()
        {
            throw new System.NotImplementedException();
        }

        public virtual Aiming Aiming(BaseState super) => new Aiming(context, this, super);
        public virtual AimingIdleState AimIdle(BaseState super) =>
            new(context, this, super);
        public virtual AimingMovingState AimMove(BaseState super) =>
            new(context, this, super);

        public override RootState Stagger() => new Stagger(_context, this);
    }
}
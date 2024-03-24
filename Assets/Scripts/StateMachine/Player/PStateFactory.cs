using Game;
using Game.Abilities;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Game.StateMachine.Player
{
    public enum PState
    {
        Grounded,
        Idle, Walk, Run,
        Jump,
        Airborne,
        AirborneAscend, AirborneApex, AirborneDescend
    }

    public class PStateFactory : HumanoidStateFactory
    {
        protected new PStateMachine context;

        public PStateFactory(PStateMachine currentContext) : base(currentContext)
        {
            context = currentContext;
        }

        public override RootState Default() => Grounded();
        public override RootState Airborne() =>
            new AirborneState(context, this);
        public override Aiming Aiming(BaseState superState) =>
            new Aiming(context, this, superState); 

        public override RootState Grounded() =>
            new GroundedState(context, this); 

        public override BaseState Idle(BaseState superState) =>
            new HumanoidIdleSubState(context, this, superState);

        public override BaseState Run(BaseState superState) =>
            new HumanoidRunSubState(context, this, superState);

        public override BaseState Walk(BaseState superState) =>
            new HumanoidWalkSubState(context, this, superState);
        public override BaseState Airborne(BaseState superState) =>
            new HumanoidAirborneSubState(context, this, superState);

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

        public override AimingIdleState AimIdle(BaseState super) =>
            new(context, this, super);
        public override AimingMovingState AimMove(BaseState super) =>
            new(context, this, super);
    }
}

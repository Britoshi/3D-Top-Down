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

    public class PStateFactory : StateFactory
    {
        protected PStateMachine context;

        public PStateFactory(PStateMachine currentContext) : base(currentContext)
        {
            context = currentContext;
        }

        public override RootState Airborne() =>
            new AirborneState(context, this);

        public override RootState Default() => Grounded(); 

        public override RootState Grounded() =>
            new GroundedState(context, this); 

        public override BaseState Idle(BaseState superState) =>
            new PIdleSubState(context, this, superState);

        public override BaseState Run(BaseState superState) =>
            new PRunSubState(context, this, superState);

        public override BaseState Walk(BaseState superState) =>
            new PWalkSubState(context, this, superState);
        public override BaseState Airborne(BaseState superState) =>
            new PAirborneSubState(context, this, superState);

        public override RootState Jump() => new PJumpState(context, this);

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
    }
}

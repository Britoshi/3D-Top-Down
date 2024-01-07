using Game;
using Game.Abilities;
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
            new EntityIdleSubState(context, this, superState);

        public override BaseState Run(BaseState superState) =>
            new EntityRunSubState(context, this, superState);

        public override BaseState Walk(BaseState superState) =>
            new EntityWalkSubState(context, this, superState);
        public override BaseState Airborne(BaseState superState) =>
            new EntityWalkSubState(context, this, superState);
    }
}

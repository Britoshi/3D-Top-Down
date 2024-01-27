
using Game.Abilities;
using Game;
using System.Collections.Generic;
namespace Game.StateMachine
{
    public abstract class StateFactory
    {
        public SortedDictionary<string, BaseState> library;

        protected StateMachine _context;

        public StateFactory(StateMachine currentContext)
        {
            _context = currentContext;
            library = new SortedDictionary<string, BaseState>();
        }

        public abstract RootState Default();
        public abstract RootState Grounded();
        public abstract RootState Airborne();
        public abstract BaseState Idle(BaseState superState);
        public abstract BaseState Walk(BaseState superState);
        public abstract BaseState Run(BaseState superState);
        public abstract BaseState Airborne(BaseState superState);

        public abstract RootState Jump();// => new PlayerJumpState(_context, this);
        public abstract BaseState AirborneAscend();// => new PlayerAscendingSubState(_context, this);
        public abstract BaseState AirborneApex();// => new PlayerApexSubState(_context, this);
        public abstract BaseState AirborneDescend();// => new PlayerDescendingSubState(_context, this);


        /*
        public PlayerBaseState Idle(PlayerBaseState superState, bool standAlone = false) => new PlayerIdleState(_context, this, superState, standAlone);
        public PlayerBaseState Walk(PlayerBaseState superState) => new PlayerWalkState(_context, this, superState);
        public PlayerBaseState Run(PlayerBaseState superState) => new PlayerRunState(_context, this, superState);
        public PlayerRootState Jump() => new PlayerJumpState(_context, this);
        public PlayerRootState Grounded() => new PlayerGroundedState(_context, this);
        public PlayerRootState Airborne() => new PlayerAirborneState(_context, this);
        public PlayerBaseState AirborneAscend() => new PlayerAscendingSubState(_context, this);
        public PlayerBaseState AirborneApex() => new PlayerApexSubState(_context, this);
        public PlayerBaseState AirborneDescend() => new PlayerDescendingSubState(_context, this);

        public PlayerRootState Ability(BaseAbility ability) => new PlayerAbilityState(_context, this, ability);
        public PlayerRootState Wall() => new PlayerWallState(_context, this);

        public PlayerBaseState Crawl(PlayerBaseState superState) => new PlayerCrawlState(_context, this, superState);
        public PlayerBaseState Crouch(PlayerBaseState superState) => new PlayerCrouchState(_context, this, superState);
        */
    }
}
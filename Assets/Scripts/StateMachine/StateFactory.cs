
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
        public abstract RootState Stagger();
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

        public virtual EntityAbilityState Ability(NAbility ability) =>
            new(ability, _context, this);
        public virtual FillerState Filler(string name, bool interruptable) => new(_context, this, name, interruptable);
    }
}
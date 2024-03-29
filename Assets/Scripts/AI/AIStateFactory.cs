using Game.StateMachine;
using System;
using System.Collections.Generic;

namespace Game.AI
{
    public class AIStateFactory : BritoObject
    {

        protected AIStateMachine _context;

        public AIStateFactory(AIStateMachine currentContext)
        {
            _context = currentContext;  
        }

        public virtual AIState Default() => Wander();

        public virtual AIWanderAround Wander() => new AIWanderAround(_context, this);
        public virtual AIChaseEnemy Chase(Entity target) => new AIChaseEnemy(_context, this, target);
    }
}
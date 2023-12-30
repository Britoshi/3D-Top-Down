using Game;

using System; 

namespace Game.Utilities
{
    public abstract class OnKillAffectHandler : BritoBehavior
    {
        [NonSerialized] public OnHitAffect affect;

        public Status deadVictim, killer;
        public bool Triggered = false;
        public void Trigger(Status victim, Status killer, OnHitAffect affect)
        {
            this.deadVictim = victim;
            this.killer = killer;
            this.affect = affect;
            OnTrigger();
        }
        public abstract void OnTrigger();
    }
}
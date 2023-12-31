using System;

namespace Game.Abilities
{
    public class AbilityCost
    {
        protected Entity Owner;
        protected ResourceAffect[] Costs;

        public AbilityCost(Entity owner, ResourceAffect[] costs)
        {
            Owner = owner;
            Costs = costs;
            //foreach (var cost in Costs)
            //    cost.Init(owner);
        }

        public bool CanCast()
        {
            throw new NotImplementedException();
            //foreach (var cost in Costs)
            //    if (!cost.CanDeduct()) return false;
            //return true;
        }

        public void Deduct()
        {
            throw new NotImplementedException();
            //foreach (var cost in Costs)
            //    cost.Deduct();
        }

        public static AbilityCost None => throw new NotImplementedException();// new AbilityCost(null, null); }
        //public static AbilityCost SimpleCost(EntityBase owner, ResourceStat stat, float amount) =>
        //    new AbilityCost(owner, new Cost[1] { new Cost(stat, amount) }); 
        //public static AbilityCost SimplePercentCost(EntityBase owner, ResourceStat stat, float amount) =>
        //    new AbilityCost(owner, new Cost[1] { new CostPercent(stat, amount) });
    }
}

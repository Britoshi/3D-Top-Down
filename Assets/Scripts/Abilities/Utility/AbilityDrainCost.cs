/*
using Game.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Abilities
{
    public class AbilityDrainCost : AbilityCost
    {
        protected Cost[] HoldCosts;
        public AbilityDrainCost(EntityBase owner, Cost[] costs, Cost[] holdCosts) : base(owner, costs)
        {
            HoldCosts = holdCosts;
        }

        public bool CanHold()
        { 
            foreach (var cost in HoldCosts)
                if (!cost.CanDeduct()) return false;
            return true;
        }

        public void Drain()
        {
            foreach (var cost in HoldCosts)
                cost.Deduct();
        }

    }
}
*/
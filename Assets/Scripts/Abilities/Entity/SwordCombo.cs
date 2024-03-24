using System;
using System.Collections.Generic;

namespace Game.Abilities
{
    public class SwordCombo : NChainedAbility
    {
        public SwordCombo(Entity owner) : base(owner,
            new() { "Sword Combo_01_1", "Sword Combo_01_2", "Sword Combo_01_3", "Sword Combo_01_4" }, isCastableAirborne:  false, lockMovement:true, lockJump:true, null, null)
        {

        }

        public override string GetName()
        {
            return "Sword Combo Ability";
        }

        public override bool GetAnimationRootMotion()
        {
            return true;
        }


        protected override CooldownOn ApplyCooldownOn()
        {
            return CooldownOn.START;
        }

    }
}
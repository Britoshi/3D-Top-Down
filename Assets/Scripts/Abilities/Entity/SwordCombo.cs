using System;
using System.Collections.Generic;

namespace Game.Abilities
{
    public class SwordCombo : NChainedAbility
    {
        public SwordCombo(Entity owner) : base(owner,
            new() { "Sword Combo Custom 1", "Sword Combo Custom 2", "Sword Combo Custom 3", "Sword Combo Custom 4" }, isCastableAirborne:  false, lockMovement:true, lockJump:true, null, null)
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

        public override string GetFillerAnimationName()
        {
            return GetAnimationNodeName() + " End";
        }

        protected override CooldownOn ApplyCooldownOn()
        {
            return CooldownOn.START;
        }
        internal override void Affect(Entity contact)
        {
            switch(index)
            {
                case 0:
                    Owner.PrimaryAttack(contact, 10);
                    break;
                case 1:
                    Owner.PrimaryAttack(contact, 10);
                    break;
                case 2:
                    Owner.PrimaryAttack(contact, 10);
                    break;
                case 3:
                    Owner.PrimaryAttack(contact, 10);
                    break;
            }
        }
    }
}
namespace Game.Abilities
{
    public class NChainedAbility : NAbility
    {
        public NChainedAbility(Entity owner, bool isCastableAirborne, bool lockMovement, bool lockJump, Cooldown cooldown = null, AbilityResourceCost cost = null) : base(owner, isCastableAirborne, lockMovement, lockJump, cooldown, cost)
        {
        }

        public override string GetAnimationNodeName()
        {
            throw new System.NotImplementedException();
        }

        public override bool GetAnimationRootMotion()
        {
            throw new System.NotImplementedException();
        }

        public override string GetName()
        {
            throw new System.NotImplementedException();
        }

        protected override CooldownOn ApplyCooldownOn()
        {
            throw new System.NotImplementedException();
        }
    }
}
using Game;
using Game.Abilities;

public class SecondaryAttackAbility : NAbility
{
    public SecondaryAttackAbility(Entity owner) : 
        base(owner, isCastableAirborne: false, lockMovement: true, lockJump: true, Cooldown.Create(1f))
    {

    }

    protected override CooldownOn ApplyCooldownOn() => CooldownOn.START;

    public override string GetName()
    {
        return "Secondary Attack Ability";
    }
    public override string GetAnimationNodeName()
    {
        return "attack02";
    }
    public override bool GetAnimationRootMotion()
    {
        return true;
    }
}
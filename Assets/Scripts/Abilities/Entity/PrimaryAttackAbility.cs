using Game;
using Game.Abilities;

public class PrimaryAttackAbility : NAbility
{
    public PrimaryAttackAbility(Entity owner) : 
        base(owner, isCastableAirborne: false, lockMovement: true, lockJump: true, Cooldown.Create(1f))
    {

    }

    protected override CooldownOn ApplyCooldownOn() => CooldownOn.START;

    public override string GetName()
    {
        return "Primary Attack Ability";
    }
    public override string GetAnimationNodeName()
    {
        return "attack01";
    }
    public override bool GetAnimationRootMotion()
    {
        return true;
    }
}
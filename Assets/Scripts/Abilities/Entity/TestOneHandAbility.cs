using Game;
using Game.Abilities;

public class TestOneHandAbility : NAbility
{
    public TestOneHandAbility(Entity owner) : base(owner, 
        isCastableAirborne: false, lockMovement: true, lockJump: true, 
        Cooldown.Create(1f))
    {

    }

    protected override CooldownOn ApplyCooldownOn() => CooldownOn.START;

    public override string GetName()
    {
        return "Test One Hand Ability";
    }
    public override string GetAnimationNodeName()
    {
        return "standing_1H_cast_spell_01";
    } 
    public override bool GetAnimationRootMotion()
    {
        return true;
    }
}
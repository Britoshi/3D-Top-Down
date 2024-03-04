using Game;
using Game.Abilities;

public class TestSwordAbility : NAbility
{
    public TestSwordAbility(Entity owner) : base(owner, false, Cooldown.Create(1f))
    {

    }

    protected override CooldownOn ApplyCooldownOn() => CooldownOn.START;

    public override string GetName()
    {
        return "Test Sword Ability";
    }
    public override string GetAnimationNodeName()
    {
        return "2Hand-Sword-Attack1";
    }
    public override bool GetAnimationRootMotion()
    {
        return true;
    }
}
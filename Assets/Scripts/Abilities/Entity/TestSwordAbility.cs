using Game;
using Game.Abilities;

public class TestSwordAbility : NAbility
{
    public TestSwordAbility(Entity owner) : 
        base(owner, isCastableAirborne: false, lockMovement:true, lockJump:true, Cooldown.Create(1f))
    {
        preemptiveAnimationCancelThreshHold = .3f;
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
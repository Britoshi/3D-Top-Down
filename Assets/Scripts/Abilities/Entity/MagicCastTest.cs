using Game;
using Game.Abilities;

public class MagicCastTest : NAbility
{
    public MagicCastTest(Entity owner) : base(owner,
        isCastableAirborne: false, lockMovement: false, lockJump: true,
        Cooldown.Create(0f))
    {

    }

    protected override CooldownOn ApplyCooldownOn() => CooldownOn.START;

    public override string GetName()
    {
        return "Magic Cast Test";
    }
    public override string GetAnimationNodeName()
    {
        return "Standing_1H_Magic_Attack_02";
    }
    public override bool GetAnimationRootMotion()
    {
        return false;
    }
}
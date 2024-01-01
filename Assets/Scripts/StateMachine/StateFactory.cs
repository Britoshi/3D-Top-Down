
using Game.Abilities;
using Game;

public class StateFactory
{
    StateMachine _context;

    public StateFactory(StateMachine currentContext)
    {
        _context = currentContext;
    }

    /*
    public PlayerBaseState Idle(PlayerBaseState superState, bool standAlone = false) => new PlayerIdleState(_context, this, superState, standAlone);
    public PlayerBaseState Walk(PlayerBaseState superState) => new PlayerWalkState(_context, this, superState);
    public PlayerBaseState Run(PlayerBaseState superState) => new PlayerRunState(_context, this, superState);
    public PlayerRootState Jump() => new PlayerJumpState(_context, this);
    public PlayerRootState Grounded() => new PlayerGroundedState(_context, this);
    public PlayerRootState Airborne() => new PlayerAirborneState(_context, this);
    public PlayerBaseState AirborneAscend() => new PlayerAscendingSubState(_context, this);
    public PlayerBaseState AirborneApex() => new PlayerApexSubState(_context, this);
    public PlayerBaseState AirborneDescend() => new PlayerDescendingSubState(_context, this);

    public PlayerRootState Ability(BaseAbility ability) => new PlayerAbilityState(_context, this, ability);
    public PlayerRootState Wall() => new PlayerWallState(_context, this);

    public PlayerBaseState Crawl(PlayerBaseState superState) => new PlayerCrawlState(_context, this, superState);
    public PlayerBaseState Crouch(PlayerBaseState superState) => new PlayerCrouchState(_context, this, superState);


    */
}
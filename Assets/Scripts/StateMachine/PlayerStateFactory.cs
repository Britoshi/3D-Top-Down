using Game;
using Game.Abilities;
using System.Collections.Generic;
using UnityEditor;

namespace Game
{
    public enum PlayerState
    {
        Grounded, 
        Idle, Walk, Run, 
        Jump,
        Airborne, 
        AirborneAscend, AirborneApex, AirborneDescend
    }

    public class PlayerStateFactory
    {
        PlayerStateMachine _context; 

        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext; 
        }

        public PlayerBaseState Idle(PlayerBaseState superState, bool standAlone = false) => new PlayerIdleState(_context, this, superState, standAlone);
        public PlayerBaseState Walk(PlayerBaseState superState) => null;// new PlayerWalkState(_context, this, superState);  
        public PlayerBaseState Run(PlayerBaseState superState) => null;// new PlayerRunState(_context, this, superState);  
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
    }
}

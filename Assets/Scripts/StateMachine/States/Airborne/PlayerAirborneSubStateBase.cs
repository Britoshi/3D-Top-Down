using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class PlayerAirborneSubStateBase : PlayerBaseState
    {
        protected PlayerAirborneSubStateBase(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) : base(currentContext, entityStateFactory)
        {
            InitializeSubState();
        }

        public override void InitializeSubState()
        { 
            if (!Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Idle(this));
            else if (Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Walk(this));
            else
                SetSubState(Factory.Run(this));
        }
    }
}

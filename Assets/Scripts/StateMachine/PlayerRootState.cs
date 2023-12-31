using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public abstract class PlayerRootState : PlayerBaseState
    {
        protected PlayerRootState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) : base(currentContext, entityStateFactory)
        {
            IsRootState = true;
        }

        public override bool UpdateState()
        {
            HandlePrevApplyVariable();
            ApplyVelocity();
            return true;
        }

        protected void HandlePrevApplyVariable()
        {

        }

        protected void ApplyVelocity()
        {  
            Ctx.SetVelocityX(Ctx.HorizontalVelocity);
        }

    }
}

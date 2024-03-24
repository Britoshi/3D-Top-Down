using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.StateMachine.Player
{
    public abstract class HumanoidAirborneSubStateBase : EntityAirborneSubStateBase
    {
        protected HumanoidAirborneSubStateBase(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, BaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {
            InitializeSubState();
        }

        public override void InitializeSubState()
        {
            /*
            if (!Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Idle(this));
            else if (Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Walk(this));
            else
                SetSubState(Factory.Run(this));*/
        }
    }
}

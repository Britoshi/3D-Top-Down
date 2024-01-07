using Game; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.StateMachine
{
    public class EntityRunSubState : EntityGroundedSubStateBase, IHasAnimation
    {
        public virtual string GetAnimationName() => "run";
        //protected override float MaxSpeed => Ctx.MaximumMovementSpeed * Ctx.SprintMovementMultiplier;
        public EntityRunSubState(StateMachine currentContext, StateFactory entityStateFactory, BaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {
        }
        public override void EnterState()
        {
            //ChangeAnimation("Run");
        }
        public override bool UpdateState()
        {
            return base.UpdateState(); 
        }
        public override bool FixedUpdateState()
        {
            return true;
        }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates()
        {
            /*
            if (Ctx.IsHoldingDown)
            {
                if (Ctx.IsMoving)
                    return SwitchState(Factory.Crawl(CurrentSuperState));
                return SwitchState(Factory.Crouch(CurrentSuperState));
            }*/
            if (!Ctx.IsMoving)
                return SwitchState(Factory.Idle(CurrentSuperState));
            else if (Ctx.IsMoving && !Ctx.IsRunning)
                return SwitchState(Factory.Walk(CurrentSuperState)); 
            return false;
        } 
    }
}

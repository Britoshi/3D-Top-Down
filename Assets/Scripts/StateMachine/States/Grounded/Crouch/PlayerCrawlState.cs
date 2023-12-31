using Game; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class PlayerCrawlState : PlayerMovementBaseSubState
    { 
        //protected override float MaxSpeed => Ctx.MaximumMovementSpeed * Ctx.SprintMovementMultiplier * .35f;
        public PlayerCrawlState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, PlayerBaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {

        }


        void OnCrouch()
        {
            throw new NotImplementedException();
            //Ctx.Player.EntityCollider.Crouch();
        }

        void OnStandUp()
        {

            throw new NotImplementedException();
            //Ctx.Player.EntityCollider.Uncrouch();
        }

        public override void EnterState()
        {
            OnCrouch();
            ChangeAnimation("Crawl"); 
        }
        public override bool FixedUpdateState()
        {

            return true;
        }

        public override bool UpdateState()
        {

            return base.UpdateState(); 
        }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates() 
        {
            if (Ctx.IsHoldingDown)
            {
                if (Ctx.IsMoving) return false;
                return SwitchState(Factory.Crouch(CurrentSuperState));
            }

            OnStandUp();

            if (!Ctx.IsMoving)
                return SwitchState(Factory.Idle(CurrentSuperState));
            else if (Ctx.IsMoving && Ctx.IsRunning)
                return SwitchState(Factory.Run(CurrentSuperState));
            return SwitchState(Factory.Walk(CurrentSuperState));
        } 
    }
}

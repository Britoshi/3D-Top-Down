using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Abilities
{
    public abstract class NChainedAbility : NAbility
    {
        public const float CHAIN_TIMEOUT = 5f; 
        public float timeoutCountDown;

        public List<string> animationList;
        public int index;

        public NChainedAbility(Entity owner, List<string> animations, bool isCastableAirborne, bool lockMovement, bool lockJump, Cooldown cooldown = null, AbilityResourceCost cost = null) : base(owner, isCastableAirborne, lockMovement, lockJump, cooldown, cost)
        {
            index = 0;
            animationList = animations;
        }

        public override AbilityCastTryResult CanCast(bool queueAbility = false)
        {
            //perhaps check if the animation is able? 
            if (Owner.abilityController.IsCasting)
            {
                return AbilityCastTryResult.Queue("Already Casting");
            }
            if (!cooldown.CanCast())
            {
                if(timeoutCountDown <= 0)
                    return AbilityCastTryResult.Fail("Ability On Cooldown.");
            }
            else if (!Cost.CanDeduct())
                return AbilityCastTryResult.Fail("Not Enough Resources.");
            return AbilityCastTryResult.Success();
        }
        public override void PassiveUpdate()
        {
            base.PassiveUpdate();
            if(index != 0 && !IsCasting)
            {
                if(timeoutCountDown > 0)
                {
                    timeoutCountDown -= Time.deltaTime;
                }
                else
                {
                    index = 0;
                    if(ApplyCooldownOn() == CooldownOn.END)
                    { 
                        cooldown.ApplyCooldown();
                        print("Applied Cooldown Ran out of time");
                    }
                }
            }
        }

        public override string GetAnimationNodeName()
        {
            return animationList[index];
        }
        public override void OnAbilityCast()
        {
            base.OnAbilityCast();
            timeoutCountDown = CHAIN_TIMEOUT;
        }

        public override void OnAbilityUpdate()
        {
            base.OnAbilityUpdate();

        }

        public override void OnAbilityEnd()
        {
            base.OnAbilityEnd();
            index++;
            if (index >= animationList.Count)
                index = 0;
            else timeoutCountDown = CHAIN_TIMEOUT;
        }
        #region Animation Functions #Only put animation related function.
        public override void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animationProgress = 0;
            if (ApplyCooldownOn() == CooldownOn.START && index == 0)
            {
                cooldown.ApplyCooldown();
                print("Applied Cooldown Start");
            }
        }
        public override void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animationProgress = stateInfo.normalizedTime % 1;
        }
        public override void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            print(index);
            if (ApplyCooldownOn() == CooldownOn.END && index == animationList.Count - 1)
            {
                cooldown.ApplyCooldown();
                print("Applied Cooldown End");
            }

            animationProgress = -1;

            //This  should onlybe if it is standalone
            Owner.abilityController.SetAbility(null);
            //print("Ability Animation ended normally", GetName());
            Owner.stateMachine.ResetState();
        }
        #endregion
    }
}
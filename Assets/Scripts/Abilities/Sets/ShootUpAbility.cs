/*
using Game.Abilities; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{

    internal class ShootUpAbility : HoldAbility
    {
        public new KineticEntity Owner;

        public ShootUpAbility(KineticEntity owner) : 
            base(owner, "Shoot Up", castDuration: 5f, CooldownOn.END, 
                new AbilityDrainCost(owner, Utility.Cost.Create(owner, ResourceStat.MP, 5), Utility.Cost.Create(owner, ResourceStat.MP, 5f))
                , Cooldown.Create(5f))
        {
            Owner = owner;
        }
         
        public override void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationStart(animator, stateInfo, layerIndex);
            Owner.ResetVelocity();
        }

        public override void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationUpdate(animator, stateInfo, layerIndex);
             
            Owner.SetVelocityY(10f);
        }

        public override void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationEnd(animator, stateInfo, layerIndex);
        }
    }
}
*/
/*
using Game.Abilities;
using Game.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class RollAbility : BaseAbility
    {
        public new KineticEntity Owner;
        public RollAbility(KineticEntity owner) : base(owner, "Roll", cooldown: Abilities.Cooldown.Create(.75f), cost:null)
        {
            Owner = owner;
            MovementOverride = true;
        }

        public override void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationStart(animator, stateInfo, layerIndex);
            Owner.ResetVelocity();
        }

        public override void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationUpdate(animator, stateInfo, layerIndex); 

            var add = (1 - stateInfo.normalizedTime) * 3f;
            var newVelocity = (Owner.FacingDir * (add + 1.5f)) * stateInfo.speed * stateInfo.speedMultiplier; 
            Owner.SetVelocityX(newVelocity); 
        }

        public override void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationEnd(animator, stateInfo, layerIndex);
        } 
    }
}
*/
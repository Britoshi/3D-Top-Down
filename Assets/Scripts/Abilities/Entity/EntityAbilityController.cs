using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Abilities
{
    [RequireComponent(typeof(Entity))]
    public class EntityAbilityController : BritoBehavior
    {
        private Animator animator;

        public Entity entity;
        public List<NAbility> 
            activeAbilities, 
            passiveAbilities;

        public NAbility currentAbility;

        public bool IsUsingAbility => currentAbility != null;

        public void Initialize(Entity self)
        {
            entity = self;
            animator = self.animator;

        }

        public void Update()
        {
            currentAbility?.OnAbilityUpdate();
        }

        public void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            currentAbility.OnAnimationStart(animator, stateInfo, layerIndex);
        }
        public void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            currentAbility.OnAnimationUpdate(animator, stateInfo, layerIndex);
        }
        public void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            currentAbility.OnAnimationEnd(animator, stateInfo, layerIndex);
        }

        public void CancelAbility()
        {
            throw new NotImplementedException();
        }
    }
}
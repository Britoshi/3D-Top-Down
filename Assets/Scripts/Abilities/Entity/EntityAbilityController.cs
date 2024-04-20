using System;
using System.Collections;
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

        public SortedDictionary<string, NAbility> abilities;

        public const float QUEUE_EXPIRE_TIME = .5f;
        public NAbility abilityQueue;
        float abilityQueueExpireTimer;
        private NAbility currentAbility;
        public NAbility CurrentAbility =>  currentAbility;
        public void SetAbility(NAbility ability) => currentAbility = ability;
        
        private NAbility primaryAbility;
        internal bool IsCasting => currentAbility != null;

        public bool IsUsingAbility => currentAbility != null;

        public bool interrupted;

        private void Awake()
        {
            abilities = new();
        }

        public void Initialize(Entity self)
        {
            entity = self;
            animator = self.animator;  
        }
        public void Cast(NAbility ability, bool skipCheck = false)
        {
            if (ability == null) return;
            //print("CAST  CALL  ", ability.GetName());
            var castResult = ability.TryCast(skipCheck);
            if (castResult.result == AbilityResultType.SUCCESS)
            {
                currentAbility = ability;
            }
            else
            {
                DebugText.Log(castResult.message);
            }

        }

        public void TriggerPrimaryAttack()
        {
            Cast(primaryAbility);
        }

        public void Update()
        { 
            if (GameSystem.Paused) return;

            currentAbility?.OnAbilityUpdate();

            foreach (var ability in abilities.Values)
                ability.PassiveUpdate();


            /*
            if (abilityQueue != null)
            {
                abilityQueueExpireTimer -= Time.deltaTime;
                if (abilityQueueExpireTimer < 0) abilityQueue = null;
            }*/
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

        public void RegisterAbility(NAbility ability)
        {
            abilities[ability.GetName()] = ability;
        } 
        public void TriggerAbilityQueue()
        { 
            entity.stateMachine.ResetState();
        }

        public void SetPrimaryAbility(NAbility ability) 
        {
            primaryAbility = ability;
        }

        internal void ClearPrimaryAbility()
        {
            primaryAbility = null;
        }
    }

    
} 
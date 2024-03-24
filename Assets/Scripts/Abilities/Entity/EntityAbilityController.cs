using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

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

        NAbility testAbility, pAttack, sAttack;
        internal bool IsCasting => currentAbility != null;

        public bool IsUsingAbility => currentAbility != null;
        private void Awake()
        {
            abilities = new();
        }

        public void Initialize(Entity self)
        {
            entity = self;
            animator = self.animator;
            testAbility = new TestSwordAbility(self);
            pAttack = new SwordCombo(self);
            sAttack = new SecondaryAttackAbility(self);
        }
        public void Cast(NAbility ability, bool skipCheck = false)
        {
            print("CAST  CALL  ", ability.GetName());
            var castResult = ability.TryCast(skipCheck);
            if (castResult.result == AbilityResultType.SUCCESS)
            {
                currentAbility = ability;
            }
            else
            {
                //Lmao spagettiiii
                //if (castResult.result == AbilityResultType.QUEUE)
                //    QueueAbility(ability);

//else 
                    DebugText.Log(castResult.message);
            }

        }

        public void Update()
        {
            currentAbility?.OnAbilityUpdate();

            foreach (var ability in abilities.Values)
                ability.PassiveUpdate();

            if (GameSystem.Paused) return;
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (testAbility.TryCast().result == AbilityResultType.SUCCESS)
                {
                    currentAbility = testAbility;
                }
            } 
            if (Input.GetButtonDown("Fire1"))
                Cast(pAttack);

            if (abilityQueue != null)
            {
                abilityQueueExpireTimer -= Time.deltaTime;
                if (abilityQueueExpireTimer < 0) abilityQueue = null;
            }
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
        bool triggerAbilityQueue;
        public void TriggerAbilityQueue()
        {
            triggerAbilityQueue = true;
            entity.stateMachine.ResetState();
        }



    }

    
} 
using System;
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

        public  SortedDictionary<string, NAbility> abilities;

        public const float QUEUE_EXPIRE_TIME = .5f;
        public NAbility abilityQueue;
        float abilityQueueExpireTimer;
        public NAbility currentAbility;

        NAbility testAbility,  pAttack,sAttack;
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
            testAbility  = new TestSwordAbility(self);
            pAttack =  new PrimaryAttackAbility(self);
            sAttack = new SecondaryAttackAbility(self);
        }
        public void Cast(NAbility ability)
        {
            var castResult = ability.TryCast();
            if (castResult.result == ResultType.SUCCESS)
            {
                currentAbility = ability;
            }
            else
            {
                //Lmao spagettiiii
                if(castResult.message == "Already Casting")
                    QueueAbility(ability);
                
                else DebugText.Log(castResult.message);
            }
            
        }
        public void Update()
        {
            currentAbility?.OnAbilityUpdate();

            foreach (var ability in abilities.Values)
                ability.PassiveUpdate();

            if (Input.GetKeyDown(KeyCode.P))
            {
                if(testAbility.TryCast().result == ResultType.SUCCESS)
                {
                    currentAbility = testAbility;
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log(animator.applyRootMotion);
            }

            if (Input.GetButtonDown("Fire1"))
                Cast(pAttack);
            
            if(abilityQueue != null)
            {
                abilityQueueExpireTimer -= Time.deltaTime;
                if(abilityQueueExpireTimer < 0) abilityQueue = null; 
            }
            //else if (Input.GetButtonDown("Fire2"))
            //    Cast(sAttack);
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

        internal bool TryCastAbilityQueue()
        {
            if (abilityQueue == null) return false;  
            if (abilityQueue.TryCast().result == ResultType.SUCCESS)
            {
                print("casting a queed ability");
                currentAbility = abilityQueue;
                abilityQueue = null;
                print("new curr ability", currentAbility.GetName());
                return true;
            }
            return false;
        }

        internal void QueueAbility(NAbility ability)
        { 
            abilityQueue = ability;
            abilityQueueExpireTimer = QUEUE_EXPIRE_TIME;
            DebugText.Log("Ability Queued");
        }
    }
}
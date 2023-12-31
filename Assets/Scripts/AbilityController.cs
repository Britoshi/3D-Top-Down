using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Abilities;
using System.ComponentModel;

namespace Game
{
    public enum InputKeyBinding
    {
        [Description("INPUT PRIMARY")]
        PRIMARY,
        [Description("INPUT SECONDARY")]
        SECONDARY,
        [Description("INPUT ABILITY 1")]
        ABILITY_1,
        [Description("INPUT ABILITY 2")]
        ABILITY_2,
        [Description("INPUT ABILITY 3")]
        ABILITY_3,
        [Description("INPUT ABILITY 4")]
        ABILITY_4,
        [Description("INPUT ABILITY ULTIMATE")]
        ABILITY_ULTIMATE,
        [Description("INPUT DODGE")]
        ABILITY_DODGE
    }

    public class AbilityController : MonoBehaviour
    {



        public static AbilityController Instance { private set; get; }
        [SerializeField]
        private PlayerStateMachine _playerStateMachine;

        //Game Play
        public Dictionary<InputKeyBinding, BaseAbility> BindedAbilities;


        public static float CoolDownReduction = 1.0f;

        //Ability Using Handler
        public SortedDictionary<string, BaseAbility> abilities;

        public static BaseAbility CurrentAbility => Instance.currentAbility;

        [SerializeField]
        public BaseAbility currentAbility;

        public bool inAnimation;

        private void Awake()
        {
            Instance = this;

            BindedAbilities = new Dictionary<InputKeyBinding, BaseAbility>();

            foreach (InputKeyBinding keybinding in (InputKeyBinding[])Enum.GetValues(typeof(InputKeyBinding)))
                BindedAbilities[keybinding] = null; 

            _playerStateMachine = GetComponent<PlayerStateMachine>();
        }

        public static bool TryTriggerAbility(InputKeyBinding key, bool keyDown, bool? keyHeld = null) => 
            Instance._TryTriggerAbility(key, keyDown, keyHeld);

        private bool _TryTriggerAbility(InputKeyBinding key, bool keyDown, bool? keyHeld)
        {
            /*
            var ability = BindedAbilities[key];
            bool isHoldAbility = false;

            var activated = false;

            if (ability != null)
                isHoldAbility = ability.GetType().IsSubclassOf(typeof(HoldAbility)); 

            if (keyDown)
            {
                var bindedAbility = GetBindedKey(key);

                if (bindedAbility != null) 
                    ActivateAbility(bindedAbility, out activated); 

                if(keyHeld != null)
                    if (!activated && !isHoldAbility)
                        InputController.QueueAbility(key);
            }

            //This is a hold ability
            if (keyHeld != null)
                if (isHoldAbility)
                {  
                    (ability as HoldAbility).SetButtonReading(keyHeld.Value);
                }
            return activated;*/
            throw new NotImplementedException();
        }

        /// <summary>
        /// Activates the prompted ability, but if it is in middle of animation, return false
        /// </summary>
        /// <param name="key"></param>
        /// <returns>If it is in middle of animation, return false </returns>
        /// <exception cref="Exception"></exception>
        public static BaseAbility GetBindedKey(InputKeyBinding key)
        {
            var ability = Instance.BindedAbilities[key];
            if (ability == null)
            {
                print("Key binding not set!");
                return null;
            }
            return ability;// Instance.ActivateAbility(ability); 
        }

        public void ActivateAbility(BaseAbility ability, out bool state)
        {
            //Conditions to return false
            if (ability.CanCast() == false || _playerStateMachine.IsBusy)
            { 
                state = false;
                return;
            }

            currentAbility = ability;
            ability.OnCast();
            
            _playerStateMachine.SwitchCurrentState(_playerStateMachine.Factory.Ability(currentAbility));
            state = true;
        }

        //This is only called on animStart of @AbilityProcessor
        public static void ActivateOnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            //if (CurrentAbility.IsHoldAbility())
                //if ((CurrentAbility as HoldAbility).PreventAnimationDelegate) return;

            Instance.currentAbility.OnAnimationStart(animator, stateInfo, layerIndex);
            throw new NotImplementedException();
        }

        public static void ActivateOnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var ability = Instance.currentAbility;
            ability.OnAnimationUpdate(animator, stateInfo, layerIndex); 
        }

        //Only called at the end of @AbilityProcessor
        public static void ActivateOnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            /*
            if (CurrentAbility.IsHoldAbility()) 
                if ((CurrentAbility as HoldAbility).PreventAnimationDelegate) return; 

            Instance.currentAbility.OnAnimationEnd(animator, stateInfo, layerIndex); 
            UIController.Instance.UpdateIcon(Instance.currentAbility); */
            throw new NotImplementedException();
        }

        #region Active GamePlay Comp 

        public static void AssignAbilitySlot(InputKeyBinding key, string name)
        {
            if (!Instance.BindedAbilities.ContainsKey(key))
                throw new System.Exception("Ability " + key + " does not exist!");

            var ability = Instance.abilities[name];
            Instance.BindedAbilities[key] = ability;
            //UIController.AssignAbilitySlot(key, ability);
            throw new NotImplementedException();
        }

        #endregion


        // Start is called before the first frame update
        void Start()
        {  
            //abilities = AbilityUtility.Import(); 

            AssignAbilitySlot(InputKeyBinding.ABILITY_DODGE, "Roll");
            AssignAbilitySlot(InputKeyBinding.PRIMARY, "Chain Attack");
            AssignAbilitySlot(InputKeyBinding.ABILITY_2, "Fire Ball");
            AssignAbilitySlot(InputKeyBinding.ABILITY_3, "Shoot Up");
            throw new NotImplementedException();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //This later must be redone. What if the player puts away the ability?
            foreach (var ability in BindedAbilities.Values)
            {
                if (ability != null)
                    ability.GlobalUpdate();
            }
        }
    }
}
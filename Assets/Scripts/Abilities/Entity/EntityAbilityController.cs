using UnityEngine;

namespace Game.Abilities
{
    [RequireComponent(typeof(Entity))]
    public class EntityAbilityController : BritoBehavior
    {
        public Entity entity;

        public NAbility currentAbility;

        public bool IsUsingAbility => currentAbility != null;

        public void Initialize(Entity self)
        {
            entity = self;


        }

        public void AbilityProcessorStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
        public void AbilityProcessorUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }
        public void AbilityProcessorEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}
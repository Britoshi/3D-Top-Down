using Game;
using UnityEngine; 

public class AbilityProcessor : StateMachineBehaviour
{
    Entity entity;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(entity == null) entity = animator.GetComponent<Entity>();
        entity.abilityController.AbilityProcessorStart(animator, stateInfo, layerIndex);
    } 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        entity.abilityController.AbilityProcessorUpdate(animator, stateInfo, layerIndex);
    } 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        entity.abilityController.AbilityProcessorEnd(animator, stateInfo, layerIndex);
    } 
}

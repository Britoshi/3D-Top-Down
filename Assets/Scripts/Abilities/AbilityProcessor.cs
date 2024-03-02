using Game;
using UnityEngine; 

public class AbilityProcessor : StateMachineBehaviour
{
    Entity entity;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(entity == null) entity = animator.GetComponentInParent<Entity>();
        entity.abilityController.OnAnimationStart(animator, stateInfo, layerIndex);
    } 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        entity.abilityController.OnAnimationUpdate(animator, stateInfo, layerIndex);
    } 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        entity.abilityController.OnAnimationEnd(animator, stateInfo, layerIndex);
    } 
}

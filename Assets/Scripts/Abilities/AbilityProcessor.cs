using Game;
using UnityEngine; 

public class AbilityProcessor : StateMachineBehaviour
{
    Entity entity;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(entity == null) entity = animator.GetComponent<Entity>();
        //Debug.Log(animator.GetCurrentAnimatorClipInfo(0));
        Debug.Log("Ability Started");
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

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

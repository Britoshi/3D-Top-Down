using Game;
using Game.Abilities;
using Game.StateMachine;
using UnityEngine; 

public class AbilityProcessor : StateMachineBehaviour
{
    Entity entity;
    EntityAbilityController ability;
    public bool exited;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entity == null)
        {
            entity = animator.GetComponent<Entity>();
            ability = entity.abilityController;
        }
        //Debug.Log(animator.GetCurrentAnimatorClipInfo(0));
        //Debug.Log("Ability Started");
        entity.abilityController.OnAnimationStart(animator, stateInfo, layerIndex);
        exited = false;
    } 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        entity.abilityController.OnAnimationUpdate(animator, stateInfo, layerIndex);
    } 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (exited) return;
        entity.abilityController.OnAnimationEnd(animator, stateInfo, layerIndex);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

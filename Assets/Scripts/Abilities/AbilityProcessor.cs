using Game;
using UnityEngine;

public class AbilityProcessor : StateMachineBehaviour
{   
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AbilityController.ActivateOnStart(animator, stateInfo, layerIndex); 
    } 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AbilityController.ActivateOnUpdate(animator, stateInfo, layerIndex);
    } 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {     
        AbilityController.ActivateOnExit(animator, stateInfo, layerIndex); 
    } 
}

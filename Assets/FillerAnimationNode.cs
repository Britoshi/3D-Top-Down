using Game;
using Game.StateMachine;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;  

public class FillerAnimationNode : StateMachineBehaviour
{
    AbilityExitState abilityExitState;
    Entity entity;
    public string stateName;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(entity == null) entity = animator.GetComponent<Entity>();
        abilityExitState = entity.stateMachine.Factory.AbilityExit();
        entity.stateMachine.Interrupt(abilityExitState);
        abilityExitState.currNode = this;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        abilityExitState.currNode = null;
        if (abilityExitState.interrupted) return;
        OnExit(); 
    }
    public void OnExit()
    {
        abilityExitState.currNode = null;
        abilityExitState = null;
        entity.stateMachine.ResetState();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

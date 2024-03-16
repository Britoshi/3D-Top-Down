using UnityEngine;
using System.Collections;

public class PelvisSet : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<MecFootPlacer> ().mAdjustPelvisVertically = true;
	}
}

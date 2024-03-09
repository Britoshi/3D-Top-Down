using UnityEngine;
using System.Collections;

public class IdleUpdate : StateMachineBehaviour 
{
	private float mIdleRayCast = 0.5f;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
	{
		animator.GetComponent<MecFootPlacer>().DisablePlant(AvatarIKGoal.LeftFoot);
		animator.GetComponent<MecFootPlacer>().DisablePlant(AvatarIKGoal.RightFoot);
	}
	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		FootPlacementData lF1 = animator.GetComponents<FootPlacementData>()[0];
		FootPlacementData lF2 = animator.GetComponents<FootPlacementData>()[1];

		//setting up first foot transition time and extra ray dist check
		if (lF1 != null)
		{
			if(stateInfo.normalizedTime  > 0.25f)
			{
				lF1.mExtraRayDistanceCheck = mIdleRayCast;
			}
			else
			{
				lF1.mExtraRayDistanceCheck = 0;
			}
		}

		//setting up second foot transition time and extra ray dist check
		if (lF2 != null)
		{
			if(stateInfo.normalizedTime  > 0.25f)
			{
				lF2.mExtraRayDistanceCheck = mIdleRayCast;
			}
			else
			{
				lF2.mExtraRayDistanceCheck = 0;
			}
		}
	}
}

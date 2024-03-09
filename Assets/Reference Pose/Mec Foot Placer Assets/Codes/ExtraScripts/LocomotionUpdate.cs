using UnityEngine;
using System.Collections;

public class LocomotionUpdate : StateMachineBehaviour {

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
	{
		animator.GetComponent<MecFootPlacer>().EnablePlant(AvatarIKGoal.LeftFoot);
		animator.GetComponent<MecFootPlacer>().EnablePlant(AvatarIKGoal.RightFoot);		
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
	{

		float lCurrentSpeedFactor = animator.GetFloat("speed");
		float lTime = animatorStateInfo.normalizedTime - Mathf.Floor(animatorStateInfo.normalizedTime);
		float lBlendedTime = 0.5f - 0.25f * lCurrentSpeedFactor;

		FootPlacementData[] lFeet = animator.GetComponents<FootPlacementData>();
		FootPlacementData lFoot = null;

        //First foot setup start
        for (byte i = 0; i < lFeet.Length; i++)
       {
            switch (lFeet[i].mFootID)
            {
                case FootPlacementData.LimbID.LEFT_FOOT:
                    lFoot = lFeet[i];

                    if (lTime > 0.5 && lTime < 0.5f + lBlendedTime)
                    {
                        lFoot.mExtraRayDistanceCheck = 0.7f;
                    }
                    else
                    {
                        lFoot.mExtraRayDistanceCheck = -0.2f;
                    }                    
                    break;

                case FootPlacementData.LimbID.RIGHT_FOOT:
                    lFoot = lFeet[i];

                    //Setting up raycast extra ray dist
                    if (lTime < lBlendedTime)
                    {
                        lFoot.mExtraRayDistanceCheck = 0.7f;
                    }
                    else
                    {
                        lFoot.mExtraRayDistanceCheck = -0.2f;
                    }
                    break;

                case FootPlacementData.LimbID.LEFT_HAND:
                    lFoot = lFeet[i];
                    break;

                case FootPlacementData.LimbID.RIGHT_HAND:
                    lFoot = lFeet[i];
                    break;
            }

            //Setting up transition time
            lFoot.mTransitionTime = 0.15f - (0.1f * lCurrentSpeedFactor);            
        }	
	}
}

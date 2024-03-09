using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Entity))]
    public class EntityIKController : MonoBehaviour
    {
        Animator animator;

        public bool enableFootIK = true;
        [Range(0, 2)][SerializeField] private float heightFromGroundRayCast = 1.14f;
        [Range(0, 2)][SerializeField] private float rayCastDownDistance = 1.5f;

        public LayerMask envLayer;
        public float pelvisOffset;

        [Range(0, 1)][SerializeField] private float pelvisVerticalSpeed = 0.28f;
        [Range(0, 1)][SerializeField] private float feetToIKPositionSpeed = .5f;

        public string leftFootAnimVariableName = "LeftFootCurve",
            rightFootAnimVariableName = "RightFootCurve";

        public bool useProIKFeatures = false;
        public bool showSolverDebug = true;

        private Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
        private Quaternion leftFootIKRotation, rightFootIKRotation;
        private float lastPelvisPositionY, lastLeftFootPositionY, lastRightFootPositionY;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (enableFootIK)
            {
                AdjustFeetTarget(ref rightFootIKPosition, HumanBodyBones.RightFoot);
                AdjustFeetTarget(ref leftFootIKPosition, HumanBodyBones.LeftFoot);

                FeetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation);
                FeetPositionSolver(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation);
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (enableFootIK)
            {
                //print("working?");
                MovePelvisHeight();

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                if (useProIKFeatures)
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimVariableName));

                MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);

                //Left
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                if (useProIKFeatures)
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimVariableName));

                MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);
            }
        }

        void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
        {
            Vector3 targetIKPosition = animator.GetIKPosition(foot);

            if(positionIKHolder != Vector3.zero)
            {
                targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
                positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

                float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
                targetIKPosition.y += yVariable;
                lastFootPositionY = yVariable;

                targetIKPosition = transform.TransformPoint(targetIKPosition);
                animator.SetIKRotation(foot, rotationIKHolder);
            }
            animator.SetIKPosition(foot, targetIKPosition);
        }

        void MovePelvisHeight()
        {
            if(rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
            {
                lastPelvisPositionY = animator.bodyPosition.y; 
                return;
            }

            float lOffsetPos = leftFootIKPosition.y - transform.position.y;
            float rOffsetPos = rightFootIKPosition.y - transform.position.y;
            float totalOffset = (lOffsetPos < rOffsetPos) ? lOffsetPos : rOffsetPos;

            var newPelvisPos = animator.bodyPosition + Vector3.up * totalOffset;
            newPelvisPos.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPos.y, pelvisVerticalSpeed);

            animator.bodyPosition = newPelvisPos;
            lastPelvisPositionY = animator.bodyPosition.y;
        }

        void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPosition, ref Quaternion feetIKRotation)
        {
            RaycastHit feetOutHit;

            if (showSolverDebug)
                Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (rayCastDownDistance + heightFromGroundRayCast), Color.green);

            if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, rayCastDownDistance + heightFromGroundRayCast, envLayer))
            {
                feetIKPosition = fromSkyPosition;
                feetIKPosition.y = feetOutHit.point.y + pelvisOffset;

                feetIKRotation = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;
                return;
            }

            feetIKPosition = Vector3.zero;
        }

        void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
        {
            feetPositions = animator.GetBoneTransform(foot).position;
            feetPositions.y = transform.position.y + heightFromGroundRayCast;
        }
    }
}
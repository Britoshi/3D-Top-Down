using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class EntityAnimationFunctionController : MonoBehaviour
    {
        public Entity entity;
        // Start is called before the first frame update
        void Start()
        {
            entity = GetComponent<Entity>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FootR()
        {

        }
        public void FootL()
        {

        }

        public void Hit()
        {

        }

        private void OnAnimatorMove()
        {
            if (entity.stateMachine.currentState.GetIsRootMotion()) 
                entity.animator.ApplyBuiltinRootMotion();
        }
    }
}
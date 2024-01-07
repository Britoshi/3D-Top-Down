using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StateMachine.Player
{

    public class PStateMachine : StateMachine
    {
        public override void AssignFactory()
        {
            Factory = new PStateFactory(this);
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {

        }
    }
}
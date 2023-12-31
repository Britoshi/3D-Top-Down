using System;
using UnityEngine;

namespace Game.Abilities
{
    public class ChainNode : BaseAbility
    {
        private float time, gracePeriod;
        public void ApplyGracePeriod() => time = gracePeriod; 
        public void ResetTime() => time = 0;

        internal ChainNode next;

        public override bool CanCast() => time > 0;
        public override void GlobalUpdate()
        {
            if(CanCast())
                time -= Time.deltaTime;
        }

        public override float GetProgress() => 1 - (time / gracePeriod);

        public ChainNode(Entity owner, ChainedAbility master, float castGrace, AbilityCost cost = null) : 
            base(owner, master.NodeName, cost: cost, cooldown: null)
        {
            gracePeriod = castGrace;

            next = null;

            if (master.head == null)
            {
                master.head = this;
                master.curr = this;
                master.tail = this;
                next = this;
            }
            else if(master.Count == 1)
            {
                master.head.next = this;
                master.tail = this;
                next = master.head;
            }
            else
            {
                master.tail.next = this;
                master.tail = this; 
                next = master.head;
            }  
            master.Count++;
        }


    }

    public class ChainedAbility : BaseAbility
    {
        public int Count;
        internal ChainNode head, tail, curr;
         

        public ChainedAbility(Entity owner, string name, Cooldown cooldown = null) : 
            base(owner, name, cdOn: CooldownOn.CUSTOM, cooldown: cooldown, cost: null)
        {
            Count = 0;
            head = null;
            tail = null;
            curr = null;
            busy = false;
        }

        internal string NodeName { get => Name + " " + (Count + 1); }


        public override BaseAbility GetAbility()
        {
            return curr;
        }
        public override float GetProgress()
        {
            if (curr == head)
            {
                if (!curr.CanCast())
                    return cooldown.GetProgress();
            }
            if (busy)
            {
                return animationProgress;
            }
            return curr.GetProgress(); 
        }

        public override float GetProgress2()
        {
            if (!CanCast())
            {
                return 0f;
            }
            return 1 - curr.GetProgress();
        }

        public override string GetAnimation() => curr.GetAnimation();
        public override string GetIcon() => curr.Name; 

        public override void OnCast()
        {
            if (curr == head)
                cooldown.ApplyCooldown();

            curr.Cost.Deduct();

            if (curr == tail)
                curr.next.ResetTime();
            else
                curr.next.ApplyGracePeriod();

            curr = curr.next;
            busy = true;
        }

        public override void GlobalUpdate()
        {
            base.GlobalUpdate();
            curr.GlobalUpdate();

            //Reset
            if(!curr.CanCast())
            {
                head.ResetTime();
                curr = head;
            }
        }

        public override bool CanCast()
        {
            if (busy) return false;

            if(curr == head)
            {
                if (!curr.Cost.CanCast()) return false;
                if (cooldown.CanCast()) return true;
            }

            if(curr.CanCast()) return true; 

            if (!curr.Cost.CanCast()) return false;
            if (!cooldown.CanCast()) return false;

            return true;
        }

        public override void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationStart(animator, stateInfo, layerIndex);

        }

        public override void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationUpdate(animator, stateInfo, layerIndex);

        }

        public override void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationEnd(animator, stateInfo, layerIndex);
        }
    }
}

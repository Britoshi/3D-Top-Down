namespace Game
{
    internal interface IAdvancedTriggers
    {
        public delegate void OnHitFunction(Entity object1, Entity object2, float value);
        public delegate void OnKillFunction(Entity object1, Entity object2);

        public OnHitAffect[] OnHitAffects { set; get; }
        public OnHitAffect[] OnGetHitAffects { set; get; }
        public OnHitAffect[] OnKillAffects { set; get; }


        public static void OnHit(IAdvancedTriggers triggers, Entity owner, Entity target, float amount, OnHitFunction func)
        {
            if (triggers.OnHitAffects == null) return; 
            for (int i = 0; i < triggers.OnHitAffects.Length; i++)
                triggers.OnHitAffects[i]?.ApplyOnHit(owner, target);
            func(owner, target, amount);
        }
        public static void OnGetHit(IAdvancedTriggers triggers, Entity owner, Entity source, float amount, OnHitFunction func)
        {
            if (triggers.OnGetHitAffects == null) return;
            for (int i = 0; i < triggers.OnGetHitAffects.Length; i++)
                triggers.OnGetHitAffects[i]?.ApplyOnGetHit(owner, source);
            func(owner, source, amount); 
        }

        public static void OnKill(IAdvancedTriggers triggers, Entity killer, Entity victim, OnKillFunction func)
        {
            if (triggers.OnKillAffects == null) return;
            for (int i = 0; i < triggers.OnKillAffects.Length; i++)
                triggers.OnKillAffects[i]?.ApplyOnKill(killer, victim);

            func(killer, victim);
        }
    }
}
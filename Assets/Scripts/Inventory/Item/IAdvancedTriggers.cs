namespace Game
{
    internal interface IAdvancedTriggers
    {
        public delegate void OnHitFunction(Status object1, Status object2, float value);
        public delegate void OnKillFunction(Status object1, Status object2);

        public OnHitAffect[] OnHitAffects { set; get; }
        public OnHitAffect[] OnGetHitAffects { set; get; }
        public OnHitAffect[] OnKillAffects { set; get; }


        public static void OnHit(IAdvancedTriggers triggers, Status owner, Status target, float amount, OnHitFunction func)
        {
            if (triggers.OnHitAffects == null) return; 
            for (int i = 0; i < triggers.OnHitAffects.Length; i++)
                triggers.OnHitAffects[i]?.ApplyOnHit(owner, target);
            func(owner, target, amount);
        }
        public static void OnGetHit(IAdvancedTriggers triggers, Status owner, Status source, float amount, OnHitFunction func)
        {
            if (triggers.OnGetHitAffects == null) return;
            for (int i = 0; i < triggers.OnGetHitAffects.Length; i++)
                triggers.OnGetHitAffects[i]?.ApplyOnGetHit(owner, source);
            func(owner, source, amount); 
        }

        public static void OnKill(IAdvancedTriggers triggers, Status killer, Status victim, OnKillFunction func)
        {
            if (triggers.OnKillAffects == null) return;
            for (int i = 0; i < triggers.OnKillAffects.Length; i++)
                triggers.OnKillAffects[i]?.ApplyOnKill(killer, victim);

            func(killer, victim);
        }
    }
}
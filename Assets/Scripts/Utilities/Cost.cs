/*
namespace Game.Utility
{ 
    public class Cost
    {
        protected Entity owner;
        protected EntityResourceStat resource;
        protected float cost;
        protected ResourceStat statCost;
        public Cost(ResourceStat stat, float cost)
        {
            resource = null;
            this.cost = cost;
            statCost = stat;
        }

        public void Init(EntityBase owner)
        {
            this.owner = owner;
            resource = owner.Stats[statCost];
        }

        public virtual float GetCost() => cost;
        public bool CanDeduct() => resource.GetValue() - GetCost() >= 0;
        public virtual void Deduct() => resource.AddValue(-GetCost());

        public static Cost[] Create(EntityBase owner, ResourceStat statCost, float cost)
        {
            var costElement = new Cost(statCost, cost);
            costElement.Init(owner);
            return new Cost[] { costElement };
        }
    }

    public class CostPercent : Cost
    {
        private bool maximumBase;

        public CostPercent(ResourceStat stat, float cost, bool maximumBase = true) : base(stat, cost)
        {
            this.maximumBase = maximumBase;
        }
        public override float GetCost()
        {
            if (maximumBase) return resource.FullValue * cost;
            else return resource.GetValue() * cost;
        }
    }
}

*/
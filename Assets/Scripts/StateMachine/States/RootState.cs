
namespace Game
{
    public abstract class RootState : BaseState
    {
        protected RootState(StateMachine currentContext, StateFactory entityStateFactory) : base(currentContext, entityStateFactory)
        {
            IsRootState = true;
        }

        public override bool UpdateState()
        {
            HandlePrevApplyVariable();
            ApplyVelocity();
            return true;
        }

        protected void HandlePrevApplyVariable()
        {

        }

        protected void ApplyVelocity()
        {
            //ctx.SetVelocityX(ctx.HorizontalVelocity);
        }

    }
}

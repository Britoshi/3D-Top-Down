namespace Game.StateMachine
{
    public interface IHasAnimation
    {
        public string GetAnimationName(); 
    }
    public interface IAnimationLayerOverride
    {
        public int GetLayerIndex();
    }
}
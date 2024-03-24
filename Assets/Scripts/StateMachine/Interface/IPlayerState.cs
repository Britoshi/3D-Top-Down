namespace Game.StateMachine.Player
{
    public interface IPlayerState
    { 
        public PStateMachine P_CTX { set; get; }
        public PStateFactory P_Factory { set; get; }
    }
    public interface IHumanoidState
    {
        public HumanoidStateMachine H_CTX { set; get; }
        public HumanoidStateFactory H_Factory { set; get; }
    }
}
namespace Game.StateMachine.Player
{
    public interface IPlayerState
    { 
        public PStateMachine P_CTX { set; get; }
        public PStateFactory P_Factory { set; get; }
    }
}
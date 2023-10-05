using Player.Factory;

namespace Player.States
{
    public sealed class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(PlayerContext context, PlayerStateFactory stateFactory) : base(context, stateFactory)
        {
            IsRootState = true;
        }
        
        public override void EnterState()
        { }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        { }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerApplicable.GroundedState)
            {
                SwitchState(StateFactory.Grounded());
            }
            
        }

        public override void InitializeSubState()
        { }
    }
}
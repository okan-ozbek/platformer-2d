using Player.Factory;

namespace Player.States
{
    public abstract class PlayerBaseState
    {
        protected bool IsRootState;
        protected readonly PlayerContext Context;
        protected readonly PlayerStateFactory StateFactory;

        private PlayerBaseState _currentSubState;
        private PlayerBaseState _currentSuperState;
        
        public PlayerBaseState(PlayerContext context, PlayerStateFactory stateFactory)
        {
            Context = context;
            StateFactory = stateFactory;
        }
        
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        public void UpdateStates()
        {
            CheckSwitchStates();
            _currentSubState?.UpdateStates();
        }

        protected void SwitchState(PlayerBaseState newState)
        {
            ExitState();
            
            newState.EnterState();

            if (IsRootState)
            {
                Context.PlayerState = newState;    
            } 
            else
            {
                _currentSuperState?.SetSubState(newState);
            }
        }

        private void SetSuperState(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(PlayerBaseState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }
    }
}
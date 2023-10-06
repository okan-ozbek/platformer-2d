using Player.Factories;

namespace Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine Context;
        protected PlayerStateFactory Factory;

        protected PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
        {
            Context = context;
            Factory = factory;
        }
        
        protected abstract void OnEnter();
        protected abstract void OnLeave();
        protected abstract void OnUpdate();

        protected abstract void CanUpdateState();
        
        protected void SwitchState(PlayerBaseState state)
        {
            OnLeave();

            Context.State = state;
            
            state.OnEnter();
        }

        public void Initialize()
        {
            OnEnter();
        }

        public void Update()
        {
            CanUpdateState();
            OnUpdate();
        }
    }
}

using System.Collections.Generic;
using Player.Enums;
using Player.States;

namespace Player.Factories
{
    public sealed class PlayerStateFactory
    {
        private readonly Dictionary<PlayerState, PlayerBaseState> _states = new();

        public PlayerStateFactory(PlayerStateMachine context)
        {
            _states.Add(PlayerState.Dash, new PlayerDashState(context, this));
            _states.Add(PlayerState.Fall, new PlayerFallState(context, this));
            _states.Add(PlayerState.Idle, new PlayerIdleState(context, this));
            _states.Add(PlayerState.Jump, new PlayerJumpState(context, this));
            _states.Add(PlayerState.Move, new PlayerMoveState(context, this));
            _states.Add(PlayerState.WallSlide, new PlayerWallSlideState(context, this));
            _states.Add(PlayerState.WallJump, new PlayerWallJumpState(context, this));
        }

        public PlayerBaseState Dash()
        {
            return _states[PlayerState.Dash];
        }
        
        public PlayerBaseState Fall()
        {
            return _states[PlayerState.Fall];
        }
        
        public PlayerBaseState Idle()
        {
            return _states[PlayerState.Idle];
        }
        
        public PlayerBaseState Jump()
        {
            return _states[PlayerState.Jump];
        }
        
        public PlayerBaseState Move()
        {
            return _states[PlayerState.Move];
        }
        
        public PlayerBaseState WallSlide()
        {
            return _states[PlayerState.WallSlide];
        }
        
        public PlayerBaseState WallJump()
        {
            return _states[PlayerState.WallJump];
        }
    }
}

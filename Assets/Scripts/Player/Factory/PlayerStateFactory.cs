using Player.States;

namespace Player.Factory
{
    public class PlayerStateFactory
    {
        private PlayerContext _context;

        public PlayerStateFactory(PlayerContext context)
        {
            _context = context;
        }

        public PlayerBaseState Grounded()
        {
            return new PlayerGroundedState(_context, this);
        }

        public PlayerBaseState Jump()
        {
            return new PlayerJumpState(_context, this);
        }
        
        public PlayerBaseState Fall()
        {
            return new PlayerFallState(_context, this);
        }
        
        public PlayerBaseState Dash()
        {
            return new PlayerDashState(_context, this);
        }
        
        public PlayerBaseState Move()
        {
            return new PlayerMoveState(_context, this);
        }
        
        public PlayerBaseState Idle()
        {
            return new PlayerIdleState(_context, this);
        }
        
        
    }
}
using UnityEngine;

namespace Player
{
    public class PlayerApplicable
    {
        private PlayerContext _context;

        public bool GroundedState => (_context.Grounded());
        public bool FallState => (!_context.Grounded() && _context.rigidbody2D.velocity.y < 0.0f); 
        public bool JumpState => (_context.PlayerInput.JumpBufferTimer > 0.0f && _context.coyoteTimer > 0.0f); // Can only happen in the grounded state
        public bool DashState => (_context.PlayerInput.PressedSpace && Mathf.Abs(_context.rigidbody2D.velocity.x) > 0.0f && _context.coyoteTimer <= 0.0f); // Can only happen in the fall and jump state
        public bool MoveState => (_context.PlayerInput.InputDirection.x != 0.0f);
        public bool IdleState => (_context.PlayerInput.InputDirection.x == 0.0f);
    }
}
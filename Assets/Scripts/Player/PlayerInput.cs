using UnityEngine;

namespace Player
{
    public sealed class PlayerInput
    {
        public Vector2 Direction { get; private set; }
        public Vector2 DirectionNormalized { get; private set; }
        public Vector2 LastDirection { get; private set; }
        
        public bool PressedSpace { get; private set; }
        public bool ReleasedSpace { get; private set; }
        
        private const float DefaultJumpBufferTime = 0.1f;
        public bool JumpBufferAvailable { get; private set; }
        private float _jumpBufferTimer;

        public void Update()
        {
            Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            DirectionNormalized = Direction.normalized;

            PressedSpace = Input.GetKeyDown(KeyCode.Space);
            ReleasedSpace = Input.GetKeyUp(KeyCode.Space);

            SetLastDirection();
            SetJumpBufferTime();
        }
        
        public void DisableJumpBuffer()
        {
            _jumpBufferTimer = 0.0f;
            JumpBufferAvailable = false;
        }

        private void SetLastDirection()
        {
            Vector2 lastDirection = LastDirection;
            
            lastDirection.x = (Direction.x != 0.0f) ? Direction.x : lastDirection.x;
            lastDirection.y = (Direction.y != 0.0f) ? Direction.y : lastDirection.y;
            
            LastDirection = lastDirection;
        }

        private void SetJumpBufferTime()
        {
            _jumpBufferTimer = (PressedSpace) 
                ? DefaultJumpBufferTime 
                : Mathf.Max(0, _jumpBufferTimer - Time.deltaTime);

            JumpBufferAvailable = (_jumpBufferTimer > 0.0f);
        }

        
    }
}

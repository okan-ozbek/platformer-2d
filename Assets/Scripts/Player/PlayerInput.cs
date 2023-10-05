using UnityEngine;

namespace Player
{
    public class PlayerInput
    {
        public Vector2 InputDirection { get; private set; }
        
        public bool PressedSpace { get; private set; }
        public bool ReleasedSpace { get; private set; }
        
        public float JumpBufferTimer;

        public void UpdateInputs()
        {
            InputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            PressedSpace = Input.GetKeyDown(KeyCode.Space);
            ReleasedSpace = Input.GetKeyDown(KeyCode.Space);
            
            CalculateJumpBufferTime();
        }

        private void CalculateJumpBufferTime()
        {
            if (PressedSpace)
            {
                JumpBufferTimer = PlayerContext.JumpBufferTime;
            }

            JumpBufferTimer -= Mathf.Max(Time.deltaTime, 0.0f);
        }
    }
}
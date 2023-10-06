using UnityEngine;

namespace Player
{
    public sealed class PlayerMovement
    {
        private Vector2 _lastDirection;

        public void Update(PlayerStateMachine context)
        {
            if (context.Input.Direction.x != 0.0f)
                Move(context);

            if (context.velocity.x > 0.0f && context.Input.Direction.x == 0.0f)
                Brake(context);
        }
        
        private void Move(PlayerStateMachine context)
        {
            context.velocity.x += context.horizontalAcceleration * Time.deltaTime;
            context.velocity.x = Mathf.Min(context.velocity.x, context.maxHorizontalVelocity);
            
            if (Mathf.RoundToInt(_lastDirection.x) != Mathf.RoundToInt(context.Input.Direction.x))
            {
                _lastDirection.x = context.Input.Direction.x;

                context.velocity.x *= 0.3f;
            }

            context.SetVelocity(context.velocity.x * _lastDirection.x, context.rigid.velocity.y);
        }

        private void Brake(PlayerStateMachine context)
        {
            context.velocity.x -= context.horizontalDeceleration * Time.deltaTime;
            context.velocity.x = Mathf.Max(context.velocity.x, 0.0f);
        
            context.SetVelocity(context.velocity.x * _lastDirection.x, context.rigid.velocity.y);
        }
    }
}

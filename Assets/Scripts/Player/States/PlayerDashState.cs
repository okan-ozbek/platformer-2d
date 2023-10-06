using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

        private const float NoGravity = 0.0f;
        private const float MaxDashTime = 0.15f;
        private const float DashPower = 8.0f;

        private Vector2 _direction;
        private float _dashTimer;

        protected override void OnEnter()
        {
            Context.canDash = false;
            
            _direction = Context.Input.Direction.normalized;
            _dashTimer = 0.0f;
            
            Context.SetGravityScale(NoGravity);
        }

        protected override void OnLeave()
        {
            
        }

        protected override void OnUpdate()
        {
            if (!ExceededMaxDashTime())
            {
                Context.SetVelocity(_direction * DashPower);
                _dashTimer += Time.deltaTime;
            }
            else
            {
                Context.SetGravityScale();
            }
        }

        protected override void CanUpdateState()
        {
            if (ExceededMaxDashTime() && Context.grounded)
            {
                SwitchState(
                    (Context.rigid.velocity.x != 0.0f) 
                        ? Factory.Move()
                        : Factory.Idle()
                );
            }

            if (ExceededMaxDashTime() && Context.rigid.velocity.y < 0.0f)
            {
                SwitchState(Factory.Fall());
            }
        }

        private bool ExceededMaxDashTime()
        {
            return (_dashTimer > MaxDashTime);
        }
    }
}

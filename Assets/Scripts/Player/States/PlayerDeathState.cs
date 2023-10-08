using Player.Enums;
using Player.Factories;
using UnityEngine;
using System.Collections;

namespace Player.States
{
    public sealed class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        private const float DeathAnimationTime = 1.5f;
        private const float DeathFadeTime = 1.0f;
        private const float TransitionTime = 0.5f;
        private const float RespawnTime = 1.0f;
        
        private float _deathAnimationTimer;
        private float _deathFadeTimer;
        private float _transitionTimer;
        private float _respawnTimer;
        private bool _canTransition;

        protected override void OnEnter()
        {
            Context.Animation.ChangeAnimation(Context, PlayerAnimationState.Fall);
            
            Context.SetVelocity(Vector2.zero);
            Context.SetGravityScale(-0.5f);

            _deathAnimationTimer = DeathAnimationTime;
            _deathFadeTimer = DeathFadeTime;
            _transitionTimer = TransitionTime;
            _respawnTimer = RespawnTime;
            _canTransition = false;
        }

        protected override void OnLeave()
        {
            Context.SetGravityScale();
            
            Context.deathManager.FadeOut();
        }

        protected override void OnUpdate()
        {
            _deathAnimationTimer = Mathf.Max(0.0f, _deathAnimationTimer - Time.deltaTime);
            _deathFadeTimer = Mathf.Max(0.0f, _deathFadeTimer - Time.deltaTime);
            
            
            Context.sprite.color = new Color(
                Context.sprite.color.r,
                Context.sprite.color.g,
                Context.sprite.color.b,
                _deathFadeTimer
            );
            
            if (_deathFadeTimer <= 0.0f)
            {
                Context.deathManager.FadeIn();
                _transitionTimer = Mathf.Max(0.0f, _transitionTimer - Time.deltaTime);
                
                if (_transitionTimer <= 0.0f)
                {
                    Context.transform.position = Context.spawnPosition;
                    Context.sprite.color = new Color(
                        Context.sprite.color.r,
                        Context.sprite.color.g,
                        Context.sprite.color.b,
                        1
                    );
                    Context.SetGravityScale();

                    _respawnTimer = Mathf.Max(0.0f, _respawnTimer - Time.deltaTime);
                    if (_respawnTimer <= 0.0f)
                    {
                        Context.deathManager.FadeOut();
                        _canTransition = true;
                    }
                }
            }
        }

        protected override void CanUpdateState()
        {
            if (_canTransition)
            {
                SwitchState(
                    (Context.rigid.velocity.y < 0.0f) 
                        ? Factory.Fall()
                        : Factory.Idle()
                );
            }
        }
    }
}
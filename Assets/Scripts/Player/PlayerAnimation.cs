using Player.Enums;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation
    {
        public void ChangeAnimation(PlayerStateMachine context, PlayerAnimationState animationState)
        {
            foreach (AnimatorControllerParameter parameter in context.anim.parameters)
            {
                context.anim.SetBool(parameter.name, false);
            }
            
            context.anim.SetBool(animationState.ToString(), true);
        }
    }
}
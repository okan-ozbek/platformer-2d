using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 0.5f;
    
    public void FadeIn()
    {
        transition.SetBool("Crossfade", true);
    }

    public void FadeOut()
    {
        transition.SetBool("Crossfade", false);
    }
    
    
}

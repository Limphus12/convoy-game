using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.utilities
{
    public class AnimationHandler : MonoBehaviour
    {
        [Header("Attributes - Animation")]
        [SerializeField] protected Animator animator;

        [SerializeField] protected bool canAnimate = true;

        protected string currentState;

        protected void PlayAnimation(string newState)
        {
            if (!canAnimate) return;

            //stops the same animation from interrupting itself.
            if (currentState == newState) return;

            //play the animation
            animator.Play(newState);

            //reassign the current state
            currentState = newState;
        }

        protected void SetParamater(string paramater, int value)
        {
            if (canAnimate) animator.SetInteger(paramater, value);
        }

        protected void SetParamater(string paramater, float value)
        {
            if (canAnimate) animator.SetFloat(paramater, value);
        }

        protected void SetParamater(string paramater, bool value)
        {
            if (canAnimate) animator.SetBool(paramater, value);
        }

        protected void SetTrigger(string paramater, bool value)
        {
            if (!canAnimate) return;

            if (value) animator.SetTrigger(paramater);
            else animator.ResetTrigger(paramater);
        }
    }
}
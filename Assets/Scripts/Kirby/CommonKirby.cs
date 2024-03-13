using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommonKirby : Kirby
{
    // ÈíÀÔ

    private void OnInhale(InputValue value)
    {
        if (!IsGround)
        {
            return;
        }

        if (value.isPressed)
        {
            Animator.SetBool("Inhaling", true);
            Animator.Play("Inhaling");

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Inhaling")
                && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Animator.Play("Inhale");
            }
        }
        else if (!value.isPressed)
        {
            Animator.SetBool("Inhaling", false);
        }
    }
}

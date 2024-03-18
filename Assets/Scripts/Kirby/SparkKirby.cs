using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SparkKirby : Kirby
{
    // АјАн

    private void OnSparkAttack(InputValue value)
    {
        if (value.isPressed && IsGround)
        {
            Animator.Play("Attack");
            Animator.SetBool("Attack", true);

            transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        }

        if (!value.isPressed) 
        {
            Animator.SetBool("Attack", false);

            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        }
    }
}

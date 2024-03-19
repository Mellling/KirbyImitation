using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SparkKirby : Kirby
{
    private bool attack;

    Coroutine attackOn;

    IEnumerator AttackOn()
    {
        yield return new WaitForSeconds(0.15f);

        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;

        StopCoroutine(attackOn);

        if (!attack)
        {
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        }
    }

    // АјАн

    private void OnSparkAttack(InputValue value)
    {
        if (value.isPressed && IsGround)
        {
            Animator.Play("Attack");
            attack = true;
            Animator.SetBool("Attack", attack);

            attackOn = StartCoroutine(AttackOn());
        }

        if (!value.isPressed) 
        {
            attack = false;
            Animator.SetBool("Attack", attack);
            Debug.Log("check");
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        }
    }
}

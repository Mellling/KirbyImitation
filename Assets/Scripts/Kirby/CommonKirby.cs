using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommonKirby : Kirby
{
    [SerializeField] Collider2D targetRange;
    [SerializeField] float range;

    private bool keeping;

    // 흡입

    private void OnInhale(InputValue value)
    {
        if (!IsGround ||
            (Animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch")
            && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            || IsSliding)
        {
            return;
        }

        if (value.isPressed)
        {
            Animator.SetBool("Inhaling", true);
            Inhalable();
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
            DisInhalable();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (GetMonsterCheak.Contain(collision.gameObject.layer))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            MonsterIn(monster);

            //
        }
    }

    private void MonsterIn(Monster monster)
    {
        Manager.GetInstanse().GetMonsterData(monster.gameObject);
        Destroy(monster.gameObject);

        Animator.Play("Keep");
        keeping = true;
        Animator.SetBool("Keeping", keeping);

        Manager.GetInstanse().ChangeKirbyAblility();
    }

    private void OnChange(InputValue value)
    {
        if (value.isPressed && keeping) 
        {
            keeping = false;
            Animator.SetBool("Keeping", keeping);

            // Manager.GetInstanse().ChangeKirbyAblility();
        }
    }

    private void Inhalable()
    {
        targetRange.enabled = true;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        //transform.GetChild(0).GetComponent<Collider2D>()
    }

    private void DisInhalable()
    {
        targetRange.enabled = false;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GetMonsterCheak.Contain(collision.gameObject.layer))
        {
            Debug.Log("집에가고싶다");
        }
    }

    private void TargetIn()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, range);
    }
}

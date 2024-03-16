using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CommonKirby : Kirby
{
    [SerializeField] Collider2D targetRange;

    private bool keeping;

    // »Ì¿‘

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
        if (GetMonsterCheak.Contain(collision.gameObject.layer))
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Inhale"))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                MonsterIn(monster.gameObject);
            }
            else
            {
                Manager.GetInstanse().GetDamage(20);

                Animator.Play("GetDamage");

                Vector2 velocity = Rigid.velocity;

                if (transform.position.x < collision.transform.position.x)
                {
                    velocity.x = -15;
                }
                else if (transform.position.x > collision.transform.position.x)
                {
                    velocity.x = 15;
                }
                Rigid.velocity = velocity;
            }
        }

        base.OnCollisionEnter2D(collision);
    }

    private void MonsterIn(GameObject monster)
    {
        Manager.GetInstanse().SetMonsterData(monster);
        monster.SetActive(false);

        Animator.Play("Keep");
        keeping = true;
        Animator.SetBool("Keeping", keeping);
    }

    Coroutine change;

    IEnumerator Chang()
    {
        keeping = false;
        Animator.SetBool("Keeping", keeping);

        yield return new WaitForSeconds(0.27f);

        Manager.GetInstanse().ChangeKirbyAblility();
        StopCoroutine(change);
    }

    private void OnChange(InputValue value)
    {
        if (value.isPressed && keeping) 
        {
            change = StartCoroutine(Chang());
        }
    }

    private void Inhalable()
    {
        targetRange.enabled = true;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
    }

    private void DisInhalable()
    {
        targetRange.enabled = false;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
    }

    private void OnSpitOut(InputValue value)
    {
        if (value.isPressed && keeping)
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Keep"))
            {
                Animator.Play("SpritOut");
                keeping = false;
                Animator.SetBool("Keeping", keeping);
                Manager.GetInstanse().DestroyMonster();
            }
        }
    }
}

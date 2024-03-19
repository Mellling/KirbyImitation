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
    private bool inhale;

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
            inhale = true;
            Animator.SetBool("Inhaling", inhale);
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
            inhale = false;
            Animator.SetBool("Inhaling", inhale);
            DisInhalable();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetMonsterCheak.Contain(collision.gameObject.layer))
        {
            if (inhale)
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();

                if (monster == null) // 몬스터가 아닌 박스가 들어온 경우
                {
                    BlockIn(collision.gameObject);
                    return;
                }
                MonsterIn(monster.gameObject);
            }
            else
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();

                if (monster == null || monster.Die)
                {
                    return;
                }

                Manager.GetInstanse().GetDamage(monster.Damage);

                Animator.Play("GetDamage");

                Vector2 velocity = Rigid.velocity;

                if (transform.position.x < collision.transform.position.x)
                {
                    velocity.x = -8;
                }
                else if (transform.position.x > collision.transform.position.x)
                {
                    velocity.x = 8;
                }
                Rigid.velocity = velocity;
            }
        }

        base.OnCollisionEnter2D(collision);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetMonsterCheak.Contain(collision.gameObject.layer) && !inhale)
        {
            Monster monster = collision.gameObject.transform.parent.gameObject.GetComponent<Monster>();

            Manager.GetInstanse().GetDamage(monster.Damage);

            Animator.Play("GetDamage");

            Vector2 velocity = Rigid.velocity;

            if (transform.position.x < collision.transform.position.x)
            {
                velocity.x = -8;
            }
            else if (transform.position.x > collision.transform.position.x)
            {
                velocity.x = 8;
            }
            Rigid.velocity = velocity;
        }
    }

    private void MonsterIn(GameObject monster)
    {
        Manager.GetInstanse().SetMonsterData(monster);
        monster.SetActive(false);

        Animator.Play("Keep");
        keeping = true;
        Animator.SetBool("Keeping", keeping);
    }

    private void BlockIn(GameObject block)
    {
        Destroy(block);

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
        Manager.GetInstanse().CanInhaledSet(true);
        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
    }

    private void DisInhalable()
    {
        targetRange.enabled = false;
        Manager.GetInstanse().CanInhaledSet(false);
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

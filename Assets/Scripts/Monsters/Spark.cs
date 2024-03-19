using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : Monster
{
    [SerializeField] LayerMask groundCheakLayer;
    [SerializeField] float jumpPower;

    private bool isGround;
    private bool jumpHightest;

    Coroutine attack;

    IEnumerator Attack()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.48f);

        transform.GetChild(0).gameObject.SetActive(false);
        StopCoroutine(attack);
    }

    public override void Move()
    {
        int random = (int)Random.Range(0f, 2f);

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (random == 0)
            {
                Moving();
                Animator.Play("Jump", 0);
            }
            else
            {
                Animator.Play("Attack");
                attack = StartCoroutine(Attack());
            }

        }

        if (!isGround)
        {
            CheakJumpSituation();
        }
    }

    private void Moving()
    {
        Vector2 velocity = Rigid.velocity;

        if (Render.flipX)
        {
            velocity.x = -1;
            if (isGround)
            {
                velocity.y = jumpPower;
            }
        }
        else if (!Render.flipX)
        {
            velocity.x = 1;
            if (isGround)
            {
                velocity.y = jumpPower;
            }
        }
        Rigid.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (groundCheakLayer.Contain(collision.gameObject.layer))
        {
            isGround = true;
            Animator.Play("Idle", 0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (groundCheakLayer.Contain(collision.gameObject.layer))
        {
            isGround = false;
        }
    }

    private void CheakJumpSituation()
    {
        if (Rigid == null)
        {
            return;
        }

        if (!(Rigid.velocity.y < 0 && Rigid.velocity.y > 0)) // ¼öÁ¤
        {
            jumpHightest = true;
            Animator.SetBool("JumpHightest", jumpHightest);
        }
        else if (Rigid.velocity.y < 0)
        {
            jumpHightest = false;
            Animator.SetBool("JumpHightest", jumpHightest);
        }
    }
}

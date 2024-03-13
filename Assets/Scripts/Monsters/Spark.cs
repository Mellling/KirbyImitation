using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : Monster
{
    [SerializeField] LayerMask groundCheakLayer;
    [SerializeField] float jumpPower;
    [SerializeField] Animator animator;

    private bool isGround;
    private bool jumpHightest;

    public override void Move()
    {
        Debug.Log(Rigid.velocity.y);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Moving();
            animator.Play("Jump", 0);
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
            animator.Play("Idle", 0);
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
        if (!(Rigid.velocity.y < 0 && Rigid.velocity.y > 0)) // ¼öÁ¤
        {
            jumpHightest = true;
            animator.SetBool("JumpHightest", jumpHightest);
        }
        else if (Rigid.velocity.y < 0)
        {
            jumpHightest = false;
            animator.SetBool("JumpHightest", jumpHightest);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Kriby : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Animator animator;

    [Header("Property")]
    [SerializeField] float movePower;
    [SerializeField] float brakePower;
    [SerializeField] float maxXSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float runPower;
    [SerializeField] float runmaxSpeed;

    [SerializeField] LayerMask groundCheakLayer;

    private Vector2 moveDir;
    private bool isGround;
    private bool isJumping;
    private bool isRunning;

    private void FixedUpdate()
    {
        Move();
        if (isJumping)
        {
            CheakJumpSituation();
        }

        animator.SetBool("IsGround", isGround);
        animator.SetBool("Running", isRunning);
    }

    // 기본 움직임
    public void Move()
    {
        if (isRunning)
        {
            Moving(runmaxSpeed, runPower);
        }
        else
        {
            Moving(maxXSpeed, movePower);
        }
    }

    private void Moving(float max, float power)
    {
        if (moveDir.x < 0 && rigid.velocity.x > -max)
        {
            rigid.AddForce(Vector2.right * moveDir * power);
        }
        else if (moveDir.x > 0 && rigid.velocity.x < max)
        {
            rigid.AddForce(Vector2.right * moveDir * power);
        }
        else if (moveDir.x == 0 && rigid.velocity.x < 0)
        {
            if (isRunning)
            {
                isRunning = false;
            }
            rigid.AddForce(Vector2.right * brakePower);
        }
        else if (moveDir.x == 0 && rigid.velocity.x > 0)
        {
            if (isRunning)
            {
                isRunning = false;
            }
            rigid.AddForce(Vector2.left * brakePower);
        }
    }
    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        
        if (moveDir.x < 0)
        {
            render.flipX = true;
            animator.SetBool("Walking", true);
        }
        else if (moveDir.x > 0)
        {
            render.flipX = false;
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    // 점프
    public void Jump()
    {
        Vector2 velocity = rigid.velocity;
        velocity.y = jumpSpeed;
        rigid.velocity = velocity;
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && isGround)
        {
            Jump();
            isJumping = true;
            animator.SetBool("JumpUp", true);
        }
    }

    private void CheakJumpSituation()
    {
        if (rigid.velocity.y > 0)
        {
            animator.SetBool("JumpUp", true);

        }
        else if (rigid.velocity.y <= 0)
        {
            animator.SetBool("JumpUp", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundCheakLayer.Contain(collision.gameObject.layer))
        {
            isGround = true;

            if (isJumping)
            {
                isJumping = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (groundCheakLayer.Contain(collision.gameObject.layer))
        {
            isGround = false;
        }
    }

    // 달리기

    private void OnRun(InputValue value)
    {
        if (value.isPressed && isGround)
        {
            isRunning = true;
        }
    }

    // 풍선
    /*private void OnBalloon(InputValue value)
    {
        if ()
    }*/
}

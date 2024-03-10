using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.XR.Haptics;

public class Kriby : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Animator animator;
    [SerializeField] InputActionAsset inputAction;

    [Header("Property")]
    [SerializeField] float movePower;
    [SerializeField] float brakePower;
    [SerializeField] float maxXSpeed;

    [SerializeField] float jumpSpeed;

    [SerializeField] float runPower;
    [SerializeField] float runmaxSpeed;

    [SerializeField] float flyGravity;
    [SerializeField] float flyYPower;
    [SerializeField] float flyXPower;
    [SerializeField] float flyXMaxSpeed;
    [SerializeField] float flyYMaxSpeed;

    [SerializeField] int Hp;

    [SerializeField] LayerMask groundCheakLayer;

    private Vector2 moveDir;
    private bool isGround;
    private bool isJumping;
    private bool isRunning;
    private bool isFlying;
    private bool isCrouching;
    private bool isSliding;

    /*private static Kriby instanse;

    public static Kriby GetInstanse() { return instanse; }

    private void Awake()
    {
        if (instanse == null)
        {
            instanse = new Kriby();
            Destroy(this);

            return;
        }

        instanse = this;
    }

    private Kriby() { }*/

    private void FixedUpdate()
    {
        Move();

        if (!isFlying)
        {
            CheakJumpSituation();
        }

        animator.SetBool("IsGround", isGround);
        animator.SetBool("Running", isRunning);
    }

    // �⺻ ������
    public void Move()
    {
        if (isRunning)
        {
            Moving(runmaxSpeed, runPower);
        }
        else if (isFlying)
        {
            Moving(flyXMaxSpeed, flyXPower);
        }
        else
        {
            Moving(maxXSpeed, movePower);
        }
    }

    private void Moving(float max, float power)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
        {
            power = 0;
        }

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

        if (isFlying)
        {
            if (moveDir.y > 0 && rigid.velocity.y < flyYMaxSpeed)
            {
                rigid.AddForce(Vector2.up * moveDir * flyYPower, ForceMode2D.Impulse);
            }
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

    // ����
    public void Jump()
    {
        Vector2 velocity = rigid.velocity;
        velocity.y = jumpSpeed;
        rigid.velocity = velocity;
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && isGround && !isFlying && !isCrouching)
        {
            Jump();
            isJumping = true;
            if (animator.GetBool("JumpUp") == false)
            {
                animator.SetBool("JumpUp", true);
            }
        }

    }

    private void CheakJumpSituation()
    {
        if (rigid.velocity.y < 0)
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
                animator.SetBool("JumpUp", false);
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

    // �޸���
    private void OnRun(InputValue value)
    {
        if (value.isPressed && isGround)
        {
            isRunning = true;
        }
        else if (!value.isPressed)
        {
            isRunning = false;
        }
    }

    // ��ũ����

    private void OnCrouch(InputValue value)
    {
        if (isSliding)
        {
            isSliding = false;
            animator.SetBool("isSlide", isSliding);
        }

        if (value.isPressed)
        {
            isCrouching = true;
            animator.SetBool("Crouching", isCrouching);
        }
        else if (!value.isPressed)
        {
            isCrouching = false;
            animator.SetBool("Crouching", isCrouching);
        }
    }

    public void Slide()
    {
        Vector2 velocity = rigid.velocity;

        if (render.flipX)
        {
            velocity.x = -10;
        }
        else if (!render.flipX)
        {
            velocity.x = 10;
        }
        rigid.velocity = velocity;
    }

    private void OnSlide(InputValue value)
    {

        if (isGround && !isFlying && animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
        {
            isCrouching = false;
            animator.SetBool("Crouching", isCrouching);

            isSliding = true;
            animator.SetBool("isSlide", isSliding);

            Slide();

            animator.Play("Sliding");
        }
    }

    // ǳ��

        private void OnBalloon(InputValue value)
    {
        if (!isGround && isJumping)
        {
            isFlying = true;
            rigid.gravityScale = flyGravity;
            animator.SetBool("Fly", isFlying);
        }
    }

    private void OnBalloonClear(InputValue value)
    {
        if (isFlying)
        {
            isFlying = false;
            rigid.gravityScale = 1.0f;
            animator.SetBool("Fly", isFlying);
        }
    }

    // ���� �Ա�
}

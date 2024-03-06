using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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

    [SerializeField] LayerMask groundCheakLayer;

    private Vector2 moveDir;
    private bool isGround;
    private bool isJumping;
    private bool isRunning;
    private bool isFlying;
    // private bool canMove = true;
    private void Awake()
    {
        inputAction.Disable();
        presseCo = null;
        InputActionMap playerMap = inputAction.FindActionMap("Player");
        InputAction playerAction = playerMap.FindAction("Balloon");

        if (playerAction != null)
        {
            playerAction.started += OnEnter;
            playerAction.canceled += OnExit;
        }

        inputAction.Enable();
    }
    private void FixedUpdate()
    {
        Move();

        if (isJumping && !isFlying)
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
            if(animator.GetBool("JumpUp") == false)
                animator.SetBool("JumpUp", true);
        }

    }

    private void CheakJumpSituation()
    {
        if (rigid.velocity.y > 0)
        {
            //animator.SetBool("JumpUp", true);

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

    // 달리기
    private void OnRun(InputValue value)
    {
        if (value.isPressed && isGround)
        {
            isRunning = true;
        }
    }

    // 웅크리기
    private void OnCrouch(InputValue value)
    {

    }

    Coroutine presseCo = null;
    // 풍선
    void OnEnter(InputAction.CallbackContext context)
    {
        if (presseCo != null)
        {
            StopCoroutine(presseCo);
        }
        presseCo = StartCoroutine(OnPressed());

        Debug.Log("진입");
    }
    IEnumerator OnPressed()
    {
        yield return new WaitForSeconds(0.2f);
        while(true)
        {
            if (!isGround)
            {
                Debug.Log("BallonFly 중");
                isFlying = true;
                animator.SetBool("JumpUp", false);
                animator.SetBool("Fly", isFlying);

                rigid.gravityScale = flyGravity;
            }

            Debug.Log("진입 중");
            yield return new WaitForFixedUpdate();
        }
    }
    void OnExit(InputAction.CallbackContext context)
    {
        if (presseCo != null)
        {
            StopCoroutine(presseCo);
        }
        isFlying = false;
        animator.SetBool("Fly", isFlying);
        rigid.gravityScale = 1.0f;
        Debug.Log("탈출");
    }

}

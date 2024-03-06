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

    [SerializeField] LayerMask groundCheakLayer;

    private Vector2 moveDir;
    private bool isGround;
    private bool isJumping;
    private bool isRunning;
    private bool isFlying;
    private bool isCrouching;
    private bool isSliding;

    private void Awake()
    {
        /*inputAction.Disable();
        presseCo = null;
        InputActionMap playerMap = inputAction.FindActionMap("Player");
        InputAction playerAction = playerMap.FindAction("Crouch");

        if (playerAction != null)
        {
            playerAction.started += OnEnter;
            playerAction.canceled += OnExit;
        }

        inputAction.Enable();*/

        /*inputAction.Disable();
        presseCo = null;
        InputActionMap playerMap = inputAction.FindActionMap("Player");
        InputAction playerAction = playerMap.FindAction("Balloon");

        if (playerAction != null)
        {
            playerAction.started += OnEnter;
            playerAction.canceled += OnExit;
        }

        inputAction.Enable();*/
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
        if (isCrouching)
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

    // 점프
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
            Debug.Log("OnJump");
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
        if (value.isPressed)
        {

            if (isSliding)
            {
                Debug.Log("In");
                isSliding = false;
                animator.SetBool("isSlide", false);

                isCrouching = true;
                animator.SetBool("Crouching", isCrouching);
            }
            else
            {
                isCrouching = true;
                animator.SetBool("Crouching", isCrouching);
            }

            
        }
        else if (!value.isPressed)
        {
            isCrouching = false;
            animator.SetBool("Crouching", isCrouching);
        }
    }

    /*Coroutine presseCo = null;

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
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (isGround && !isFlying && !isSliding)
            {
                Debug.Log("OnCrouch 중");
                isCrouching = true;
                animator.SetBool("Crouching", isCrouching);
            }

            if (isSliding)
            {
                // isSliding = false;
                // animator.SetBool("isSlide", isSliding);
                animator.Play("Sliding");
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
        isCrouching = false;
        animator.SetBool("Crouching", isCrouching);
        Debug.Log("탈출");
    }*/

    private void OnSlide(InputValue value)
    {
        if (value.isPressed && isCrouching)
        {
            isCrouching = false;
            animator.SetBool("Crouching", isCrouching);

            isSliding = true;
            animator.SetBool("isSlide", isSliding);
        }
    }

    // 풍선

        private void OnBalloon(InputValue value)
    {
        if (!isGround)
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

    /*Coroutine presseCo = null;

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
    }*/

}

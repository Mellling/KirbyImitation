using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.XR.Haptics;

public class Kirby : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }
    [SerializeField] SpriteRenderer render;
    public SpriteRenderer Render { get { return render; } }
    [SerializeField] Animator animator;
    public Animator Animator {  get { return animator; } }
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
    public LayerMask GetGroundCheak { get { return groundCheakLayer;  } }
    [SerializeField] LayerMask MonsterCheakLayer;
    public LayerMask GetMonsterCheak { get { return MonsterCheakLayer;  } }
    [SerializeField] LayerMask StageDoorCheakLayer;

    private Vector2 moveDir;
    private bool isGround;
    public bool IsGround {  get { return isGround; } }
    private bool isJumping;
    private bool isRunning;
    private bool isFlying;
    private bool isCrouching;
    private bool isSliding;
    public bool IsSliding { get { return isSliding; } }

    private bool getMonster;
    public bool GetMonster { get { return getMonster; } }

    private bool stageChageable;

    private static string kirbyName;

    public static string KirbyName() { return kirbyName; }

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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Keep"))
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Inhale"))
        {
            return;
        }

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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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

        if (GetMonsterCheak.Contain(collision.gameObject.layer) 
            && Manager.GetInstanse().KirbyData.KirbyAbility != "Common")
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();

            if (monster == null || monster.Die)
            {
                return;
            }

            Manager.GetInstanse().GetDamage(monster.Damage);

            animator.Play("GetDamage");

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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (groundCheakLayer.Contain(collision.gameObject.layer))
        {
            isGround = false;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (isJumping)
                {
                    return;
                }
                animator.Play("JumpDown", 0);
            }
        }
    }

    // 달리기
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

    // 웅크리기

    private void OnCrouch(InputValue value)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Keep"))
        {
            return;
        }

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

    // 풍선

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

    // 스테이지 이동

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (StageDoorCheakLayer.Contain(collision.gameObject.layer))
        {
            stageChageable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (StageDoorCheakLayer.Contain(collision.gameObject.layer))
        {
            stageChageable = false;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetMonsterCheak.Contain(collision.gameObject.layer))
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

    private void OnGoNextStage(InputValue value)
    {
        if (value.isPressed && stageChageable)
        {
            Manager.GetInstanse().StageChanging.Change();
            stageChageable = false;
        }
    }
}

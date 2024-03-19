using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Animator animator;
    public Animator Animator { get { return animator; } }

    public Rigidbody2D Rigid { get { return rigid; } }
    public SpriteRenderer Render { get { return render; } }

    public string Name() { return name; }

    [SerializeField] int hp;

    private bool die;
    public bool Die { get {  return die; } }

    private bool isInhaled;

    [SerializeField] float damage;
    public float Damage { get { return damage; } }

    [SerializeField] LayerMask canFowardCheakLayer;
    [SerializeField] LayerMask playerCheakLayer;

    Coroutine monsterDie;

    IEnumerator MonsterDie()
    {
        animator.Play("Die");

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
        StopCoroutine(monsterDie);
    }

    protected virtual void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Move();
        }

        if (isInhaled)
        {
            Inhaled();
        }


        if (hp <= 0)
        {
            monsterDie = StartCoroutine(MonsterDie());
        }
    }

    public virtual void Move()
    {
        Vector2 velocity = rigid.velocity;

        if (render.flipX)
        {
            velocity.x = -1;
        }
        else if (!render.flipX)
        {
            velocity.x = 1;
        }
        rigid.velocity = velocity;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {

        if (canFowardCheakLayer.Contain(collision.gameObject.layer) && !die)
        {
            render.flipX = !render.flipX;
        }

        if (playerCheakLayer.Contain(collision.gameObject.layer)
            && !die)
        {
            GetDamage();

            if (hp != 0)
            {
                animator.Play("Damage");

                Vector2 velocity = Rigid.velocity;

                if (transform.position.x < collision.transform.position.x)
                {
                    velocity.x = -3;
                }
                else if (transform.position.x > collision.transform.position.x)
                {
                    velocity.x = 3;
                }
                Rigid.velocity = velocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerCheakLayer.Contain(collision.gameObject.layer))
        {
            string ability = Manager.GetInstanse().KirbyData.KirbyAbility;

            if (ability == "Common")
            {
                if (!Manager.GetInstanse().CanInhaled)
                {
                    return;
                }

                Manager.GetInstanse().CanInhaledSet(false);

                gameObject.layer = 10;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

                animator.Play("Die");
                die = true;
                animator.SetBool("Die", die);

                isInhaled = true;
            }
            else if (ability == "Spark")
            {
                GetDamage();

                if (hp != 0)
                {
                    animator.Play("Damage");

                    Vector2 velocity = Rigid.velocity;

                    if (transform.position.x < collision.transform.position.x)
                    {
                        velocity.x = -3;
                    }
                    else if (transform.position.x > collision.transform.position.x)
                    {
                        velocity.x = 3;
                    }
                    Rigid.velocity = velocity;
                }
            }
        }
    }

    private void GetDamage()
    {
        hp -= Manager.GetInstanse().KirbyDamage;
    }

    private void Inhaled()
    {
        Vector2 speed = Vector2.zero;

        Transform player = Manager.GetInstanse().transform.GetChild(0).gameObject.transform;

        transform.position = Vector2.SmoothDamp(transform.position, player.position, ref speed, 0.07f);
    }
}
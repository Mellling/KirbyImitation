using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private int hp;

    private bool die;

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

    private void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Move();
        }

        if (hp == 0)
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

        if (canFowardCheakLayer.Contain(collision.gameObject.layer))
        {
            render.flipX = !render.flipX;
        }

        if (playerCheakLayer.Contain(collision.gameObject.layer)
            && !die)
        {
            hp -= Manager.GetInstanse().KirbyDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerCheakLayer.Contain(collision.gameObject.layer))
        {
            animator.Play("Die");
            die = true;
        }
    }
}
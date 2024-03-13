using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;
    //[SerializeField] Animator animator;

    public Rigidbody2D Rigid { get { return rigid; } }
    public SpriteRenderer Render { get { return render; } }

    public string Name() { return name; }

    [SerializeField] LayerMask canFowardCheakLayer;
    [SerializeField] LayerMask PlayerCheakLayer;

    private void Update()
    {
        Move();
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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;

    [Header("Property")]
    /*[SerializeField] int Hp;
    [SerializeField] int Hit;*/

    [SerializeField] LayerMask canFowardCheakLayer;
    [SerializeField] LayerMask PlayerCheakLayer;

    private void Update()
    {
        Move();
    }

    public void Move()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canFowardCheakLayer.Contain(collision.gameObject.layer))
        {
            Debug.Log(" OnCollisionEnter");
            render.flipX = !render.flipX;
        }
        // else if (PlayerCheakLayer.Contain(collision.gameObject.layer)) 
        {

        }
    }
}

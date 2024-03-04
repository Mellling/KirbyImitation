using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Kriby : MonoBehaviour
{
    [Header("Componemt")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;
    [SerializeField] float movePower;
    private Vector2 moveDir;

    private void Update()
    {
        Move();
    }
    public void Move()
    {
        rigid.AddForce(Vector2.right * moveDir * movePower);
    }
    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
}

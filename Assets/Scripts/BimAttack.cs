using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BimAttack : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Animator animator;

    private GameObject bimMonster;

    private void Update()
    {
        FlipX();
    }

    private void FlipX()
    {
        bimMonster = transform.parent.gameObject;

        if (bimMonster.GetComponent<SpriteRenderer>().flipX)
        {
            renderer.flipX = true;
            animator.Play("BimLeft");
        }
        else
        {
            renderer.flipX = false;
            animator.Play("BimRight");
        }
    }
}

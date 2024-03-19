using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaddleDoo : Monster
{
    private int random;
    private bool setRandom;

    protected override void Update()
    {
        base.Update();

        if (!setRandom)
        {
            waitRandom = StartCoroutine(WaitRandom());
        }
    }

    // АјАн

    Coroutine attack;

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.18f);

        transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        transform.GetChild(0).gameObject.SetActive(false);
        StopCoroutine(attack);
    }

    Coroutine waitRandom;

    IEnumerator WaitRandom()
    {
        random = (int)Random.Range(0f, 2f);

        Debug.Log(random);

        setRandom = true;

        yield return new WaitForSeconds(3f);

        setRandom = false;
        StopCoroutine(waitRandom);
    }

    public override void Move()
    {
        if (random == 0)
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }

            base.Move();
        }
        else
        {
            Animator.Play("Attack");
            attack = StartCoroutine(Attack());
            random = 0;
        }
    }
}

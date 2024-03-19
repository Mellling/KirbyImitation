using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : MonoBehaviour
{
    [SerializeField] float recoverHp;
    [SerializeField] LayerMask playerCheakLayer;

    private void Use()
    {
        Manager.GetInstanse().ReCover(recoverHp);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerCheakLayer.Contain(collision.gameObject.layer))
        {
            Use();
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Box: MonoBehaviour
{
    private bool isInhale;

    [SerializeField] LayerMask playerCheakLayer;

    private void Update()
    {
        if (isInhale)
        {
            Inhale();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerCheakLayer.Contain(collision.gameObject.layer))
        {
            if (Manager.GetInstanse().KirbyData.KirbyAbility == "Common")
            {
                if (!Manager.GetInstanse().CanInhaled)
                {
                    return;
                }

                Manager.GetInstanse().CanInhaledSet(false);

                Debug.Log("In");
                isInhale = true;
                gameObject.layer = 10;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Inhale()
    {
        Vector2 speed = Vector2.zero;

        Transform player = Manager.GetInstanse().transform.GetChild(0).gameObject.transform;

        transform.position = Vector2.SmoothDamp(transform.position, player.position, ref speed, 0.1f);
    }
}

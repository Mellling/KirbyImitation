using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCheakCollider : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float range;
    [SerializeField] LayerMask targeMask;
    [SerializeField] LayerMask obstacleMask;

    Collider2D[] colliders = new Collider2D[5];

    private void Update()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, targeMask);
        
        for (int i = 0; i < size; i++)
        {
            Vector2 dirToTarget = (gameObject.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            if (Vector2.Dot(transform.position, dirToTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }

            /*float distToTarget = Vector2.Distance(transform.position, colliders[i].transform.position);
            if (Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
            {
                continue;
            }*/

            // Debug.DrawRay(transform.position, dirToTarget * dirToTarget, Color.red);
        }
    }
}

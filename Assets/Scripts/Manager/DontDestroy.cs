using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instanse;
    private void Awake()
    {
        if (instanse != null)
        {
            Destroy(this);

            return;
        }
        instanse = this;

        DontDestroyOnLoad(instanse);
    }
}

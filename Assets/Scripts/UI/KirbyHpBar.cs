using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyHpBar : HpBar
{
    private void Awake()
    {
        curHp = 100.0f;
        maxHp = 100.0f;
    }

    private void Update()
    {
        curHp = Manager.GetInstanse().KirbyHp;
        CheckHp();
    }
}

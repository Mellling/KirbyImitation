using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    protected float curHp;
    protected float maxHp;

    [SerializeField] Slider HpBarSlider;
    public void CheckHp()
    {
        if (maxHp == 0 || curHp < 0)
        {
            return;
        }

        if (HpBarSlider != null)
        {
            HpBarSlider.value = curHp / maxHp;
        }
    }
}

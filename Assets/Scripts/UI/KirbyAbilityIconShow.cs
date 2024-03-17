using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KirbyAbilityIconShow : MonoBehaviour
{
    private Image curAblityImage;

    private void Start()
    {
        curAblityImage = GetComponent<Image>();
        SetImage();
    }

    public void SetImage()
    {
        curAblityImage.sprite = Manager.GetInstanse().KirbyData.AbilityIconImage;
    }
}

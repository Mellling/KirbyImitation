using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KirbyData", menuName = "Data/Kirby")]
public class KirbyData : ScriptableObject
{
    [SerializeField]
    string kirbyAbility;
    public string KirbyAbility { get { return kirbyAbility; } }

    [SerializeField]
    GameObject kirby;
    public GameObject Kirby { get { return kirby; } }

    [SerializeField] Sprite abilityNameImage;
    public Sprite AbilityNameImage { get { return abilityNameImage; } }

    [SerializeField] Sprite abilityIconImage = null;
    public Sprite AbilityIconImage { get { return abilityIconImage; } }
}

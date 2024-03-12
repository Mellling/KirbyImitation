using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    KirbyData getableAbility = null;
    public KirbyData GetableAbility {  get { return getableAbility; } }

    [SerializeField]
    GameObject monster;
    public GameObject GetMonster { get { return monster; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    KirbyData getableAbility;
    public KirbyData GetableAbility {  get { return getableAbility; } }

    [SerializeField]
    Monster wahtMonster;
    public Monster GetMonster { get { return wahtMonster; } }

    [SerializeField]
    GameObject monster;
    public GameObject Monster {  get { return monster; } }
}

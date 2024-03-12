using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instanse;

    private int hp;

    [Header("Kirby")]
    [SerializeField] KirbyData kirbyData;

    [Header("Monster")]
    [SerializeField] MonsterData[] monsterList;

    private MonsterData monsterData;

    private Monster monster;
    public Monster GetMonsterData(Monster monster) { return this.monster = monster; }
    public static Manager GetInstanse() { return instanse; }

    private void Update()
    {
        
    }

    private void Awake()
    {
        if (instanse != null)
        {
            Destroy(this);

            return;
        }
        Debug.Log(hp);
        instanse = this;
    }

    private Manager()
    {
        hp = 100;
    }

    public void ChangeKirbyAblility()
    {
        if (kirbyData.KirbyAbility != "Common")
        {
            return;
        }

        foreach(MonsterData wantFindMonster in monsterList) 
        {
            if (wantFindMonster.GetMonster == monster)
            {
                monsterData = wantFindMonster;
                break;
            }
        }

        Vector3 position = gameObject.transform.position;
        Destroy(transform.GetChild(0).gameObject);
        GameObject newKirby = Instantiate(monsterData.GetableAbility.Kirby, position, Quaternion.identity);
        newKirby.transform.parent = transform;
    }
}

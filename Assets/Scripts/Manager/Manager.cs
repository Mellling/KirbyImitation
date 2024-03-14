using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Manager : MonoBehaviour
{
    private static Manager instanse;

    private int hp;

    [Header("Kirby")]
    [SerializeField] KirbyData kirbyData;

    [Header("Monster")]
    [SerializeField] MonsterData[] monsterList;

    [SerializeField] CinemachineVirtualCamera camera;

    private MonsterData monsterData;

    private GameObject monster;
    public GameObject GetMonsterData(GameObject monster) { return this.monster = monster; }

    public static Manager GetInstanse() { return instanse; }

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
        if (kirbyData.KirbyAbility != "Common" || monster == null)
        {
            return;
        }

        foreach (MonsterData wantFindMonster in monsterList) 
        {
            if (wantFindMonster.GetMonster.name == monster.name)
            {
                monsterData = wantFindMonster;
                break;
            }
        }

        kirbyData = monsterData.GetableAbility;

        Vector3 position = transform.GetChild(0).gameObject.transform.position;
        Destroy(transform.GetChild(0).gameObject);
        GameObject newKirby = Instantiate(monsterData.GetableAbility.Kirby, position, Quaternion.identity);
        newKirby.transform.parent = transform;

        Destroy(monster);

        // camera.Follow = transform.GetChild(0).gameObject.transform;
    }

    public void DestroyMonster()
    {
        Destroy(monster);
    }
}

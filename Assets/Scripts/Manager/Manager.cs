using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Manager : MonoBehaviour
{
    private static Manager instanse;

    private int kirbyHp;
    public int KirbyHp {  get { return kirbyHp; } }

    [SerializeField]
    private int kirbyDamage;
    public int KirbyDamage { get { return kirbyDamage; } }

    [Header("Kirby")]
    [SerializeField] KirbyData kirbyData;
    public KirbyData KirbyData {  get { return kirbyData; } }

    [Header("Monster")]
    [SerializeField] MonsterData[] monsterList;

    [SerializeField] CinemachineVirtualCamera camera;

    private MonsterData monsterData;

    private GameObject monster;
    public GameObject SetMonsterData(GameObject monster) { return this.monster = monster; }

    public static Manager GetInstanse() { return instanse; }

    private void Awake()
    {
        if (instanse != null)
        {
            Destroy(this);

            return;
        }
        Debug.Log(kirbyHp);
        instanse = this;
    }

    private Manager()
    {
        kirbyHp = 100;
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

        if (monsterData.GetableAbility ==  null) 
        {
            Destroy(monster);
            return;
        }

        kirbyData = monsterData.GetableAbility;

        Vector3 position = transform.GetChild(0).gameObject.transform.position;
        Destroy(transform.GetChild(0).gameObject);
        GameObject newKirby = Instantiate(monsterData.GetableAbility.Kirby, position, Quaternion.identity);
        newKirby.transform.parent = transform;

        Destroy(monster);
    }

    public void DestroyMonster()
    {
        Destroy(monster);
    }

    public void GetDamage(int damage)
    {
        kirbyHp -= damage;
        Debug.Log($"hp: {kirbyHp}");
    }
}

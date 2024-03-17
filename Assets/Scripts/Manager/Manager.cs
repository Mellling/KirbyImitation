using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Manager : MonoBehaviour
{
    private static Manager instanse;

    public static Manager GetInstanse() { return instanse; }

    [Header("Kirby")]
    [SerializeField] KirbyData kirbyData;
    public KirbyData KirbyData {  get { return kirbyData; } }

    private static float KIRBYMAXHP = 100.0f;
    public float GetKirbyMaxHp { get { return KIRBYMAXHP; } }

    private float kirbyHp;
    public float KirbyHp { get { return kirbyHp; } }

    [SerializeField] private int kirbyDamage;
    public int KirbyDamage { get { return kirbyDamage; } }

    [SerializeField] GameObject abilityImage;
    [SerializeField] GameObject abilityIcon;

    ///////////////////////////////////////////////////////////////////////////////////////

    [Header("Monster")]
    [SerializeField] MonsterData[] monsterList;

    private MonsterData monsterData;

    private GameObject monster;
    public GameObject SetMonsterData(GameObject monster) { return this.monster = monster; }

    //////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] GameObject KirbyAbilityUI;

    //////////////////////////////////////////////////////////////////////////////////////

    StageChange stageChange;
    public StageChange StageChanging { get { return stageChange; } }

    //////////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instanse != null)
        {
            Destroy(this);

            return;
        }

        instanse = this;

        gameObject.transform.position = GameObject.Find("StartPoint").transform.position;

        DontDestroyOnLoad(instanse);
    }

    private Manager()
    {
        kirbyHp = KIRBYMAXHP;
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

        abilityImage.GetComponent<KirbyAbilityNameShow>().SetImage();
        abilityIcon.GetComponent<KirbyAbilityIconShow>().SetImage();

        Destroy(monster);
    }

    public void DestroyMonster()
    {
        Destroy(monster);
    }

    public void GetDamage(float damage)
    {
        kirbyHp -= damage;
    }

    public void SetStageChange(StageChange change)
    {
        stageChange = change;
    }
}

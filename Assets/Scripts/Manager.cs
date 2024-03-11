using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instanse;

    private int hp;

    [Header("Kirby")]
    [SerializeField] KirbyData kirbyData;
    [SerializeField] KirbyData kirbyData2;
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

        if (kirbyData != kirbyData2)
        {
            Debug.Log("In");
            Vector3 position = gameObject.transform.position;
            Destroy(transform.GetChild(0).gameObject);
            GameObject newKirby = Instantiate(kirbyData2.Kirby, position, Quaternion.identity);
            newKirby.transform.parent = transform;
        }
    }

    private Manager() 
    {
        hp = 100;
    }
}

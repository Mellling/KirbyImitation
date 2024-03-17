using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        Setplayer();
    }

    private void Setplayer()
    {
        Manager.GetInstanse().gameObject.transform.position = gameObject.transform.position;
        Manager.GetInstanse().transform.GetChild(0).gameObject.transform.position = transform.position;
    }
}

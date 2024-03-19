using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFollow : MonoBehaviour
{
    private void Update()
    {
        if (GetComponent<CinemachineVirtualCamera>().m_Follow == null)
        {
            SetLook();
        }
    }
    private void SetLook()
    {
        if (GameObject.Find("Player").transform.GetChild(0) == null)
        {
            Debug.Log("Can't Find");
            return;
        }
        GetComponent<CinemachineVirtualCamera>().m_Follow = GameObject.Find("Player").transform.GetChild(0);
    }
}

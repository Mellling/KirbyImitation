using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.CullingGroup;

public class StageChange : MonoBehaviour
{
    [SerializeField] string nextStageName;

    private void Start()
    {
        Manager.GetInstanse().SetStageChange(this);
    }

    public void Change()
    {
        SceneManager.LoadScene(nextStageName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugGroupControl : MonoBehaviour
{
    [SerializeField] private GameObject stage1Bugs;
    [SerializeField] private GameObject stage2Bugs;

    public void ActivateStage(int stage)
    {
        if (stage == 1)
        {
            stage1Bugs.SetActive(true);
            stage2Bugs.SetActive(false);
        }
        else if (stage == 2)
        {
            stage1Bugs.SetActive(false); // 可保留 true，如果第一關蟲子還需存在
            stage2Bugs.SetActive(true);
        }
    }

    public void DeactivateAll()
    {
        stage1Bugs.SetActive(false);
        stage2Bugs.SetActive(false);
    }
}

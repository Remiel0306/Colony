using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugGroupControl : MonoBehaviour
{
    [SerializeField] GameObject teachLevel;
    [SerializeField] GameObject Level1Bugs;
    [SerializeField] GameObject level2Bugs;

    private void Start()
    {
        ActivateStage(0);
    }
    public void ActivateStage(int stage)
    {
        if (stage == 0)
        {
            teachLevel.SetActive(true);
            Level1Bugs.SetActive(false);
            level2Bugs.SetActive(false);
        }

        if (stage == 1)
        {
            Level1Bugs.SetActive(true);
            level2Bugs.SetActive(false);
            teachLevel.SetActive(false);
        }
        else if (stage == 2)
        {
            Level1Bugs.SetActive(false); // 可保留 true，如果第一關蟲子還需存在
            level2Bugs.SetActive(true);
            teachLevel.SetActive(false);
        }
    }

    public void DeactivateAll()
    {
        Level1Bugs.SetActive(false);
        level2Bugs.SetActive(false);
        teachLevel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugGroupControl : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject teachLevel;
    [SerializeField] GameObject Level1Bugs;
    [SerializeField] GameObject level2Bugs;

    bool isClose = false;

    private void Start()
    {
        ActivateStage(0);
    }
    public void ActivateStage(int stage)
    {
        if(!playerManager.isPlayerDead)
        {
            if (stage == 0)
            {
                teachLevel.SetActive(true);
                Level1Bugs.SetActive(false);
                level2Bugs.SetActive(false);
                isClose = false;
            }

            if (stage == 1)
            {
                Level1Bugs.SetActive(true);
                level2Bugs.SetActive(false);
                teachLevel.SetActive(false);
                isClose = false;
            }
            else if (stage == 2)
            {
                Level1Bugs.SetActive(false); 
                level2Bugs.SetActive(true);
                teachLevel.SetActive(false);
                isClose = false;
            }
        }
        else if (playerManager.isPlayerDead && !isClose)
        {
            DeactivateAll();
            isClose = true;
        }
    }

    public void DeactivateAll()
    {
        teachLevel.SetActive(false);
        Level1Bugs.SetActive(false);
        level2Bugs.SetActive(false);
    }
}

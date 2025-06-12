using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugGroupControl : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject teachLevel;
    [SerializeField] GameObject Level1Bugs;
    [SerializeField] GameObject level1To2Bugs;

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
                level1To2Bugs.SetActive(false);
                isClose = false;
            }

            else if (stage == 1)
            {
                teachLevel.SetActive(false);
                Level1Bugs.SetActive(true);
                level1To2Bugs.SetActive(false);
                isClose = false;
            }
            else if (stage == 2)
            {
                teachLevel.SetActive(false);
                Level1Bugs.SetActive(false); 
                level1To2Bugs.SetActive(true);
                isClose = false;
            }
            else if(stage == 3)
            {
                teachLevel.SetActive(false);
                Level1Bugs.SetActive(false);
                level1To2Bugs.SetActive(false);
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
        level1To2Bugs.SetActive(false);
    }
}

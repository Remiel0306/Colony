using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugRespawnCtrl : MonoBehaviour
{
    [Header("-----FlyBug-----")]
    [SerializeField] FlyBugManager[] flyBugManager;
    [SerializeField] GameObject[] flyBugs;
    [Header("-----BoomBug-----")]
    [SerializeField] EnemyManager[] enemyManager;
    [SerializeField] GameObject[] boomBugs;

    //Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetAllFlyBug()
    {
        for(int i = 0; i < flyBugs.Length; i++)
        {
            flyBugs[i].SetActive(true);
            flyBugManager[i].FlyBugReset();
        }

        for (int i = 0; i < boomBugs.Length; i++)
        {
            boomBugs[i].SetActive(true);
            enemyManager[i].ResetBugs();
        }
    }
}

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
    [Header("-----Prefab Enemy-----")]
    [SerializeField] FlyEnemyType2 flyBugType2;
    [SerializeField] DeletThatShit deletThatShit;

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
        for (int i = 0; i < flyBugs.Length; i++)
        {
            flyBugs[i].SetActive(true);
            flyBugManager[i].FlyBugReset();
        }

        for (int i = 0; i < boomBugs.Length; i++)
        {
            boomBugs[i].SetActive(true);
            enemyManager[i].ResetBugs();
        }

        //flyBugType2.Destory();
        //deletThatShit.Destory();
    }

    public void ClearAllFlyBugs()
    {
        GameObject[] allBugs = GameObject.FindGameObjectsWithTag("FlyBugClone");
        foreach (GameObject bug in allBugs)
        {
            Destroy(bug);
        }

        GameObject[] allBugs2 = GameObject.FindGameObjectsWithTag("StupidBoomBug2");
        foreach (GameObject bug in allBugs2)
        {
            Destroy(bug);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] UiManager uiManager;
    [SerializeField] Enemy enemyScript;
    [SerializeField] EnemyAttack enemyAttackScript;
    [SerializeField] GameObject boomBug;

    public bool isRespawn = false;
    bool notUrUusiness = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        if(!enemyScript.isDied && isRespawn)
        {
            enemyAttackScript.gameObject.SetActive(true);
            isRespawn = false;
        }
    }
    public void RespawnRest()
    {
        enemyAttackScript.gameObject.SetActive(true);
        enemyAttackScript.transform.position = enemyAttackScript.startPosition;
        enemyAttackScript.Rest();
        enemyScript.currentHealth = enemyScript.maxHealth;
        isRespawn = true;

        enemyScript.isDied = false;
        AliveBug();
    }

    public void AliveBug()
    {
        enemyAttackScript.gameObject.SetActive(true);
    }
}

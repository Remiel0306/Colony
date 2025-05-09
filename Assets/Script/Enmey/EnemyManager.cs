using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] PlayerManager playerManager;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = enemyAttack.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isDied)
        {
            enemy.StartCoroutine(enemy.Died());
        }

        if (playerManager.isPlayerDead)
        {
            enemyAttack.StartCoroutine(enemyAttack.DiedDelay());
        }
    }

    public void ResetBugs()
    {
        playerManager.isPlayerDead = false;
        enemy.isDied = false;
        enemy.contactPlayer = false;
        enemy.isStop = false;
        enemyAttack.showUp = false;
        enemyAttack.move = false;
        enemyAttack.attackAgain = false;
        enemyAttack.boomAnimator.SetBool("isMoving", false);
        enemy.currentHealth = enemy.maxHealth;
        enemyAttack.transform.position = startPosition;
        enemyAttack.boomObj.SetActive(false);

        enemyAttack.StopAllCoroutines();
        enemyAttack.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        enemyAttack.bugBody.SetActive(true);
        enemyAttack.gameObject.SetActive(true);
    }
}

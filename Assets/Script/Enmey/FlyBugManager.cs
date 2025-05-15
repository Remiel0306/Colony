using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBugManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] FlyEnemyAttack flyEnemyAttack;
    [SerializeField] FlyEnemy flyEnemy;
    [SerializeField] GameObject flybug;
    [SerializeField] GameObject flyBugBody;

    public bool notReset = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flyEnemy.isFlyBugDied)
        {
            flyEnemyAttack.StartCoroutine(flyEnemyAttack.DiedDelay());
        }

        if (playerManager.isPlayerDead)
        {
            flyEnemyAttack.StartCoroutine(flyEnemyAttack.DiedDelay());
        }
    }

    public void FlyBugReset()
    {
        flyEnemyAttack.transform.position = flyEnemyAttack.originalPosition;
        flyEnemyAttack.isAttacking = false;
        flyEnemyAttack.facingRight = true;
        flyEnemyAttack.isEnter = false;
        flyEnemy.isFlyBugDied = false;
        flyEnemyAttack.currentState = FlyBugState.Stay;
        flyEnemyAttack.flyBugAnimator.SetBool("isAttack", false);
        flyEnemyAttack.flyBugAnimator.Play("Fly Bug Idle");

        flyEnemy.currentHealth = flyEnemy.maxHealth;
        flybug.SetActive(true);
        flyBugBody.SetActive(true);
    }
}

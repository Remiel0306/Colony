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

    bool deadOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();

        if (flyEnemyAttack == null)
            flyEnemyAttack = GetComponentInChildren<FlyEnemyAttack>();

        if (flyEnemy == null)
            flyEnemy = GetComponentInChildren<FlyEnemy>();

        if (flybug == null && flyEnemyAttack != null)
            flybug = flyEnemyAttack.gameObject;

        if (flyBugBody == null && flyEnemyAttack != null)
            flyBugBody = flyEnemyAttack.transform.GetChild(0).gameObject; // 視情況抓子物件
    }


    // Update is called once per frame
    void Update()
    {
        if (flyEnemy.isFlyBugDied && !deadOnce)
        {
            deadOnce = true;
            flyEnemyAttack.StartCoroutine(flyEnemyAttack.DiedDelay());
        }

        if (playerManager.isPlayerDead && !deadOnce)
        {
            deadOnce = true;
            flyEnemyAttack.StartCoroutine(flyEnemyAttack.DiedDelay());
        }
    }

    public void FlyBugReset()
    {
        flyEnemyAttack.transform.position = flyEnemyAttack.originalPosition;
        flyEnemyAttack.isAttacking = false;
        flyEnemyAttack.facingRight = false;
        flyEnemyAttack.isEnter = false;
        flyEnemy.isFlyBugDied = false;
        flyEnemyAttack.transform.Rotate(0f, 0f, 0f);
        flyEnemyAttack.currentState = FlyBugState.Stay;
        flyEnemyAttack.bodyCollider.enabled = false;
        flyEnemyAttack.flyBugAnimator.SetBool("isAttack", false);
        flyEnemyAttack.flyBugAnimator.Play("Fly Bug Idle");

        flyEnemy.currentHealth = flyEnemy.maxHealth;
        flybug.SetActive(true);
        flyBugBody.SetActive(true);

        deadOnce = false;
    }
}

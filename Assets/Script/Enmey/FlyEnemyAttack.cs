using System.Collections;
using UnityEngine;

public enum FlyBugState
{
    Stay,
    StayToAttack,
    Attack,
    AttackToBack,
    Back,
}

public class FlyEnemyAttack : MonoBehaviour 
{
    [SerializeField] GameObject player;
    [SerializeField] Transform playerTransform;
    [SerializeField] float speed = 5f;
    [SerializeField] FlyEnemy flyEnemyScript;
    [SerializeField] FlyBug flyBug;
    [SerializeField] Animator flyBugAnimator;

    FlyBugState currentState = FlyBugState.Stay;

    Vector3 targetCurrentPosition;
    Vector3 originalPosition;

    public bool finishDeathAnime = false;
    float attackDelay = 1.5f;
    float finishAttackDelay = 2f;
    bool isAttacking = false;
    bool reachedTarget = false;
    bool facingRight = false;
    bool isEnter = false;
    
    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        StateMachine();

        if(currentState != FlyBugState.Attack && flyEnemyScript.contactPlayer == true)
        {
            flyEnemyScript.contactPlayer = false;
        }

        if (currentState == FlyBugState.Attack && flyBug.isTouchGround)
        {
            currentState = FlyBugState.AttackToBack;
            flyBug.isTouchGround = false;
        }

        if (flyEnemyScript.isDied)
        {
            currentState = FlyBugState.AttackToBack;
            flyBugAnimator.Play("Fly Bug Death");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentState == FlyBugState.Stay)
        {
            currentState = FlyBugState.StayToAttack;

            isEnter = true;
        }
    }

    void StateMachine()
    {
        switch (currentState)
        {
            case FlyBugState.Stay:
                FaceingPlayer();
                break;

            case FlyBugState.StayToAttack:
                StartCoroutine(AttackAfterDelay());
                currentState = FlyBugState.Stay; // 防止重複啟動協程
                break;

            case FlyBugState.Attack:
                DoAttack();

                if (Vector3.Distance(transform.position, targetCurrentPosition) < 0.2f || flyEnemyScript.contactPlayer)
                {
                    currentState = FlyBugState.AttackToBack;
                }
                break;

            case FlyBugState.AttackToBack:
                flyEnemyScript.contactPlayer = false;
                StartCoroutine(FinishAttack());
                currentState = FlyBugState.Stay; // 防止重複啟動協程
                break;

            case FlyBugState.Back:
                StartCoroutine(BackToSky());
                currentState = FlyBugState.Stay; // 同上
                break;
        }
    }

    IEnumerator AttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        targetCurrentPosition = player.transform.position + Vector3.up * .15f;
        isAttacking = true;
        currentState = FlyBugState.Attack;
    }

    void DoAttack()
    {
        if (!isAttacking) return;

        Vector3 dir = (targetCurrentPosition - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        flyBugAnimator.SetBool("isAttack", true);
    }

    IEnumerator FinishAttack()
    {
        isAttacking = false;

        yield return new WaitForSeconds(finishAttackDelay);

        currentState = FlyBugState.Back;
        flyBugAnimator.SetBool("isAttack", false);
    }

    IEnumerator BackToSky()
    {
        float riseDistance = 3.5f;
        float riseSpeed = 2f; 
        float duration = riseDistance / riseSpeed;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * riseDistance;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos; // 確保落點精準
        currentState = FlyBugState.StayToAttack;
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
    }

    void FaceingPlayer()
    {
        if (isEnter)
        {
            if (transform.position.x < playerTransform.position.x && !facingRight)
            {
                Flip();
                facingRight = true;
            }
            if (transform.position.x > playerTransform.position.x && facingRight)
            {
                Flip();
                facingRight = false;
            }
        }
    }
}

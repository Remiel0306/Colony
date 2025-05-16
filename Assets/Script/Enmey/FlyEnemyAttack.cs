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
    [SerializeField] FlyEnemy flyEnemy;
    [SerializeField] FlyBugManager flyBugManager;
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] Transform playerTransform;
    [SerializeField] FlyEnemy flyEnemyScript;
    [SerializeField] FlyBug flyBug;
    [SerializeField] GameObject player;
    [SerializeField] public Animator flyBugAnimator;
    [SerializeField] Transform floorCheck;
    [SerializeField] Transform ceilingCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float speed = 5f;

    public FlyBugState currentState = FlyBugState.Stay;

    Vector3 targetCurrentPosition;
    public Vector3 originalPosition;

    public BoxCollider2D bodyCollider;
    public bool finishDeathAnime = false;
    float attackDelay = 1.5f;
    float finishAttackDelay = 2f;
    float touchWallCounter;
    float facingRightFloat = 0f;
    public bool isAttacking = false;
    public bool facingRight = false;
    public bool isEnter = false;
    bool reachedTarget = false;

    void Start()
    {
        originalPosition = transform.position;
        bodyCollider.enabled = false;
    }

    void Update()
    {
        Debug.Log(touchWallCounter);

        StateMachine();

        if (Vector3.Distance(transform.position, playerTransform.position) > 15f)
        {
            if (currentState != FlyBugState.Stay) 
            {
                StopAllCoroutines(); 
                StartCoroutine(ReturnToOriginalPosition());
                currentState = FlyBugState.Stay; 
            }
            return;
        }

        if (currentState != FlyBugState.Attack && flyEnemyScript.contactPlayer == true)
        {
            flyEnemyScript.contactPlayer = false;
        }

        if (currentState == FlyBugState.Attack && flyBug.isTouchGround)
        {
            currentState = FlyBugState.AttackToBack;
            flyBug.isTouchGround = false;
            touchWallCounter++;
        }

        if (facingRight)
        {
            facingRightFloat = -1f;
        }
        else
        {
            facingRightFloat = 1f;
        }

        if (flyEnemyScript.isFlyBugDied)
        {
            currentState = FlyBugState.AttackToBack;
            flyBugAnimator.Play("Fly Bug Death");
            flyBugManager.notReset = true;
            aimAndShoot.isKillFlyBug = true;
            flyEnemyScript.isFlyBugDied = false;
        }

        if (flyEnemy.isStop)
        {
            StartCoroutine(KnockbackFromPlayer(1f, 0.2f));
            currentState = FlyBugState.StayToAttack;
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

    void DoAttack()
    {
        if (!isAttacking) return;

        Vector3 dir = (targetCurrentPosition - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        flyBugAnimator.SetBool("isAttack", true);
    }
    IEnumerator AttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        
        bodyCollider.enabled = true;
        targetCurrentPosition = player.transform.position + Vector3.up * .15f;
        isAttacking = true;
        currentState = FlyBugState.Attack;
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
            if (IsTouchCeiling()) // 🔸 撞到天花板就中止
            {
                Debug.Log("撞到天花板，中止上升");
                break;
            }

            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = transform.position; // 保持當前位置
        currentState = FlyBugState.StayToAttack;
    }

    IEnumerator KnockbackFromPlayer(float force, float duration)
    {

        Vector2 direction = (transform.position - playerTransform.position).normalized;
        float timer = 0f;

        while (timer < duration)
        {
            transform.Translate(direction * facingRightFloat * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        flyEnemy.isStop = false;
    }

    public IEnumerator DiedDelay()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    IEnumerator ReturnToOriginalPosition()
    {
        isAttacking = false;
        flyBugAnimator.SetBool("isAttack", false);
        bodyCollider.enabled = false;

        Vector3 startPos = transform.position;
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        // 重設狀態與變數
        touchWallCounter = 0;
        flyEnemyScript.contactPlayer = false;
        flyEnemy.isStop = false;
        isAttacking = false;

        // 稍等一下才重新進入攻擊流程
        yield return new WaitForSeconds(1f);

        currentState = FlyBugState.StayToAttack;
        StartCoroutine(NotDeadReset());
    }

    IEnumerator NotDeadReset()
    {
        yield return new WaitForSeconds(.5f);

        flyBugManager.FlyBugReset();
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
    }

    bool IsTouchCeiling()
    {
        return Physics2D.OverlapBox(ceilingCheck.position, new Vector2(0.6f, 0.05f), 0, groundLayer);
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

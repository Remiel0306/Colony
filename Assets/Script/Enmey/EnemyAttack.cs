using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] UiManager uiManager;
    [SerializeField] EnemyManager enemyManagerScript;
    [SerializeField] bool facingRight = false;
    [SerializeField] bool attackAgain = true;
    [SerializeField] public float speed;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject boomObj;
    [SerializeField] public Animator boomAnimator;
    [SerializeField] BoxCollider2D bodyCollider;
    [SerializeField] BoxCollider2D hitTrigger;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] private ScreenShakeProfile boomProfile;


    public GameObject bugBody;
    public float stopAttackTime = 1.5f;
    public float attackAgainTime = 1.5f;
    public float showUpTime = 1.5f;
    bool showUp = false;
    bool isMove = false;
    bool isTurning = false;

    Enemy enemyScript;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    SpriteRenderer childSR;
    CinemachineImpulseSource impulseSource;
    public Vector3 startPosition;

    Color color;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        childSR = GetComponentInChildren<SpriteRenderer>();
        enemyScript = GetComponentInChildren<Enemy>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        color = childSR.color;
        color.a = 0.2f;
        childSR.color = color;
        boomObj.SetActive(false);
        bodyCollider.enabled = false;
        hitTrigger.enabled = false;

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("isDied: " + enemyScript.isDied);

        if (enemyManagerScript.isRespawn)
        {
            enemyManagerScript.isRespawn = false;
        }

        if (showUp)
        {
            color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime * 1f);
            childSR.color = color;
        }

        if (isMove && !enemyScript.contactPlayer && IsPlatformAhead() && !enemyScript.isStop)
        {
            boomAnimator.SetBool("isMoving", true);
            if (facingRight)
            {
                rb2D.velocity = new Vector3(speed * Time.deltaTime, 0f, 0f);
                StartCoroutine(StopAttack());
            }
            if (!facingRight)
            {
                rb2D.velocity = new Vector3(-speed * Time.deltaTime, 0f, 0f);
                StartCoroutine(StopAttack()); ;
            }
        }

        if (enemyScript.isStop)
        {
            StartCoroutine(AttackAgain());
        }

        if (IsPlatformAhead())
        {
            if (attackAgain) //After attack wait few seconds and use Coroutine to controller bool-move
            {
                StartCoroutine(AttackAgain());
                enemyScript.contactPlayer = false;

                if (transform.position.x > playerTransform.position.x)
                {
                    facingRight = false;
                    if (!facingRight)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                }
                if (transform.position.x < playerTransform.position.x)
                {
                    facingRight = true;
                    if (facingRight)
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                }
            }
        }

        if (IsPlatformAhead() == false)
        {
            FaceingPlayer();
        }


        if (bugBody != null && !bugBody.activeSelf)
        {
            gameObject.SetActive(false);
            isMove = false;
        }

        if (enemyScript.contactPlayer == true)
        {
            isMove = false;
            enemyScript.contactPlayer = false;
        }

        if (enemyScript.isDied)
        {
            speed = 0f;
            boomAnimator.Play("Boom Bug Boom");
            StartCoroutine(Boom());
        }

        if (!IsPlatformAhead() && !isTurning)
        {
            StartCoroutine(HandleTurnOnEdge());
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showUp = true;
            FaceingPlayer();
            StartCoroutine(StartMoving());
            boomAnimator.SetBool("isMoving", false);
        }
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
    }

    void FaceingPlayer()
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

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(showUpTime);

        isMove = true;
        bodyCollider.enabled = true;
        hitTrigger.enabled = true;
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(stopAttackTime);

        isMove = false;
        attackAgain = true;
    }

    IEnumerator AttackAgain()
    {
        yield return new WaitForSeconds(attackAgainTime);

        isMove = true;
        attackAgain = false;
        enemyScript.isStop = false;
    }
    bool IsPlatformAhead()
    {
        float checkDistance = 0.5f;
        Vector2 checkPosition = new Vector2(transform.position.x + (facingRight ? checkDistance : -checkDistance), transform.position.y - 0.5f);

        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        return hit.collider != null;
    }
    IEnumerator Boom()
    {
        yield return new WaitForSeconds(.4f);

        CameraShakeManager.instance.ScreenShakeFromProfle(boomProfile, impulseSource);
        boomObj.SetActive(true);
        enemyScript.isDied = true;
        enemyScript.isDied = false;
    }

    IEnumerator HandleTurnOnEdge()
    {
        isTurning = true;
        isMove = false; // °±¤U¨Ó
        FaceingPlayer(); // Â½Âà
        yield return new WaitForSeconds(0.5f);
        isMove = true;
        isTurning = false;
        boomAnimator.SetBool("isMoving", false);
    }

    public void BugDead()
    {
        speed = 0f;
        boomAnimator.Play("Boom Bug Boom");
        StartCoroutine(Boom());
    }
    public void Respawn()
    {
        boomObj.SetActive(false);
        enemyManagerScript.isRespawn = false;
    }

    public void Rest()
    {
        showUp = false;
        isMove = false;
        attackAgain = false;
    }
}

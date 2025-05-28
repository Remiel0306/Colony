using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] bool facingRight = false;
    [SerializeField] Transform playerTransform;
    [SerializeField] BoxCollider2D hitTrigger;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] ScreenShakeProfile boomProfile;

    public GameObject boomObj;
    public Animator boomAnimator;
    public BoxCollider2D bodyCollider;
    public bool move = false;
    public bool showUp = false;
    public bool attackAgain = true;
    public float stopAttackTime = 1.5f;
    public float attackAgainTime = 1.5f;
    public float showUpTime = 1.5f;
    public float speed = 400;
    public float startSpeed;
    bool isStopCoro = false;

    Enemy enemyScript;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    CinemachineImpulseSource impulseSource;
    public SpriteRenderer childSR;

    public Color color;

    public GameObject bugBody;
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
        startSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (showUp)
        {
            color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime * 1f);
            childSR.color = color;
        }

        if (move && enemyScript.contactPlayer == false && IsPlatformAhead()) // && !enemyScript.isStop
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

        if (enemyScript.isShotgunShoot && move)
        {
            float knockbackForce = 4f;
            speed = 0f;
            Vector2 knockbackDirection = facingRight ? Vector2.left : Vector2.right;

            rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(Wait());
        }
        else if (enemyScript.isShotgunShoot && !move)
        {
            float knockbackForce = 0.5f; // 可視情況調整，低於 1f 就很小了
            speed = 0f;

            Vector2 knockbackDirection = facingRight ? Vector2.left : Vector2.right;
            rb2D.velocity = knockbackDirection * knockbackForce;
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

        if (!IsPlatformAhead())
        {
            StopAttack();
            boomAnimator.SetBool("isMoving", false);
            FaceingPlayer();
        }

        if (bugBody != null && !bugBody.activeSelf)
        {
            gameObject.SetActive(false);
            move = false;
        }

        if (enemyScript.contactPlayer == true)
        {
            //move = false;
            StartCoroutine(StopAttack());
        }

        if (enemyScript.isDied)
        {
            speed = 0f;
            bodyCollider.enabled = false;
            boomAnimator.Play("Boom Bug Boom");
            enemyScript.isDied = false;
            aimAndShoot.isKillBoomBug = true;
            StartCoroutine(Boom());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        enemyScript.isShotgunShoot = false;
        speed = startSpeed;
        isStopCoro = false;
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

        move = true;
        bodyCollider.enabled = true;
        hitTrigger.enabled = true;
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(stopAttackTime);

        boomAnimator.SetBool("isMoving", false);
        enemyScript.contactPlayer = false;
        move = false;
        attackAgain = true;
        isStopCoro = true;
    }

    IEnumerator AttackAgain()
    {
        yield return new WaitForSeconds(attackAgainTime);

        move = true;
        attackAgain = false;
        enemyScript.isStop = false;
        boomAnimator.SetBool("isMoving", false);
    }
    bool IsPlatformAhead()
    {
        float checkDistance = 0.1f;
        Vector2 checkPosition = new Vector2(transform.position.x + (facingRight ? 1.5f : -1.5f), transform.position.y);

        Debug.DrawRay(checkPosition, Vector2.down * 1f, Color.red);  // 看 scene 中是否命中

        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, 5f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(.4f);

        CameraShakeManager.instance.ScreenShakeFromProfle(boomProfile, impulseSource);
        boomObj.SetActive(true);
    }

    public IEnumerator DiedDelay()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            StartCoroutine(StopAttack());
        }
    }
}
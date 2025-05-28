using Cinemachine;
using System.Collections;
using UnityEngine;

public class StupidEnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject boomBugFather;
    [SerializeField] GameObject boomObj;
    [SerializeField] Enemy enemy;
    [SerializeField] float speed;
    [SerializeField] float facingCheck;
    [SerializeField] float boomTime;
    [SerializeField] float startSpeed;
    [SerializeField] bool needFlip = false;
    [SerializeField] bool isMove = false;
    [SerializeField] bool isNeedFall = false;
    [SerializeField] bool startGroundCheck = false;
    [SerializeField] bool onLestGround = false;
    [SerializeField] bool doBoom = false;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator animator;
    [SerializeField] ScreenShakeProfile boomProfile;
    [SerializeField] float boomShakeDistance = 8f;

    CinemachineImpulseSource impulseSource;
    PlayerControl playerControl;
    bool facingRight = true;
    bool doingKnockBack = false;
    float knockbackForce = 20f;

    void Start()
    {
        boomTime = Random.Range(5f, 25f);
        boomObj.SetActive(false);
        startSpeed = speed;

        impulseSource = GetComponent<CinemachineImpulseSource>();
        playerControl = FindObjectOfType<PlayerControl>();

        StartCoroutine(BoomTimer());
    }

    void Update()
    {
        Debug.Log(onLestGround);

        if (isMove)
        {
            animator.SetBool("isMoving", true);
            rb2D.velocity = new Vector3(speed * facingCheck * Time.deltaTime, rb2D.velocity.y);
        }

        if (startGroundCheck)
        {
            IsGrounded();
        }

        if (!IsGrounded() && onLestGround)
        {
            Flip();
            facingCheck = facingCheck * -1;
        }

        if ((doBoom || enemy.isDied))
        {
            speed = 0f;
            animator.Play("Boom Bug Boom");
            animator.SetBool("isMoving", false);
            StartCoroutine(Boom());
        }

        if(playerControl.transform.position.x < transform.position.x)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }

        if (enemy.isShotgunShoot)
        {
            isMove = false;
            speed = 0f;
            Vector2 knockbackDirection = facingRight ? Vector2.right : Vector2.left;

            rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            enemy.isShotgunShoot = false;
            StartCoroutine(Wait());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level1DifferentGround"))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingRight = false;
            isMove = true;
        }

        if (collision.gameObject.CompareTag("Level1DifferentGround1-2"))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingRight = false;
            facingCheck = -1;
        }

        if (collision.gameObject.CompareTag("Level1DifferentGround2"))
        {
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            Flip();
            facingRight = true;
            facingCheck *= -1;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            startGroundCheck = true;
            onLestGround = true;
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Level1DifferentGround"))
                return false;

            return true;
        }

        return false;
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(0.4f);

        boomObj.SetActive(true);
        if (Vector2.Distance(transform.position, playerControl.transform.position) < boomShakeDistance)
        {
            CameraShakeManager.instance.ScreenShakeFromProfle(boomProfile, impulseSource);
        }

        yield return new WaitForSeconds(0.6f); // 等待動畫結束
        Destroy(boomBugFather);
    }

    IEnumerator BoomTimer()
    {
        yield return new WaitForSeconds(boomTime);
        doBoom = true;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        enemy.isShotgunShoot = false;
        speed = startSpeed;
        isMove = true;
    }
}
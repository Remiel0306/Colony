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

    CinemachineImpulseSource impulseSource;
    bool facingRight = true;

    void Start()
    {
        boomTime = Random.Range(5f, 30f);
        boomObj.SetActive(false);

        impulseSource = GetComponent<CinemachineImpulseSource>();
        StartCoroutine(BoomTimer());
    }

    void Update()
    {
        if (isMove)
        {
            animator.SetBool("isMoving", true);
            rb2D.velocity = new Vector2(speed * facingCheck * Time.deltaTime, rb2D.velocity.y);
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

        if (enemy.isShotgunShoot)
        {
            float knockbackForce = 4f;
            speed = 0f;
            Vector2 knockbackDirection = facingRight ? Vector2.left : Vector2.right;

            rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            enemy.isShotgunShoot = false;
        }
        else
        {
            speed = 250f;
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

        if (collision.gameObject.CompareTag("Level1DifferentGround2") || collision.gameObject.CompareTag("Level1DifferentGround1-2"))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingCheck = -1;
            facingRight = true;
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
        CameraShakeManager.instance.ScreenShakeFromProfle(boomProfile, impulseSource);

        yield return new WaitForSeconds(0.7f); // 等待動畫結束
        Destroy(boomBugFather);
    }

    IEnumerator BoomTimer()
    {
        yield return new WaitForSeconds(boomTime);
        doBoom = true;
    }
}
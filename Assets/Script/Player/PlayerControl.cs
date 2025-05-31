using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] Shotgun shotgun;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator gunAnimator;
    [SerializeField] Animator handAnimator;
    [SerializeField] float startSpeed;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpPower;
    [SerializeField] float doubleJumpPower;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float boomForce;
    [SerializeField] float wallCheckDistance = 0.1f;
    [SerializeField] BoxCollider2D boxCollider2D;

    [SerializeField] float acceleration = 20f;
    [SerializeField] float deceleration = 30f;

    [SerializeField] float boxBoomForce;  // 後座力大小，可以調整
    Transform boxBoomOrigin;  // 紀錄炸彈位置

    public Rigidbody2D rb2D;
    Vector2 vecGravity;

    public float speed = 5f;
    public bool facingRight = true;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer, enemyLayer;

    bool isTouchingWall;
    bool doublejump;
    bool isJumping;
    bool isBoom = false;
    bool boxBoom = false;
    float jumpCounter;
    float coyoteTimeCounter;
    float jumpBufferCounter;
    float currentSpeed = 0f;

    Transform boomOrigin;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        startSpeed = speed;
    }

    void Update()
    {
        float facingCheck = 0f;
        if (Input.GetKey(KeyCode.A)) facingCheck = -1f;
        else if (Input.GetKey(KeyCode.D)) facingCheck = 1f;

        if (aimAndShoot.canMove && !isBoom)
        {
            if (facingCheck != 0 && !WallCheck())
                currentSpeed = Mathf.MoveTowards(currentSpeed, facingCheck * speed, acceleration * Time.deltaTime);
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);

            rb2D.velocity = new Vector2(currentSpeed, rb2D.velocity.y);
        }

        bool isMoving = Mathf.Abs(currentSpeed) > 0.01f;
        bodyAnimator.SetBool("isMoving", isMoving);
        gunAnimator.SetBool("isMoving", isMoving);
        handAnimator.SetBool("isMoving", isMoving);

        if (facingCheck > 0 && !facingRight && !aimAndShoot.aimFlip) { Flip(); facingRight = true; }
        else if (facingCheck < 0 && facingRight && !aimAndShoot.aimFlip) { Flip(); facingRight = false; }

        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            doublejump = true;
            bodyAnimator.SetBool("isGrounded", true);
        }
        else coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;

            bodyAnimator.SetBool("isJumping", true);
            bodyAnimator.SetBool("isGrounded", false);
            gunAnimator.SetBool("isJumping", true);
            handAnimator.SetBool("isJumping", true);
        }
        else jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
                isJumping = true;
                jumpCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (doublejump)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpPower);
                isJumping = true;
                jumpCounter = 0f;
                jumpBufferCounter = 0f;
                doublejump = false;
            }
        }

        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
            if (shotgun.isShotgun)
            {
                fallMultiplier = 3f;
            }
            else
            {
                fallMultiplier = 6f;
            }

            bodyAnimator.SetBool("isJumping", false);
            gunAnimator.SetBool("isJumping", false);
            handAnimator.SetBool("isJumping", false);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            jumpCounter = 0f;
            if (rb2D.velocity.y > 0)
                rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.6f);
            coyoteTimeCounter = 0f;
        }

        if (doublejump)
            bodyAnimator.SetBool("isDoubleJump", false);
        else
            bodyAnimator.Play("Double Jump Body");

        if (!aimAndShoot.canMove)
        {
            speed = 0f;
            currentSpeed = 0f;
            StartCoroutine(ShotgunKnockBack());
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    bool isGrounded()
    {
        LayerMask combinedMask = groundLayer | enemyLayer; // 組合地面與敵人圖層
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.4f, 0.1f), 0, combinedMask);
    }

    bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.08f, 0.95f), 0, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boom"))
        {
            isBoom = true;
            boomOrigin = collision.transform;
            StartCoroutine(BoomKnockBack());
        }

        if (collision.gameObject.CompareTag("EletricBoxBoom"))
        {
            boxBoom = true;
            isBoom = true;
            boxBoomOrigin = collision.transform;
            StartCoroutine(BoxBoomKnockBack());
        }
    }

    IEnumerator BoomKnockBack()
    {
        speed = 0f;
        currentSpeed = 0f;

        Vector2 boomDir = ((Vector2)transform.position - (Vector2)boomOrigin.position).normalized;
        rb2D.velocity = Vector2.zero; // 清除舊速度避免方向亂掉
        rb2D.AddForce(boomDir * boomForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f); // 給爆炸反應一點時間
        speed = startSpeed;
        isBoom = false;
    }

    IEnumerator ShotgunKnockBack()
    {
        yield return new WaitForSeconds(0.28f);

        // 記錄當前的 Y 軸速度
        float cachedXVelocity = rb2D.velocity.x;

        yield return new WaitForEndOfFrame(); // 確保這是最後一幀不能控制的時機

        // 玩家可動
        aimAndShoot.shotgunIsShoot = false;
        speed = startSpeed;
        aimAndShoot.canMove = true;

        // 延續原來的 Y 方向速度
        rb2D.velocity = new Vector2(cachedXVelocity, rb2D.velocity.y);
    }


    IEnumerator BoxBoomKnockBack()
    {
        // 停止玩家移動
        speed = 0f;
        currentSpeed = 0f;

        // 計算後座力方向 (玩家位置 - 炸彈位置) 的反方向
        Vector2 knockBackDir = ((Vector2)transform.position - (Vector2)boxBoomOrigin.position).normalized;

        rb2D.velocity = Vector2.zero;  // 先清除原本速度
        rb2D.AddForce(knockBackDir * boxBoomForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f); // 後座力持續時間

        speed = startSpeed;
        boxBoom = false;
        isBoom = false;
    }
}

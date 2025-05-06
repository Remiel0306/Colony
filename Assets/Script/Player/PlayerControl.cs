using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;


public class PlayerControl : MonoBehaviour
{
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator gunAnimator;
    [SerializeField] Animator handAnimator;
    [SerializeField] float Speed;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpPower;
    [SerializeField] float doubleJumpPower;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float boomForce;
    [SerializeField] BoxCollider2D boxCollider2D;
    
    public Rigidbody2D rb2D;
    Vector2 vecGravity;

    public bool facingRight = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    bool doublejump;
    bool isJumping;
    bool onCrossGround = false;
    bool isBoom = false;
    bool isFlip = false;
    float jumpCounter;
    float coyoteTimeCounter;
    float jumpBufferCounter;
    Coroutine moveCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        vecGravity = new Vector2(0, -Physics2D.gravity.y);
    }

    // Update is called once per frame
    void Update()
    {
        //float facingCheck = 0f;
        //if (Input.GetKey(KeyCode.A))
        //{
        //    facingCheck = -1f;
        //}
        //else if(Input.GetKey(KeyCode.D))
        //{
        //    facingCheck = 1f;
        //}
        //if (facingCheck != 0)
        //{
        //    rb2D.velocity = new Vector2(facingCheck * Speed, rb2D.velocity.y);
        //}

        //if (facingCheck != 0)
        //{
        //    bodyAnimator.SetBool("isMoving", true);
        //    gunAnimator.SetBool("isMoving", true);
        //    handAnimator.SetBool("isMoving", true);
        //}

        //if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        //{
        //    bodyAnimator.SetBool("isMoving", false);
        //    gunAnimator.SetBool("isMoving", false);
        //    handAnimator.SetBool("isMoving", false);
        //}
        float facingCheck = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            facingCheck = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            facingCheck = 1f;
        }

        // 延遲移動控制（如果角色翻轉了，延遲一幀才移動）
        if (facingCheck != 0)
        {
            if ((facingCheck > 0 && !facingRight) || (facingCheck < 0 && facingRight))
            {
                Flip();
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(DelayedMove(facingCheck));
            }
            else
            {
                rb2D.velocity = new Vector2(facingCheck * Speed, rb2D.velocity.y);
            }

            // 移動動畫
            bodyAnimator.SetBool("isMoving", true);
            gunAnimator.SetBool("isMoving", true);
            handAnimator.SetBool("isMoving", true);
        }
        else
        {
            bodyAnimator.SetBool("isMoving", false);
            gunAnimator.SetBool("isMoving", false);
            handAnimator.SetBool("isMoving", false);
        }

        if (onCrossGround && Input.GetKeyDown(KeyCode.S))
        {
            boxCollider2D.enabled = false;
            StartCoroutine(ColliderBack());
        }

        if (facingCheck > 0 && !facingRight)
        {
            Flip();
            facingRight = true;
        }
        else if(facingCheck < 0 && facingRight)
        {
            Flip();
            facingRight = false;
        }

        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            doublejump = true;
            bodyAnimator.SetBool("isGrounded", true);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;

            bodyAnimator.SetBool("isJumping", true);
            bodyAnimator.SetBool("isGrounded", false);
            gunAnimator.SetBool("isJumping", true);
            handAnimator.SetBool("isJumping", true);
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
            isJumping = true;
            jumpCounter = 0f;
            jumpBufferCounter = 0f;

            //set animator bool
        }

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
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
                isJumping = true;
                jumpCounter = 0f;
                jumpBufferCounter = 0f;
                doublejump = false;
            }
        }

        if(rb2D.velocity.y < 0) // The falling moment
        {
            rb2D.velocity -= vecGravity * fallMultiplier * Time.deltaTime;

            bodyAnimator.SetBool("isJumping", false);
            gunAnimator.SetBool("isJumping", false);
            handAnimator.SetBool("isJumping", false);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            jumpCounter = 0f;
            if(rb2D.velocity.y > 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.6f);
            }
            coyoteTimeCounter = 0f;
        }

        if (doublejump) // && Input.GetKeyDown(KeyCode.Space)
        {
            bodyAnimator.SetBool("isDoubleJump", false);
        }
        else
        {
            bodyAnimator.Play("Double Jump Body");
        }

        if (isBoom)
        {
            Vector2 boomDirction = -gameObject.transform.right;
            rb2D.AddForce(boomDirction * boomForce, ForceMode2D.Impulse);

            isBoom = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.95f, 0.1f), 0, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CrossGround"))
        {
            onCrossGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CrossGround"))
        {
            onCrossGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boom"))
        {
            isBoom = true;
        }
    }
    IEnumerator ColliderBack()
    {
        yield return new WaitForSeconds(.2f);

        boxCollider2D.enabled = true;
    }
    IEnumerator DelayedMove(float direction)
    {
        yield return null; // 等待 1 frame
        rb2D.velocity = new Vector2(direction * Speed, rb2D.velocity.y);
    }
}

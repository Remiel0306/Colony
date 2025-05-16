using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class PlayerControl : MonoBehaviour //Fix boom knock back
{
    [SerializeField] AimAndShoot aimAndShoot;
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

    public Rigidbody2D rb2D;
    Vector2 vecGravity;

    public float speed = 5f;
    public bool facingRight = true;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;

    bool isTouchingWall;
    bool doublejump;
    bool isJumping;
    bool isBoom = false;
    bool isFlip = false;
    float jumpCounter;
    float coyoteTimeCounter;
    float jumpBufferCounter;
    float currentSpeed = 0f;
    Coroutine moveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        startSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Shougun Shot: " + aimAndShoot.shotgunIsShoot);

        float facingCheck = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            facingCheck = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            facingCheck = 1f;
        }

        if (aimAndShoot.canMove)
        {
            if (facingCheck != 0 && !WallCheck())
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, facingCheck * speed, acceleration * Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            }

            rb2D.velocity = new Vector2(currentSpeed, rb2D.velocity.y);
        }

        bool isMoving = Mathf.Abs(currentSpeed) > 0.01f;
        bodyAnimator.SetBool("isMoving", isMoving);
        gunAnimator.SetBool("isMoving", isMoving);
        handAnimator.SetBool("isMoving", isMoving);

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

        if (!aimAndShoot.canMove)
        {
            speed = 0f;
            currentSpeed = 0f;
            //aimAndShoot.canMove = false;
            StartCoroutine(ShotgunKnockBack());
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.4f, 0.1f), 0, groundLayer);
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
        }
    }
    IEnumerator ShotgunKnockBack()
    {
        yield return new WaitForSeconds(.3f);
        
        aimAndShoot.shotgunIsShoot = false;
        speed = startSpeed;
        aimAndShoot.canMove = true;
    }
}

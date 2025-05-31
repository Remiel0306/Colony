using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, IDamageable //
{
    [Header("Settings")]
    [SerializeField] public float maxHealth = 3f;
    [SerializeField] float knockBackForce = 2f;
    [SerializeField] AudioManager audioManager;

    [Header("References")]
    [SerializeField] FlyEnemyAttack flyEnemyAttack;
    [SerializeField] Collider2D flyBugBodyCollider;
    [SerializeField] Animator flyBugAnimator;
    [SerializeField] GameObject flyBugVisualRoot; // 包含圖層
    [SerializeField] GameObject flyBugWhite;

    [Header("Runtime Flags")]
    public bool isFlyBugDied = false;
    public bool contactPlayer = false;
    public bool isStop = false;

    [HideInInspector] public float currentHealth;
    Vector3 originalPosition;
    bool deadOnce = false;

    void Start()
    {
        currentHealth = maxHealth;
        originalPosition = transform.position;

        flyBugWhite.SetActive(false);

        if (flyEnemyAttack == null)
            flyEnemyAttack = GetComponentInChildren<FlyEnemyAttack>();

        if (flyBugAnimator == null)
            flyBugAnimator = GetComponentInChildren<Animator>();

        if (flyBugBodyCollider == null)
            flyBugBodyCollider = GetComponentInChildren<Collider2D>();
    }
    void Awake()
    {
        if (flyEnemyAttack == null)
            flyEnemyAttack = GetComponentInChildren<FlyEnemyAttack>();

        if (flyBugAnimator == null)
            flyBugAnimator = GetComponentInChildren<Animator>();

        if (flyBugBodyCollider == null)
            flyBugBodyCollider = GetComponentInChildren<Collider2D>();

        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();

        if (flyBugVisualRoot == null && flyEnemyAttack != null)
            flyBugVisualRoot = flyEnemyAttack.transform.GetChild(0).gameObject;
    }


    void Update()
    {
        if ((isFlyBugDied || flyEnemyAttack.playerManager.isPlayerDead) && !deadOnce)
        {
            deadOnce = true;
            StartCoroutine(ResetAfterDelay());
        }
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        audioManager?.PlayHitBugSFX(audioManager.hitBug);

        if (currentHealth <= 0 && !isFlyBugDied)
        {
            isFlyBugDied = true;
            StartCoroutine(Died());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            contactPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shotgun Bullet"))
        {
            isStop = true;
            flyBugWhite.SetActive(true);
            StartCoroutine(FlashBack());
        }

        audioManager?.PlayHitBugSFX(audioManager.hitBug);

        if(collision.gameObject.CompareTag("Normal Bullet"))
        {
            flyBugWhite.SetActive(true);
            StartCoroutine(FlashBack());
        }
    }

    IEnumerator FlashBack()
    {
        yield return new WaitForSeconds(0.08f);

        flyBugWhite.SetActive(false);
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(0.5f);
        flyBugVisualRoot?.SetActive(false);
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(2f); // 例如死亡後 2 秒重生
        ResetFlyBug();
    }

    public void ResetFlyBug()
    {
        transform.position = originalPosition;
        isFlyBugDied = false;
        contactPlayer = false;
        isStop = false;
        currentHealth = maxHealth;

        if (flyEnemyAttack != null)
        {
            flyEnemyAttack.ResetAttackState();
        }

        if (flyBugAnimator != null)
        {
            flyBugAnimator.SetBool("isAttack", false);
            flyBugAnimator.Play("Fly Bug Idle");
        }

        if (flyBugBodyCollider != null)
        {
            flyBugBodyCollider.enabled = false;
        }

        if (flyBugVisualRoot != null)
        {
            flyBugVisualRoot.SetActive(true);
        }

        deadOnce = false;
    }
}

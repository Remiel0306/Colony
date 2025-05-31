using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyAttack EnemyAttack;
    [SerializeField] AudioManager audioManager;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject flashWhite;
    [SerializeField] float knockBackForce = 2f;

    public float maxHealth = 5f;
    public float currentHealth;

    public bool isDied = false;
    public bool contactPlayer = false;
    public bool isStop = false;
    public bool isShotgunShoot = false;

    Rigidbody2D rb2DParent;


    void Start()
    {
        currentHealth = maxHealth;
        rb2DParent = GetComponentInParent<Rigidbody2D>();

        flashWhite.SetActive(false);
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        audioManager.PlayHitBugSFX(audioManager.hitBug);

        if (currentHealth <= 0)
        {
            isDied = true;
            StartCoroutine(Died());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shotgun Bullet"))
        {
            isStop = true;
            isShotgunShoot = true;
            audioManager.PlayHitBugSFX(audioManager.hitBug);

            flashWhite.SetActive(true);
            StartCoroutine(FlashBack());

            Debug.Log("222");

            currentHealth -= 0.4f;
        }

        if (collision.gameObject.CompareTag("Normal Bullet"))
        {
            audioManager.PlayHitBugSFX(audioManager.hitBug);

            flashWhite.SetActive(true);
            StartCoroutine(FlashBack());

            Debug.Log("111");

            currentHealth -= 1f;
        }

        if (currentHealth <= 0)
        {
            isDied = true;
            StartCoroutine(Died());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            contactPlayer = true;
            Debug.Log("Contacted Player");
        }
    }

    IEnumerator FlashBack()
    {
        yield return new WaitForSeconds(.08f);

        flashWhite.SetActive(false);
    }
    public IEnumerator Died()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}

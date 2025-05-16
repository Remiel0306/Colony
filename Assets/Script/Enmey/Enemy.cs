using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float knockBackForce = 2f;
    [SerializeField] EnemyAttack EnemyAttack;
    [SerializeField] SpriteRenderer spriteRenderer; // 設定敵人的 SpriteRenderer

    public float maxHealth = 5f;
    public bool isDied = false;
    public bool contactPlayer = false;
    public bool isStop = false;
    public bool isShotgunShoot = false;
    public float currentHealth;
    Rigidbody2D rb2DParent;

    bool isChangeColor = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb2DParent = GetComponentInParent<Rigidbody2D>();
    }
    void Update()
    {
        if (isChangeColor)
        {
            StartCoroutine(FlashHit());
            isChangeColor = false;
        }
    }
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        isChangeColor = true;

        if (currentHealth < 0)
        {
            isDied = true;

            StartCoroutine(Died());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            contactPlayer = true;

            Debug.Log("is Contact Player " + contactPlayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shotgun Bullet"))
        {
            isStop = true;
            isShotgunShoot = true;
        }

        if (collision.gameObject.CompareTag("Normal Bullet"))
        {
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;
        }
    }

    private IEnumerator FlashHit()
    {
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public IEnumerator Died()
    {
        yield return new WaitForSeconds(.7f);

        gameObject.SetActive(false);
    }
}

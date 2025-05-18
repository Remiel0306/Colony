using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] public float maxHealth = 3f;
    [SerializeField] float knockBackForce = 2f;

    public bool isFlyBugDied = false;
    public bool contactPlayer = false;
    public bool isStop = false;

    public float currentHealth;
    Rigidbody2D rb2DParent;

    void Start()
    {
        currentHealth = maxHealth;
        rb2DParent = GetComponentInParent<Rigidbody2D>();
    }
    private void Update()
    {

    }
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        audioManager.PlayHitBugSFX(audioManager.hitBug);

        if (currentHealth < 0)
        {
            isFlyBugDied = true;

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
        }

        audioManager.PlayHitBugSFX(audioManager.hitBug);
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(.5f);

        gameObject.SetActive(false);
    }
}

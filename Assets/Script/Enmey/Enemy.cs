using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] float knockBackForce = 2f;

    public bool isDied = false;
    public bool contactPlayer = false;
    private float currentHealth;
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
        
        if(currentHealth < 0)
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
            Debug.Log("is shotgun bullet");

            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb2DParent.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(.5f);

        gameObject.SetActive(false);
    }
}

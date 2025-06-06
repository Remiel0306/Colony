using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float maxHealth = 5f;
    [SerializeField] float knockBackForce = 2f;

    public bool isDied = false;
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
        Debug.Log("boom bug health: " + currentHealth + "," + isDied);
        if (isStop)
        {
            //OnTriggerEnter2D("Shotgun Bullet");
        }
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
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb2DParent.AddForce(direction * knockBackForce, ForceMode2D.Impulse);

            isStop = true;
        }
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(.7f);

        gameObject.SetActive(false);
    }
}

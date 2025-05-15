using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] UiManager uiManager;
    [SerializeField] int boomDamage = 2;
    [SerializeField] int boomBugDamage = 1;
    [SerializeField] int flyBugDamage = 1;
    [SerializeField] int toxicMucus = 1;
    [SerializeField] int respawnHealth = 3;

    public bool isPlayerDead = false;
    public int maxHealth = 6;
    public bool isRespawn = false;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0 && !isPlayerDead)
        {
            isPlayerDead = true;
            gameObject.SetActive(false);
        }

        if (isPlayerDead)
        {
            gameObject.SetActive(false);
        }

        if(isRespawn)
        {
            currentHealth = respawnHealth;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Fly Bug"))
        {
            currentHealth -= flyBugDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boom"))
        {
            currentHealth -= boomDamage;

            Debug.Log("Boom Attack");
        }

        if (collision.gameObject.CompareTag("BoomBug"))
        {
            currentHealth -= boomBugDamage;

            Debug.Log("Touch");
        }

        if(collision.gameObject.CompareTag("Toxic Mucus"))
        {
            currentHealth -= toxicMucus;
        }
    }
}

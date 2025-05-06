using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] UiManager uiManager;
    [SerializeField] int maxHealth = 5;
    [SerializeField] int boomDamage = 2;
    [SerializeField] int boomBugDamage = 1;
    [SerializeField] int flyBugDamage = 1;
    [SerializeField] int toxicMucus = 1;

    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
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

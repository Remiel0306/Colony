using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiManager;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] UiManager uiManager;
    [SerializeField] int maxHealth = 5;
    [SerializeField] int boomDamage = 2;
    [SerializeField] int boomBugDamage = 1;

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
        if (collision.gameObject.CompareTag("BoomBug"))
        {
            currentHealth -= boomBugDamage;

            Debug.Log("Touch");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boom"))
        {
            currentHealth -= boomDamage;

            Debug.Log("Boom Attack");
        }
    }
}

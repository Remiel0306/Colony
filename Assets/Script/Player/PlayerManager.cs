using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour //
{
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] UiManager uiManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] BugGroupControl bugGroupControl;
    [SerializeField] int boomDamage = 2;
    [SerializeField] int boomBugDamage = 1;
    [SerializeField] int flyBugDamage = 1;
    [SerializeField] int toxicMucus = 1;
    [SerializeField] int respawnHealth = 3;
    [SerializeField] int batteryAddBullet = 8;
    [SerializeField] int batteryAddHealth = 3;
    [SerializeField] Transform respawnPoint2;

    public SpriteRenderer spriteRenderer;
    public bool isPlayerDead = false;
    public int maxHealth = 6;
    public bool isRespawn = false;
    public int currentHealth;
    bool isChangeColor = false;

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

        if(isChangeColor)
        {
            StartCoroutine(ChangeColorBack());
            isChangeColor = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Fly Bug"))
        {
            currentHealth -= flyBugDamage;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;

            audioManager.PlaySFX(audioManager.playerHurt);
        }

        if (collision.gameObject.CompareTag("Battery"))
        {
            int healthToAdd = Mathf.Min(3, maxHealth - currentHealth);
            currentHealth += healthToAdd;

            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boom"))
        {
            currentHealth -= boomDamage;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;
            audioManager.PlaySFX(audioManager.playerHurt);
        }

        if (collision.gameObject.CompareTag("BoomBug"))
        {
            currentHealth -= boomBugDamage;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;
            audioManager.PlaySFX(audioManager.playerHurt);
        }

        if(collision.gameObject.CompareTag("Toxic Mucus"))
        { 
            currentHealth -= toxicMucus;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;
            audioManager.PlaySFX(audioManager.playerHurt);
        }

        if (collision.gameObject.CompareTag("Toxic Mucus Dead"))
        {
            currentHealth = 0;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;
            audioManager.PlaySFX(audioManager.playerHurt);
        }

        if (collision.gameObject.CompareTag("Level2Respawn"))
        {
            uiManager.currentRespowanPoinot = respawnPoint2.transform;
            bugGroupControl.ActivateStage(2);
        }
    }

    IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white; 
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerManager : MonoBehaviour //
{
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] UiManager uiManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] BugGroupControl bugGroupControl;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    [SerializeField] int boomDamage = 2;
    [SerializeField] int boomBugDamage = 1;
    [SerializeField] int flyBugDamage = 1;
    [SerializeField] int toxicMucus = 1;
    [SerializeField] int respawnHealth = 3;
    [SerializeField] int batteryAddBullet = 8;
    [SerializeField] int batteryAddHealth = 3;
    [SerializeField] Transform respawnPoint1_5, respawnPoint2, respawnPoint3, respawnPoint4, respawnPoint5, respawnPoint6, respawnPoint7, respawnPoint8, respawnPoint9, respawnPoint10;

    public SpriteRenderer spriteRenderer;
    public bool isPlayerDead = false;
    public bool isRespawn = false;
    public int levelCounter = 0;
    public int maxHealth = 6;
    public int currentHealth;

    CinemachineTransposer transposer;
    Vector3 cinemachinOffest = new Vector3(3f, 0f, 0f);
    float lerpSpeed = 2f;
    bool isChangeColor = false;

    void Start()
    {
        currentHealth = maxHealth;

        transposer = cinemachine.GetCinemachineComponent<CinemachineTransposer>();
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

        if (isRespawn)
        {
            currentHealth = respawnHealth;
        }

        if (isChangeColor)
        {
            StartCoroutine(ChangeColorBack());
            isChangeColor = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fly Bug"))
        {
            currentHealth -= flyBugDamage;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;

            audioManager.PlaySFX(audioManager.playerHurt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Battery"))
        {
            int healthToAdd = Mathf.Min(3, maxHealth - currentHealth);
            currentHealth += healthToAdd;

            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Boom"))
        {
            currentHealth -= boomDamage;
            spriteRenderer.color = new Color(.5f, .2f, .2f, 1f);
            isChangeColor = true;
            audioManager.PlaySFX(audioManager.playerHurt);
        }

        if (collision.gameObject.CompareTag("StupidBoom"))
        {
            currentHealth -= 1;
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

        if (collision.gameObject.CompareTag("Toxic Mucus"))
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

        if (collision.gameObject.CompareTag("Level1Respawn"))
        {
            uiManager.currentRespowanPoinot = respawnPoint2.transform;
            bugGroupControl.ActivateStage(1);
            levelCounter = 1;
        }

        if (collision.gameObject.CompareTag("Level1.5Respawn"))
        {
            uiManager.currentRespowanPoinot = respawnPoint1_5.transform;
            bugGroupControl.ActivateStage(2);
            levelCounter = 2;
        }

        if (collision.gameObject.CompareTag("Level2Respawn"))
        {
            uiManager.currentRespowanPoinot = respawnPoint3.transform;
            bugGroupControl.ActivateStage(3);
            levelCounter = 3;
            StartCoroutine(LerpCameraOffset(new Vector3(3f, 0f, 0f), 1.5f));
        }

        if (collision.gameObject.CompareTag("Level2Respawn2"))
        {
            uiManager.currentRespowanPoinot = respawnPoint4.transform;
        }
        if (collision.gameObject.CompareTag("Level2Respawn3"))
        {
            uiManager.currentRespowanPoinot = respawnPoint5.transform;
        }
        if (collision.gameObject.CompareTag("Level2Respawn4"))
        {
            uiManager.currentRespowanPoinot = respawnPoint6.transform;
        }
        if (collision.gameObject.CompareTag("Level2Respawn5"))
        {
            uiManager.currentRespowanPoinot = respawnPoint7.transform;
        }
        if (collision.gameObject.CompareTag("Level2Respawn6"))
        {
            uiManager.currentRespowanPoinot = respawnPoint8.transform;
        }
        if (collision.gameObject.CompareTag("Level2Respawn7"))
        {
            uiManager.currentRespowanPoinot = respawnPoint9.transform;
        }
        if (collision.gameObject.CompareTag("Level2Respawn8"))
        {
            uiManager.currentRespowanPoinot = respawnPoint10.transform;
        }
    }

    IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
    IEnumerator LerpCameraOffset(Vector3 targetOffset, float duration)
    {
        if (transposer == null) yield break;

        Vector3 startOffset = transposer.m_FollowOffset;
        float time = 0f;

        while (time < duration)
        {
            transposer.m_FollowOffset = Vector3.Lerp(startOffset, targetOffset, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transposer.m_FollowOffset = targetOffset;
    }
}

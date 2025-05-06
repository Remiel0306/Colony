using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] EnemyAttack enemyAttackScript;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] Enemy enemyScript;
    [SerializeField] GameObject detectorUi;
    [SerializeField] GameObject[] healthBlocks;
    [SerializeField] GameObject one;
    [SerializeField] Image blackPanel;
    [SerializeField] GameObject respawnBtn;
    [SerializeField] Transform respawnPoint1;
    [SerializeField] GameObject boomBugs;

    public int openDetector = 0;

    private bool hasStartedFade = false;

    void Start()
    {
        detectorUi.SetActive(false);
        respawnBtn.SetActive(false);

        // 黑幕一開始設為透明
        blackPanel.color = new Color(0, 0, 0, 0);

        // 確保按鈕有 CanvasGroup
        var cg = respawnBtn.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = respawnBtn.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    void Update()
    {
        UpdateHp();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            openDetector++;
            detectorUi.SetActive(openDetector % 2 != 0);
        }

        if (playerManager.isPlayerDead && !hasStartedFade)
        {
            hasStartedFade = true;
            StartCoroutine(FadeInBlackPanel());
        }
    }

    public void UpdateHp()
    {
        for (int i = 0; i < healthBlocks.Length; i++)
        {
            healthBlocks[i].SetActive(i < playerManager.currentHealth);
        }
    }

    public void PlayerRespawnBtn()
    {
        playerManager.gameObject.SetActive(true);
        playerManager.transform.position = respawnPoint1.position;
        playerManager.currentHealth = 3;

        // Reset UI
        hasStartedFade = false;
        blackPanel.color = new Color(0, 0, 0, 0);

        var cg = respawnBtn.GetComponent<CanvasGroup>();
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        respawnBtn.SetActive(false);

        playerManager.isPlayerDead = false;

        StartCoroutine(FadeOutUI());

        enemyManager.RespawnRest();

        enemyAttackScript.gameObject.SetActive(true);
        enemyAttackScript.transform.position = enemyAttackScript.startPosition;
        enemyAttackScript.Rest();
        enemyScript.currentHealth = enemyScript.maxHealth;

        enemyScript.isDied = false;
    }

    IEnumerator FadeInBlackPanel()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            blackPanel.color = new Color(0f, 0f, 0f, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        blackPanel.color = new Color(0f, 0f, 0f, 1f);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeInRespawnButton());
    }

    IEnumerator FadeInRespawnButton()
    {
        respawnBtn.SetActive(true);
        CanvasGroup cg = respawnBtn.GetComponent<CanvasGroup>();

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    IEnumerator FadeOutUI()
    {
        float duration = 1.5f;
        float elapsed = 0f;

        CanvasGroup cg = respawnBtn.GetComponent<CanvasGroup>();
        Color startColor = blackPanel.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            blackPanel.color = new Color(0f, 0f, 0f, alpha);
            cg.alpha = alpha;
            elapsed += Time.deltaTime;
            yield return null;
        }

        blackPanel.color = new Color(0f, 0f, 0f, 0f);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        respawnBtn.SetActive(false);
    }
}

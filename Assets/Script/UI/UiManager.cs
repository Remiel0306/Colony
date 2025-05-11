using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] GameObject detectorUi;
    [SerializeField] GameObject[] healthBlocks;
    [SerializeField] Image blackPannel;
    [SerializeField] GameObject respawnBtn;
    [SerializeField] Transform level1Point;

    public int openDetector = 0;

    bool isPannelOpen = false;

    // 固定 panel 顏色為 RGB(225,225,225) = 0.882f
    private Color panelBaseColor = new Color(0.882f, 0.882f, 0.882f, 0f);

    void Start()
    {
        detectorUi.SetActive(false);
        respawnBtn.SetActive(false);
        blackPannel.color = panelBaseColor;

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

            if (openDetector % 2 == 0)
            {
                detectorUi.SetActive(openDetector % 2 != 0);
            }
        }

        if (playerManager.isPlayerDead && !isPannelOpen)
        {
            StartCoroutine(FadeInBlackPanel());
            isPannelOpen = true;
        }
    }

    public void UpdateHp()
    {
        for (int i = 0; i < healthBlocks.Length; i++)
        {
            healthBlocks[i].SetActive(i < playerManager.currentHealth);
        }
    }

    public void RespawnButton()
    {
        StartCoroutine(FadeOutUI());
        isPannelOpen = false;

        playerManager.gameObject.transform.position = level1Point.position;
        playerManager.gameObject.SetActive(true);
        playerManager.currentHealth = 3;
        playerManager.isPlayerDead = false;
        aimAndShoot.isAim = false;

        enemyManager.ResetBugs();
    }

    IEnumerator FadeInBlackPanel()
    {
        yield return new WaitForSeconds(0.5f);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            Color c = panelBaseColor;
            c.a = alpha;
            blackPannel.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Color final = panelBaseColor;
        final.a = 1f;
        blackPannel.color = final;

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
        float blackPanelDuration = 1f;
        float buttonDuration = 0.4f;
        float totalDuration = Mathf.Max(blackPanelDuration, buttonDuration);

        CanvasGroup cg = respawnBtn.GetComponent<CanvasGroup>();

        for (float t = 0f; t < totalDuration; t += Time.deltaTime)
        {
            float alphaBlack = Mathf.Lerp(1f, 0f, Mathf.Clamp01(t / blackPanelDuration));
            float alphaBtn = Mathf.Lerp(1f, 0f, Mathf.Clamp01(t / buttonDuration));

            Color c = panelBaseColor;
            c.a = alphaBlack;
            blackPannel.color = c;
            cg.alpha = alphaBtn;

            yield return null;
        }

        // 最終設定
        Color final = panelBaseColor;
        final.a = 0f;
        blackPannel.color = final;

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        respawnBtn.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] FlyBugManager flyBugManager;
    [SerializeField] BugRespawnCtrl bugRespawnCtrl;
    [SerializeField] LevelManager levelManager;
    [SerializeField] BugGroupControl bugGroupControl;
    [SerializeField] GameObject level1Enemy;
    [SerializeField] GameObject detectorUi;
    [SerializeField] GameObject[] healthBlocks;
    [SerializeField] GameObject[] battery;
    [SerializeField] GameObject healthBox;
    [SerializeField] Image blackPannel;
    [SerializeField] float maxEnergy;
    [SerializeField] float currentEnergy;
    [SerializeField] GameObject respawnBtn;
    [SerializeField] Transform level1Point;
    public Transform currentRespowanPoinot;

    bool isPannelOpen = false;
    bool isDetectorOpen = false;
    bool resetLevel = false;

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

        currentRespowanPoinot = level1Point.transform;
    }

    void Update()
    {
        UpdateHp();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isDetectorOpen = !isDetectorOpen;
            detectorUi.SetActive(isDetectorOpen);
        }

        if (playerManager.isPlayerDead && !isPannelOpen)
        {
            StartCoroutine(FadeInBlackPanel());
            isPannelOpen = true;

            healthBox.SetActive(false);
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

        playerManager.gameObject.transform.position = currentRespowanPoinot.position;
        playerManager.gameObject.SetActive(true);
        healthBox.SetActive(true);
        playerManager.currentHealth = 3;
        playerManager.isPlayerDead = false;
        playerManager.spriteRenderer.color = Color.white;
        aimAndShoot.isAim = false;
        foreach (GameObject b in battery)
        {
            b.SetActive(true);
        }

        enemyManager.ResetBugs();
        flyBugManager.FlyBugReset();
        bugRespawnCtrl.ResetAllFlyBug();
        resetLevel = true;
    }
    IEnumerator FadeInBlackPanel()
    {
        yield return new WaitForSeconds(0.3f);
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
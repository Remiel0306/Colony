using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject detectorUi;
    [SerializeField] GameObject[] healthBlocks;
    [SerializeField] Image blackPannel;
    [SerializeField] GameObject respawnBtn;
    [SerializeField] Transform level1Point;

    public int openDetector = 0;

    bool isPannelOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        detectorUi.SetActive(false);
        respawnBtn.SetActive(false);
        blackPannel.color = new Color(0, 0, 0, 0);

        // 確保按鈕有 CanvasGroup
        var cg = respawnBtn.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = respawnBtn.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(playerManager.isPlayerDead);
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
            if (i < playerManager.currentHealth)
            {
                healthBlocks[i].SetActive(true);  // 還有血：打開
            }
            else
            {
                healthBlocks[i].SetActive(false); // 沒血了：關閉
            }
        }
    }

    public void RespawnButton()
    {
        playerManager.gameObject.transform.position = level1Point.position;
        playerManager.gameObject.SetActive(true);
        playerManager.currentHealth = 3;
        playerManager.isPlayerDead = false;

        isPannelOpen = false;
        StartCoroutine(FadeOutUI());
    }

    IEnumerator FadeInBlackPanel()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            blackPannel.color = new Color(0f, 0f, 0f, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        blackPannel.color = new Color(0f, 0f, 0f, 1f);

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

            blackPannel.color = new Color(0f, 0f, 0f, alphaBlack);
            cg.alpha = alphaBtn;

            yield return null;
        }

        // 最終設定
        blackPannel.color = new Color(0f, 0f, 0f, 0f);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        respawnBtn.SetActive(false);
    }
}

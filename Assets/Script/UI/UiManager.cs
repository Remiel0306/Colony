using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject detectorUi;
    [SerializeField] GameObject[] healthBlocks;
    [SerializeField] GameObject one;

    public int openDetector = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        detectorUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            openDetector++;

            if (openDetector % 2 == 0)
            {
                detectorUi.SetActive(false);
            }
            else
            {
                detectorUi.SetActive(true);
            }
        }

        UpdateHp();
        //if (playerManager.currentHealth == 3)
        //{
        //    healthBlocks[0].SetActive(false);
        //    one.SetActive(false);
        //}
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
}

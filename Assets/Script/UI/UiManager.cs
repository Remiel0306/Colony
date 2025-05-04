using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject detectorUi;
    [SerializeField] GameObject[] healthBlocks;

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

    }

    public void UpdateHp()
    {
        for (int i = playerManager.currentHealth; i < 6; i--)
        {
            healthBlocks[i].SetActive(false);
        }
    }
}

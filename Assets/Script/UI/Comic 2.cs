using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comic2 : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D playerRb2D;
    [SerializeField] GameObject pageDownBtn;
    [SerializeField] GameObject blackGround;
    [SerializeField] GameObject[] comicPage;
    [SerializeField] Transform fallPosition;
    [SerializeField] bool openComic = false;
    [SerializeField] bool openOnce = false;
    [SerializeField] bool isFinish = false;
    [SerializeField] public int pageCounter = 0;

    bool shouldTeleport = false;

    void Start()
    {
        blackGround.SetActive(false);
        pageDownBtn.SetActive(false);

        foreach (GameObject page in comicPage)
        {
            page.SetActive(false);
        }
    }

    void Update()
    {
        if (openComic && !openOnce)
        {
            if (pageCounter < comicPage.Length)
            {
                //// 關閉所有頁面
                //foreach (GameObject page in comicPage)
                //{
                //    page.SetActive(false);
                //}

                // 分段管理
                if (pageCounter < 3)
                {
                    // 顯示第 0~3 段落的其中一張
                    for (int i = pageCounter; i < 3; i++)
                    {
                        comicPage[i].SetActive(i == pageCounter);
                    }
                }
                else if (pageCounter > 2 && pageCounter < 7)
                {
                    // 顯示第 4~6 段落的其中一張
                    comicPage[0].SetActive(false);
                    comicPage[1].SetActive(false);
                    comicPage[2].SetActive(false);


                    for (int i = pageCounter; i < 7; i++)
                    {
                        comicPage[i].SetActive(i == pageCounter);
                    }
                }
                else
                {
                    // 顯示最後一頁 (第7)
                    comicPage[7].SetActive(true);
                }
            }
            else
            {
                // 超過最後一頁：關閉所有漫畫與 UI
                foreach (GameObject page in comicPage)
                {
                    page.SetActive(false);
                }

                blackGround.SetActive(false);
                pageDownBtn.SetActive(false);
                isFinish = true;
                shouldTeleport = true;
                openComic = true;
            }
        }


    }

    void FixedUpdate()
    {
        if (shouldTeleport)
        {
            //player.gameObject.transform.position = fallPosition.transform.position;

            playerRb2D.velocity = Vector2.zero;
            playerRb2D.MovePosition(fallPosition.transform.position);

            Debug.Log("12456");
            isFinish = false;
            shouldTeleport = false;
            openOnce = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            openComic = true;
            blackGround.SetActive(true);
            pageDownBtn.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level2BugContol : MonoBehaviour
{
    [SerializeField] GameObject flyBugPrefab;
    [SerializeField] GameObject boomBugPrefab;
    [SerializeField] Transform bugCreat1, bugCreat2;
    [SerializeField] Transform boomBugCreat;
    [SerializeField] FlyEnemy flyEnemy;
    [SerializeField] bool isCreatBoom = false;
    [SerializeField] bool isCreatBugs = false;

    public bool isPlayerIn = false;
    SpriteRenderer boomSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerIn && !isCreatBugs)
        {
            StartCoroutine(StartCreatBug());
            isCreatBugs = true;
        }

        if (isPlayerIn && !isCreatBoom)
        {
            StartCoroutine(StartCreatBoomBug());
            isCreatBoom = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerIn = false;
        }
    }

    IEnumerator StartCreatBug()
    {
        yield return new WaitForSeconds(3f);

        Instantiate(flyBugPrefab, bugCreat1.position, Quaternion.identity);
        Instantiate(flyBugPrefab, bugCreat2.position, Quaternion.identity);

        isCreatBugs = false;
    }

    IEnumerator StartCreatBoomBug()
    {
        yield return new WaitForSeconds(3f);

        if (boomBugPrefab != null)
        {
            GameObject boomBug = Instantiate(boomBugPrefab, boomBugCreat.position, Quaternion.identity);

            // 這裡才去抓 SpriteRenderer
            SpriteRenderer boomSprite = boomBug.GetComponent<SpriteRenderer>();
            if (boomSprite != null)
            {
                boomSprite.flipX = false;
            }
        }

        isCreatBoom = false;
    }
}

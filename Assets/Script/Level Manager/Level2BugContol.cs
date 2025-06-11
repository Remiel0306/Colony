using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level2BugContol : MonoBehaviour
{
    [SerializeField] GameObject flyBugFrefab;
    [SerializeField] Transform bugCreat1, bugCreat2;
    [SerializeField] private CreatBoomBug playerSettings;
    [SerializeField] bool isPlayerIn = false;
    [SerializeField] bool isCreatBugs = false;


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
        yield return new WaitForSeconds(2.5f);

        Instantiate(flyBugFrefab, bugCreat1.position, Quaternion.identity);
        Instantiate(flyBugFrefab, bugCreat2.position, Quaternion.identity);

        isCreatBugs = false;
    }
}

[System.Serializable]
public class CreatBoomBug
{
    [SerializeField] Transform boomBugCreat;
    [SerializeField] GameObject boomBugFrefab;
}

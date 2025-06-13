using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comic3 : MonoBehaviour
{
    [SerializeField] GameObject[] comicPage;
    [SerializeField] GameObject pageDownBtn;
    public int pageCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        pageCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(pageCounter);
        //if(pageCounter <= 0)
        //{
        //    for (int i = pageCounter; i < comicPage.Length; i++)
        //    {
        //        comicPage[i].SetActive(i == pageCounter);
        //    }
        //}
        for (int i = pageCounter; i < comicPage.Length; i++)
        {
            comicPage[i].SetActive(i == pageCounter);
        }

        if (pageCounter == comicPage.Length)
        {
            for (int i = pageCounter; i < comicPage.Length; i++)
            {
                comicPage[i].SetActive(false);
            }

            SceneManager.LoadScene("Menu");
        }
    }
}

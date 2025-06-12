using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Comic1 : MonoBehaviour
{
    [SerializeField] GameObject blackGround;
    [SerializeField] GameObject[] comicPage;
    [SerializeField] bool openComic = false;
    [SerializeField] bool openOnce = false;
    [SerializeField] int pageCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(openComic && !openOnce)
        {
            for (int i = pageCounter; i < comicPage.Length; i++)
            {
                comicPage[i].SetActive(i == pageCounter);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            openComic = true;
        }
    }

    public void PageUpButton()
    {
        pageCounter++;
    }

    public void PageDownButton()
    {
        pageCounter--;
    }
}

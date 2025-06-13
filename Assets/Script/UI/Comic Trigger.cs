using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicTrigger : MonoBehaviour
{
    [SerializeField] GameObject eKey;
    [SerializeField] bool isShow = false;
    // Start is called before the first frame update
    void Start()
    {
        eKey.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            eKey.SetActive(true);
            if(Input.GetKey(KeyCode.E))
            {
                SceneManager.LoadScene("End Story");
            }
        }
        else
        {
            eKey.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isShow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isShow = false;
        }
    }
}

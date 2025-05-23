using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayBoardTeach : MonoBehaviour
{
    [SerializeField] GameObject oneWayTeachText;
    // Start is called before the first frame update
    void Start()
    {
        oneWayTeachText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            oneWayTeachText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            oneWayTeachText.SetActive(false);
        }
    }
}

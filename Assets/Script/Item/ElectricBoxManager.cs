using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBoxManager : MonoBehaviour
{
    [SerializeField] ElectricBox electricBox;
    [SerializeField] ElectricBoxTeach electricBoxTeach;
    [SerializeField] GameObject eKey;

    public bool openBox = false;
    bool eKeyShow = false;
    // Start is called before the first frame update
    void Start()
    {
        eKey.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (eKeyShow)
        {
            eKey.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                openBox = true;
                //eKey.SetActive(false);
            }
        }
        else
        {
            eKey.SetActive(false);
            eKeyShow = false;
            openBox = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eKeyShow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eKeyShow = false;
            if(electricBox.currentPower < 9)
            {
                electricBox.currentPower = 0;
                electricBoxTeach.currentPower = 0;
            }
        }
    }
}

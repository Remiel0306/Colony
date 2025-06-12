using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchGunTrigger : MonoBehaviour
{
    [SerializeField] GameObject switchGunUi;
    [SerializeField] bool isShow = false;

    // Start is called before the first frame update
    void Start()
    {
        //switchGunUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            switchGunUi.SetActive(true);
        }
        else
        {
            switchGunUi.SetActive(false);
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

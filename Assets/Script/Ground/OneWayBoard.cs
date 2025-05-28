using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayBoard : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D platformCollider;

    private bool canDown = false;

    void Update()
    {
        if (canDown && Input.GetKeyDown(KeyCode.S))
        {
            platformCollider.isTrigger = true;
            canDown = false;
            StartCoroutine(ResetCollider());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canDown = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canDown = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canDown = false;
        }
    }

    IEnumerator ResetCollider()
    {
        yield return new WaitForSeconds(0.5f);
        platformCollider.isTrigger = false;
    }
}

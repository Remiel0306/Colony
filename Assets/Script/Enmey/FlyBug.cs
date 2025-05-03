using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBug : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    public bool isTouchGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isTouchGround = Physics2D.OverlapBox(transform.position, new Vector2(0.64f, 0.64f), 0f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("touch");

        if (collision.gameObject.CompareTag("Ground"))
        {
            isTouchGround = true;
        }
    }
}

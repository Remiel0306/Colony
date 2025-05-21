using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBug : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    private Vector3 originPosition;
    private bool isDead = false;
    public bool isTouchGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        originPosition = transform.position; // �O�U�_�l��m
    }

    // Update is called once per frame
    void Update()
    {
        isTouchGround = Physics2D.OverlapBox(transform.position, new Vector2(0.64f, 0.64f), 0f, groundLayer);
    }

    public void ResetBug()
    {
        transform.position = originPosition;
        isDead = false;
        gameObject.SetActive(true); // �T�O�ۤv���ҥ�
        // �o�̸ɤW�A�����A���]
        // �Ҧp currentState = FlyBugState.Stay;
        //     canAttack = true;
        //     animator.Play("Idle");
    }
    public void KillBug()
    {
        isDead = true;
        gameObject.SetActive(false); // ���`�ɦۦ�����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("General Bullet Stats")]
    [SerializeField] private LayerMask whatDestoryBullet;
    [SerializeField] private float destoryTime = .5f;

    [Header("Normal Bullet Stats")]
    [SerializeField] private float normalBulletSpeed = 25f;
    [SerializeField] private float normalBulletDamage = 1f;

    [Header("Physics Bullet Stats")]
    [SerializeField] private float physicsBulletSpeed = 17.5f;
    [SerializeField] private float physicsBulletGravity = 3f;
    [SerializeField] private float physicsBulletDamage = 2f;

    [Header("Shotgun Bullet Stats")]
    [SerializeField] private float shotgunBulletSpeed = 35f;
    [SerializeField] private float shotgunBulletDamage = .1f;
    [SerializeField] private float shotgunBulletDestoryTime = .2f;

    [SerializeField] GameObject impacEffect;
    [SerializeField] Animator impacAnimation;

    private Rigidbody2D rb2D;
    private float damage;

    public enum BulletType
    {
        Normal,
        Physics,
        shotgun
    }
    public BulletType bulletType;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        SetDestoryTime();
        //SetStraightVelocity();

        //change rb based on bullet type
        SetRB2DStats();

        //set velocity based on bullet type
        InitializeBulletState();
    }

    private void FixedUpdate()
    {
        //rotate bullet in dircetion of velocity
        transform.right = rb2D.velocity;
    }

    private void InitializeBulletState()
    {
        if(bulletType == BulletType.Normal)
        {
            SetStraightVelocity();
            damage = normalBulletDamage;
        }
        else if(bulletType == BulletType.Physics)
        {
            SetPhysiceVelocity();
            damage = physicsBulletDamage;
        }
        else if(bulletType == BulletType.shotgun)
        {
            SetShotgunPhysiceVelocity();
            SetShotgunDestoryTime();
            damage = shotgunBulletDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //is the collision within the whatDestoryBullet layerMask
        if((whatDestoryBullet.value &(1 << collision.gameObject.layer)) > 0)
        {
            //spwn particles

            //play sound FX

            //screen shake

            //damage enmey
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if(iDamageable != null)
            {
                iDamageable.Damage(damage);
            }

            //destroy bullet
            GameObject effect = Instantiate(impacEffect, transform.position, transform.rotation);
            Animator impacAnimation = effect.GetComponent<Animator>();
            Destroy(effect, impacAnimation.GetCurrentAnimatorStateInfo(0).length);
            //StartCoroutine(DestroyAfterAnimation(impacEffect, impacAnimation));
            Destroy(gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb2D.velocity = transform.right * normalBulletSpeed;
    }

    private void SetPhysiceVelocity()
    {
        rb2D.velocity = transform.right * physicsBulletSpeed;
    }

    private void SetShotgunPhysiceVelocity()
    {
        rb2D.velocity = transform.right * shotgunBulletSpeed;
    }

    private void SetDestoryTime()
    {
        Destroy(gameObject, destoryTime);
    }

    private void SetShotgunDestoryTime()
    {
        Destroy(gameObject, shotgunBulletDestoryTime);
    }

    private void SetRB2DStats()
    {
        if(bulletType == BulletType.Normal)
        {
            rb2D.gravityScale = 0f;
        }

        else if(bulletType == BulletType.Physics)
        {
            rb2D.gravityScale = physicsBulletGravity;
        }
    }

    private IEnumerator DestroyAfterAnimation(GameObject impact, Animator animator)
    {
        // 等一幀，讓 Animator 進入第一個動畫狀態
        yield return null;

        // 獲取動畫狀態長度
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // 等待動畫播放結束
        yield return new WaitForSeconds(animationLength);

        // 銷毀動畫物件
        Destroy(impacEffect);
    }
}

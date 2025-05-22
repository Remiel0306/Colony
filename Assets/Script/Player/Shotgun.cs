using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public enum ShotgunState
{
    Gun1,
    Gun1To2, // 變色
    Gun2,    //散彈
    Gun2To1
}

public class Shotgun : MonoBehaviour
{
    [SerializeField] AimAndShoot aimAndShoot;
    [SerializeField] GameObject gun;
    [SerializeField] SpriteRenderer gunSR;
    [SerializeField] Animator shotgunAnimator;
    [SerializeField] public bool isShotgun;

    ShotgunState currentShotgunState = ShotgunState.Gun1;

    public bool nowCanSwitch = false;
    public bool shotgunCanShoot = false;
    public bool canShoot = true;
    bool isSwitching = false;
    bool isChanging = false;

    int gunChangeCounter = 0;
    SpriteRenderer sr;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (nowCanSwitch)
            {
                SwitchShotgunState();
            }
        }

        //if (shotgunCanShoot && aimAndShoot.shotgunIsShoot)
        //{
        //    SwitchGun2To1();
        //    aimAndShoot.shotgunIsShoot = false;
        //}

        if (currentShotgunState == ShotgunState.Gun1To2 || currentShotgunState == ShotgunState.Gun2)
        {
            shotgunAnimator.SetBool("isShotgun", true);
        }
        else
        {
            shotgunAnimator.SetBool("isShotgun", false);
        }
    }

    void SwitchShotgunState()
    {
        if (isSwitching) return; // 若正在變色，則不允許切換

        switch (currentShotgunState)
        {
            case ShotgunState.Gun1:
                canShoot = true;
                currentShotgunState = ShotgunState.Gun1To2;
                isSwitching = true;
                StartCoroutine(ChangeButterTime());
                gunSR.DOColor(new Color(1f, 0.8f, 1f), .2f).OnComplete(() =>
                {
                    currentShotgunState = ShotgunState.Gun2;
                    isSwitching = false;
                    shotgunCanShoot = true;
                });
                isShotgun = true;
                break;
            case ShotgunState.Gun2:
                canShoot = true;
                currentShotgunState = ShotgunState.Gun2To1;
                isSwitching = true;
                shotgunCanShoot = false;
                gunSR.DOColor(new Color(1f, 1f, 1f), .2f).OnComplete(() =>
                {
                    currentShotgunState = ShotgunState.Gun1;
                    isSwitching = false;
                });
                isShotgun = false;
                break;
        }
    }

    IEnumerator ChangeButterTime()
    {
        canShoot = false;
        yield return new WaitForSeconds(.5f);
        canShoot = true;
    }
}
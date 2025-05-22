using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.VisualScripting;

public class AimAndShoot : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Shotgun shotgunScript;
    [SerializeField] Enemy enemy;
    [SerializeField] FlyEnemy flyEnemy;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shotgunBullet;
    [SerializeField] GameObject fireLight;
    [SerializeField] GameObject shotgunFireLight;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    [SerializeField] Transform bulletSpwanPoint;
    [SerializeField] public float recoilForce = 10;
    [SerializeField] public float orginalRecoil;
    [SerializeField] private ScreenShakeProfile profile_Rifle;
    [SerializeField] private ScreenShakeProfile priflie_Shotgun;
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] Vector2 hotspot = Vector2.zero;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] float aimZoomSize = 4.2f;
    [SerializeField] float defaultZoomSize = 4.7f;
    [SerializeField] float zoomSmoothSpeed = 10f;

    // 💡 新增：Cinemachine Offset 相關
    CinemachineFramingTransposer framingTransposer;
    Vector3 defaultOffset;
    [SerializeField] float aimOffsetStrength = 0.5f; // 偏移強度，可調整
    [SerializeField] float aimOffsetSmooth = 5f;     // 平滑過渡速度


    PlayerControl playerControl;
    CinemachineImpulseSource impulseSource;

    GameObject bulletInst;
    Vector2 worldPosition;
    Vector2 direction;
    float angle;

    public Vector2 aimDir { get; private set; }

    public bool canMove = true;
    public bool shotgunIsShoot = false;
    public bool isAim = false;
    public bool isKillBoomBug = false;
    public bool isKillFlyBug = false;
    public bool aimFlip = false;
    public int maxBulletCount = 15;
    public int currentBulletCount;

    bool isFire = false;
    bool isShotgunFire = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        orginalRecoil = recoilForce;

        // 💡 初始化 Cinemachine 組件
        framingTransposer = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer != null)
        {
            defaultOffset = framingTransposer.m_TrackedObjectOffset;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAim = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isAim = false;
        }

        if (isAim)
        {
            aimFlip = true;
            if (shotgunScript.isShotgun && shotgunScript.shotgunCanShoot)
            {
                //cinemachine.m_Lens.OrthographicSize =4.2f;

                HandleGunRotation();
                if (shotgunScript.canShoot)
                {
                    ShotgunShoot();
                }
            }
            else
            {
                // cinemachine.m_Lens.OrthographicSize = 4.2f;

                HandleGunRotation();
                if (shotgunScript.canShoot)
                {
                    HandleGunShooting();
                }
            }
        }
        else
        {
            aimFlip = false;
        }

        if (isFire)
        {
            if (playerControl.facingRight)
            {
                fireLight.transform.localEulerAngles = new Vector3(0f, 0f, -90f); // 朝右
            }
            else
            {
                fireLight.transform.localEulerAngles = new Vector3(0f, 180f, -90f); // 朝左（Y 翻轉）
            }

            fireLight.SetActive(true);
            StartCoroutine(CloseFireLight());
        }
        if (isShotgunFire)
        {
            if (playerControl.facingRight)
            {
                shotgunFireLight.transform.localEulerAngles = new Vector3(0f, 0f, -90f); // 朝右
            }
            else
            {
                shotgunFireLight.transform.localEulerAngles = new Vector3(0f, 180f, -90f); // 朝左（Y 翻轉）
            }
            shotgunFireLight.SetActive(true);
            StartCoroutine(CloseFireLight());
        }

        // Aim zoom in/out
        float targetZoom = isAim ? aimZoomSize : defaultZoomSize;
        cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(
            cinemachine.m_Lens.OrthographicSize,
            targetZoom,
            Time.deltaTime * zoomSmoothSpeed
        );

        if (isKillBoomBug && !enemy.isDied)
        {
            if (playerManager.currentHealth < playerManager.maxHealth)
            {
                playerManager.currentHealth += 1;
            }

            isKillBoomBug = false;
            Debug.Log("isKillBoomBug");
        }

        if (isKillFlyBug && !flyEnemy.isFlyBugDied)
        {
            if (playerManager.currentHealth < playerManager.maxHealth)
            {
                playerManager.currentHealth += 1;
            }

            int ammoToAdd = Mathf.Min(7, maxBulletCount - currentBulletCount);
            currentBulletCount += ammoToAdd;

            isKillFlyBug = false;
            Debug.Log("isKillFlyBug");
        }
        // 💡 滑鼠瞄準時讓鏡頭稍微偏移
        // 💡 滑鼠瞄準時讓鏡頭稍微偏移，支援左右翻轉
        if (framingTransposer != null)
        {
            if (isAim)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector3 playerPos = transform.position;

                Vector3 aimDir = mouseWorldPos - playerPos;
                aimDir.z = 0f;

                // 💡 根據角色面向來翻轉方向（否則會顛倒）
                float flipX = playerControl.facingRight ? 1f : -1f;
                Vector3 flippedDir = new Vector3(aimDir.x * flipX, aimDir.y, 0f);

                Vector3 clampedDir = Vector3.ClampMagnitude(flippedDir, 1f);

                Vector3 targetOffset = defaultOffset + clampedDir * aimOffsetStrength;

                framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                    framingTransposer.m_TrackedObjectOffset,
                    targetOffset,
                    Time.deltaTime * aimOffsetSmooth
                );
            }
            else
            {
                framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                    framingTransposer.m_TrackedObjectOffset,
                    defaultOffset,
                    Time.deltaTime * aimOffsetSmooth
                );
            }
        }
    }

    private void HandleGunRotation()
    {
        // Rotate the gun towards the mouse position
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;

        // Adjust the local scale based on the angle
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f; // Flip the gun vertically
            if (playerControl.facingRight)
            {
                playerControl.Flip();
            }
        }
        else
        {
            localScale.y = 1f;
            if (!playerControl.facingRight)
            {
                playerControl.Flip();
            }
        }

        gun.transform.localScale = localScale;
    }

    private void HandleGunShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            bulletInst = Instantiate(bullet, bulletSpwanPoint.position, gun.transform.rotation);
            //CameraShakeManager.instance.CameraShake(impulseSource);
            CameraShakeManager.instance.ScreenShakeFromProfle(profile_Rifle, impulseSource);
            audioManager.PlaySFX(audioManager.laserSound);
            audioManager.PlaySFX(audioManager.rifleShotSound);

            isFire = true;
            currentBulletCount--;
        }
    }

    private void ShotgunShoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            float spreadAngle = 45f;
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (8 - 1);

            isShotgunFire = true;

            for (int i = 0; i < 9; i++)
            {
                float bulletAngle = startAngle + (angleStep * i);
                Quaternion bulletRotation = gun.transform.rotation * Quaternion.Euler(0, 0, bulletAngle);
                Instantiate(shotgunBullet, bulletSpwanPoint.transform.position, bulletRotation);
            }
            audioManager.PlayShotgunSFX(audioManager.shotgunSound);
            audioManager.PlayShotgunSFX(audioManager.laserShotgunSound);

            shotgunIsShoot = true;
            canMove = false;
            ShutgunRecoil();
            CameraShakeManager.instance.ScreenShakeFromProfle(priflie_Shotgun, impulseSource);
        }
    }

    private void ShutgunRecoil()
    {
        Vector2 recoilDirction = -gun.transform.right;
        playerControl.rb2D.AddForce(recoilDirction * recoilForce, ForceMode2D.Impulse);
    }

    IEnumerator CloseFireLight()
    {
        yield return new WaitForSeconds(.05f);
        fireLight.SetActive(false);
        shotgunFireLight.SetActive(false);
        isFire = false;
        isShotgunFire = false;
    }
}
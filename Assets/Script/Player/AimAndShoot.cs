using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AimAndShoot : MonoBehaviour
{
    [SerializeField] Shotgun shotgun;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shotgunBullet;
    [SerializeField] Transform bulletSpwanPoint;
    [SerializeField] float recoilForce;
    [SerializeField] private ScreenShakeProfile profile_Rifle;
    [SerializeField] private ScreenShakeProfile priflie_Shotgun;

    PlayerControl playerControl;
    CinemachineImpulseSource impulseSource;

    private GameObject bulletInst;
    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;

    public bool shotgunIsShoot = false;
    bool isAim = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
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
            if (shotgun.isShotgun && shotgun.shotgunCanShoot)
            {
                HandleGunRotation();
                ShotgunShoot();
            }
            else
            {
                HandleGunRotation();
                HandleGunShooting();
            }
        }
        else
        {
            if (playerControl.facingRight)
            {
                gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0, 0, 0), 10f * Time.deltaTime);
            }
            else
            {
                gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0, 0, 180), 10f * Time.deltaTime);
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

        // Limit the angle based on the player's facing direction
        if (playerControl.facingRight)
        {
            angle = Mathf.Clamp(angle, -89f, 89f); // Limit to right side
        }
        else
        {
            angle = Mathf.Clamp(angle, 91f, 290f); // Limit to left side  ////!! I can try this, but let's me use other way to fix the rotate limite
        }

        // Apply the rotation
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f; // Flip the gun vertically
        }
        else
        {
            localScale.y = 1f;
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
        }
    }
    
    private void ShotgunShoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            float spreadAngle = 45f;
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (8 - 1);

            for (int i = 0; i < 9; i++)
            {
                float bulletAngle = startAngle + (angleStep * i);
                Quaternion bulletRotation = gun.transform.rotation * Quaternion.Euler(0, 0, bulletAngle);
                Instantiate(shotgunBullet, bulletSpwanPoint.transform.position, bulletRotation);
            }

            shotgunIsShoot = true;
            ShutgunRecoil();
            CameraShakeManager.instance.ScreenShakeFromProfle(priflie_Shotgun, impulseSource);
        }
    }

    private void ShutgunRecoil()
    {
        Vector2 recoilDirction = -gun.transform.right;
        playerControl.rb2D.AddForce(recoilDirction * recoilForce, ForceMode2D.Impulse);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowObj : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipRotationTime = 0.5f;

    private Coroutine turnCoroutine;
    private PlayerControl player;
    private bool isFacingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<PlayerControl>();

        isFacingRight = player.facingRight;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}

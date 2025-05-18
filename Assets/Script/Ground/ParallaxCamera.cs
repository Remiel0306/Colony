using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(Vector2 deltaMovement); // 修改：傳遞 Vector2 包含 x 和 y 的移動量
    public ParallaxCameraDelegate onCameraTranslate;

    private Vector3 oldPosition; // 修改：紀錄完整的 3D 位置

    void Start()
    {
        oldPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != oldPosition)
        {
            if (onCameraTranslate != null)
            {
                // 計算 x 和 y 軸的移動量
                Vector2 delta = new Vector2(oldPosition.x - transform.position.x, oldPosition.y - transform.position.y);
                onCameraTranslate(delta);
            }

            oldPosition = transform.position;
        }
    }
}
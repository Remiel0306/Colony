using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // 修改：使用 Vector2 來分別控制 x 和 y 軸的視差係數
    public Vector2 parallaxFactor;

    public void Move(Vector2 delta)
    {
        Vector3 newPos = transform.localPosition;

        // 根據 parallaxFactor 的 x 和 y 分量調整位置
        newPos.x -= delta.x * parallaxFactor.x;
        newPos.y -= delta.y * parallaxFactor.y;

        transform.localPosition = newPos;
    }
}
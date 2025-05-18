using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // �ק�G�ϥ� Vector2 �Ӥ��O���� x �M y �b�����t�Y��
    public Vector2 parallaxFactor;

    public void Move(Vector2 delta)
    {
        Vector3 newPos = transform.localPosition;

        // �ھ� parallaxFactor �� x �M y ���q�վ��m
        newPos.x -= delta.x * parallaxFactor.x;
        newPos.y -= delta.y * parallaxFactor.y;

        transform.localPosition = newPos;
    }
}
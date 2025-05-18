using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(Vector2 deltaMovement); // �ק�G�ǻ� Vector2 �]�t x �M y �����ʶq
    public ParallaxCameraDelegate onCameraTranslate;

    private Vector3 oldPosition; // �ק�G�������㪺 3D ��m

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
                // �p�� x �M y �b�����ʶq
                Vector2 delta = new Vector2(oldPosition.x - transform.position.x, oldPosition.y - transform.position.y);
                onCameraTranslate(delta);
            }

            oldPosition = transform.position;
        }
    }
}
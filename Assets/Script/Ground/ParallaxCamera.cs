using UnityEngine;

// [ExecuteInEditMode] �����}���b�s�边�Ҧ��U�]����A�i�H�b Scene �������w����v�����ʱa�Ӫ��ĪG
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    // �w�q�@�� delegate�A�ΨӶǻ���v�����ʪ��Z���]deltaMovement�^
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    // ����v�����ʮɡA�o�� delegate �|�q���q�\�̡]�Ҧp�I���h�^�i��������첾
    public ParallaxCameraDelegate onCameraTranslate;

    // �����W�@����v���b x �b����m�A�Ψӭp�Ⲿ�ʶZ��
    private float oldPosition;

    // Start() �b�C���}�l�ɩI�s
    void Start()
    {
        // ��l�� oldPosition ����v����e�� x �b��m
        oldPosition = transform.position.x;
    }

    // Update() �C�V�I�s
    void Update()
    {
        // �ˬd��v���� x �b��m�O�_���ܤ�
        if (transform.position.x != oldPosition)
        {
            // �p�G��v�����ʤF�A�B onCameraTranslate ���q�\��
            if (onCameraTranslate != null)
            {
                // �p����v�����ʪ��Z���t delta
                // �`�N�G�o�̥� oldPosition - transform.position.x�A�i�H�ھڧA�Ʊ�I�����ʪ���V�վ�
                float delta = oldPosition - transform.position.x;
                // �z�L delegate �N���ʶZ���ǻ��X�h�A�I���h�|�ھڳo�ӭȶi�業��
                onCameraTranslate(delta);
            }

            // ��s oldPosition ���ثe����m�A�ǳƤU�@���p��
            oldPosition = transform.position.x;
        }
    }
}

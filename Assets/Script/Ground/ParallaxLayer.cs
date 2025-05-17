using UnityEngine;

// [ExecuteInEditMode] ���Ӹ}���b�s�边�D����Ҧ��U�]�|����A��K�b Scene �������w���ĪG
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // parallaxFactor ��ܸӭI���h���ʪ���ҫY��
    // �ƭȶV�j�A�Ӽh�۹����v�����ʱo�V�֡F�ƭȸ��p�h���ʸ��C�A���͵��t�ĪG
    public float parallaxFactor;

    /// <summary>
    /// �ھڶǤJ�� delta �Ƚվ�I���h��������m
    /// </summary>
    /// <param name="delta">��v���������ʪ��Z���t</param>
    public void Move(float delta)
    {
        // ���o��e���a��m
        Vector3 newPos = transform.localPosition;

        // �ھ� parallaxFactor �վ� x �b��m
        // ��h delta * parallaxFactor �ϱo�I���h�V�ۤϤ�V�����A�F����t�ĪG
        newPos.x -= delta * parallaxFactor;

        // ��s�I���h�����a��m
        transform.localPosition = newPos;
    }
}
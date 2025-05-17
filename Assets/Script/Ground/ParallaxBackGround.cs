using System.Collections.Generic;
using UnityEngine;

// �o���ݩ����}���Y�Ϧb�s�边�Ҧ��U�]�����]�Ҧp�b Scene �������w���ĪG�^
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    // �ѦҤ@�� ParallaxCamera �ե�A�Ӳե�t�d�V�I���o�e��v�����ʪ��^�հT��
    public ParallaxCamera parallaxCamera;

    // �x�s�Ҧ��l����W�� ParallaxLayer �ե�A�Ψӱ���U�I���h������
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    // Start ��k�b�}���}�l�B��ɳQ�I�s
    void Start()
    {
        // �p�G�S���b Inspector ����ʫ��w parallaxCamera�A
        // �h���ձq�D��v���W���o ParallaxCamera �ե�
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // �p�G���\���o parallaxCamera�A�N�q�\���� onCameraTranslate �ƥ�A
        // ����v�����ʮɴN�|�I�s Move() ��k�ӧ�s�I�����첾
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // �I�s SetLayers() ��k�A�N�Ҧ��l����W�� ParallaxLayer �ե�[�J��C��
        SetLayers();
    }

    // �o�Ӥ�k�t�d�j�M��e GameObject ���Ҧ��l����A
    // �ñq�����o ParallaxLayer �ե�A�M���x�s�b parallaxLayers �C��
    void SetLayers()
    {
        // �M�ŭ즳���C��A����ƲK�[
        parallaxLayers.Clear();

        // ���N�Ҧ��l����
        for (int i = 0; i < transform.childCount; i++)
        {
            // ���ձq�l������o ParallaxLayer �ե�
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // �p�G�l���� ParallaxLayer �ե�
            if (layer != null)
            {
                // �N�Ӥl���󭫷s�R�W�� "Layer-i"�]�Ҧp Layer-0�BLayer-1 ���^�A�K�����
                layer.name = "Layer-" + i;
                // �N�ӭI���h�[�J�C��
                parallaxLayers.Add(layer);
            }
        }
    }

    // ����v�����ʮɡACameraManager �|�z�L�ƥ�ǤJ�@�� delta �ȡA
    // �o�Ӥ�k�|���Ҧ��� ParallaxLayer �ھڳo�� delta �ȶi�沾�ʡA
    // �H�F����t�ĪG
    void Move(float delta)
    {
        // �M���C�@�ӭI���h�A�I�s���̪� Move() ��k
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}

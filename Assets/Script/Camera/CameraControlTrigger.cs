//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;
//using UnityEditor;
//using Unity.VisualScripting; // �Ω�ۭq Inspector

//// �o�Ӹ}�����b�������� GameObject �W�A�Ψ�Ĳ�o�P������v���������\��
//public class CameraControlTrigger : MonoBehaviour
//{
//    public CustomInspectorObject customInspectorObject; // �x�s�ۭq���˵����]�w�ѼơA��K�b Inspector ���]�w
//    private Collider2D coll2D;

//    private void Start()
//    {
//        coll2D = GetComponent<Collider2D>();
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {        
//        if (collision.CompareTag("Player"))
//        {
//            if (customInspectorObject.panCameraOnContact)
//            {
//                //pan the camera
//                CameraManager.instance.PanCameraOnContact(customInspectorObject.panDistance, customInspectorObject.panTime, customInspectorObject.panDirection, false);
//            }
//        }
//    }
//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            Vector2 exitDirection = (collision.transform.position - coll2D.bounds.center).normalized;

//            if (customInspectorObject.swapCamera && customInspectorObject.cameraOnLeft != null && customInspectorObject.cameraOnRight != null)
//            {
//                //swap cameras
//                CameraManager.instance.SwapCamera(customInspectorObject.cameraOnLeft, customInspectorObject.cameraOnRight, exitDirection);
//            }

//            if (customInspectorObject.panCameraOnContact)
//            {
//                //pan the camera
//                CameraManager.instance.PanCameraOnContact(customInspectorObject.panDistance, customInspectorObject.panTime, customInspectorObject.panDirection, true);
//            }
//        }
//    }
//}


//[System.Serializable]   // �ۭq��������O�A�Ω��x�s��v����������Ѽ�
//public class CustomInspectorObject
//{    
//    public bool swapCamera = false;             // �O�_�ҥ���v�������\��   
//    public bool panCameraOnContact = false;     // �O�_�ҥα�Ĳ����v�������\��
        
//    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;     // ��ҥ���v�������\��ɡA�o������|��ܩ� Inspector �ѳ]�w���k��v��
//    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;    // �o�����w�]���áA�u���b swapCamera �� true �ɤ~�|���
    
//    [HideInInspector] public PanDirection panDirection;     // ��ҥΥ����\��ɡA�H�U�ѼƱN�������V�B�Z���P����ɶ�
//    [HideInInspector] public float panDistance = 3f;
//    [HideInInspector] public float panTime = 0.35f;
//}

//public enum PanDirection    // �T�|�A�Ω�w�q������V
//{
//    Up,
//    Down,
//    Left,
//    Right
//}

//[CustomEditor(typeof(CameraControlTrigger))]    // �ۭq Inspector �s�边�G�Ψөw�� CameraControlTrigger �b Inspector ������ܤ覡
//public class MyScriptEditor : Editor
//{
//    CameraControlTrigger cameraControlTrigger;  // �x�s�ؼЪ���ޥ�
//    private void OnEnable()     // ��s�边�ҥήɡA���o�ؼЪ���ޥ�
//    {
//        cameraControlTrigger = (CameraControlTrigger)target;
//    }
//    public override void OnInspectorGUI()   // ���g Inspector ��ø�s��k
//    {        
//        DrawDefaultInspector();     // �e�X�w�]�� Inspector ���e�]�]�A public ���^
                
//        if (cameraControlTrigger.customInspectorObject.swapCamera)  // �p�G�ҥΤF��v�������]swapCamera�^�\��A�h��ܥ��k��v�����
//        {
//            // �o����ܥ�����v�������
//            cameraControlTrigger.customInspectorObject.cameraOnLeft = EditorGUILayout.ObjectField(
//                "Camera On Left",
//                cameraControlTrigger.customInspectorObject.cameraOnLeft,
//                typeof(CinemachineVirtualCamera),
//                true) as CinemachineVirtualCamera;

//            // �o����ܥk����v�������]�`�N Label ���� "Camera On Right"�^
//            cameraControlTrigger.customInspectorObject.cameraOnRight = EditorGUILayout.ObjectField(
//                "Camera On Right",
//                cameraControlTrigger.customInspectorObject.cameraOnRight,
//                typeof(CinemachineVirtualCamera),
//                true) as CinemachineVirtualCamera;
//        }
                
//        if (cameraControlTrigger.customInspectorObject.panCameraOnContact)  // �p�G�ҥΤF�����\��]panCameraOnContact�^�A�h��ܥ����������
//        {
//            // ��ܥ�����V�U�Կ��
//            cameraControlTrigger.customInspectorObject.panDirection = (PanDirection)EditorGUILayout.EnumPopup(
//                "Camera Pan Direction",
//                cameraControlTrigger.customInspectorObject.panDirection);

//            // ��ܥ����Z�����
//            cameraControlTrigger.customInspectorObject.panDistance = EditorGUILayout.FloatField(
//                "Pan Distance",
//                cameraControlTrigger.customInspectorObject.panDistance);

//            // ��ܥ����ɶ����
//            cameraControlTrigger.customInspectorObject.panTime = EditorGUILayout.FloatField(
//                "Pan Time",
//                cameraControlTrigger.customInspectorObject.panTime);
//        }
                
//        if (GUI.changed)    // �p�G Inspector ������ʡA�аO�ؼЪ��󬰤w�ק�
//        {
//            EditorUtility.SetDirty(cameraControlTrigger);
//        }
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;
//using UnityEditor;
//using Unity.VisualScripting; // 用於自訂 Inspector

//// 這個腳本掛在場景中的 GameObject 上，用來觸發與控制攝影機相關的功能
//public class CameraControlTrigger : MonoBehaviour
//{
//    public CustomInspectorObject customInspectorObject; // 儲存自訂的檢視器設定參數，方便在 Inspector 中設定
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


//[System.Serializable]   // 自訂的資料類別，用於儲存攝影機控制相關參數
//public class CustomInspectorObject
//{    
//    public bool swapCamera = false;             // 是否啟用攝影機切換功能   
//    public bool panCameraOnContact = false;     // 是否啟用接觸時攝影機平移功能
        
//    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;     // 當啟用攝影機切換功能時，這兩個欄位會顯示於 Inspector 供設定左右攝影機
//    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;    // 這些欄位預設隱藏，只有在 swapCamera 為 true 時才會顯示
    
//    [HideInInspector] public PanDirection panDirection;     // 當啟用平移功能時，以下參數將控制平移方向、距離與持續時間
//    [HideInInspector] public float panDistance = 3f;
//    [HideInInspector] public float panTime = 0.35f;
//}

//public enum PanDirection    // 枚舉，用於定義平移方向
//{
//    Up,
//    Down,
//    Left,
//    Right
//}

//[CustomEditor(typeof(CameraControlTrigger))]    // 自訂 Inspector 編輯器：用來定制 CameraControlTrigger 在 Inspector 中的顯示方式
//public class MyScriptEditor : Editor
//{
//    CameraControlTrigger cameraControlTrigger;  // 儲存目標物件引用
//    private void OnEnable()     // 當編輯器啟用時，取得目標物件引用
//    {
//        cameraControlTrigger = (CameraControlTrigger)target;
//    }
//    public override void OnInspectorGUI()   // 重寫 Inspector 的繪製方法
//    {        
//        DrawDefaultInspector();     // 畫出預設的 Inspector 內容（包括 public 欄位）
                
//        if (cameraControlTrigger.customInspectorObject.swapCamera)  // 如果啟用了攝影機切換（swapCamera）功能，則顯示左右攝影機欄位
//        {
//            // 這裡顯示左側攝影機的欄位
//            cameraControlTrigger.customInspectorObject.cameraOnLeft = EditorGUILayout.ObjectField(
//                "Camera On Left",
//                cameraControlTrigger.customInspectorObject.cameraOnLeft,
//                typeof(CinemachineVirtualCamera),
//                true) as CinemachineVirtualCamera;

//            // 這裡顯示右側攝影機的欄位（注意 Label 應為 "Camera On Right"）
//            cameraControlTrigger.customInspectorObject.cameraOnRight = EditorGUILayout.ObjectField(
//                "Camera On Right",
//                cameraControlTrigger.customInspectorObject.cameraOnRight,
//                typeof(CinemachineVirtualCamera),
//                true) as CinemachineVirtualCamera;
//        }
                
//        if (cameraControlTrigger.customInspectorObject.panCameraOnContact)  // 如果啟用了平移功能（panCameraOnContact），則顯示平移相關欄位
//        {
//            // 顯示平移方向下拉選單
//            cameraControlTrigger.customInspectorObject.panDirection = (PanDirection)EditorGUILayout.EnumPopup(
//                "Camera Pan Direction",
//                cameraControlTrigger.customInspectorObject.panDirection);

//            // 顯示平移距離欄位
//            cameraControlTrigger.customInspectorObject.panDistance = EditorGUILayout.FloatField(
//                "Pan Distance",
//                cameraControlTrigger.customInspectorObject.panDistance);

//            // 顯示平移時間欄位
//            cameraControlTrigger.customInspectorObject.panTime = EditorGUILayout.FloatField(
//                "Pan Time",
//                cameraControlTrigger.customInspectorObject.panTime);
//        }
                
//        if (GUI.changed)    // 如果 Inspector 中有改動，標記目標物件為已修改
//        {
//            EditorUtility.SetDirty(cameraControlTrigger);
//        }
//    }
//}

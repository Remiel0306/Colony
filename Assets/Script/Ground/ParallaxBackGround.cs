using System.Collections.Generic;
using UnityEngine;

// 這個屬性讓腳本即使在編輯器模式下也能執行（例如在 Scene 視窗中預覽效果）
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    // 參考一個 ParallaxCamera 組件，該組件負責向背景發送攝影機移動的回調訊息
    public ParallaxCamera parallaxCamera;

    // 儲存所有子物件上的 ParallaxLayer 組件，用來控制各背景層的移動
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    // Start 方法在腳本開始運行時被呼叫
    void Start()
    {
        // 如果沒有在 Inspector 中手動指定 parallaxCamera，
        // 則嘗試從主攝影機上取得 ParallaxCamera 組件
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // 如果成功取得 parallaxCamera，就訂閱它的 onCameraTranslate 事件，
        // 當攝影機移動時就會呼叫 Move() 方法來更新背景的位移
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // 呼叫 SetLayers() 方法，將所有子物件上的 ParallaxLayer 組件加入到列表中
        SetLayers();
    }

    // 這個方法負責搜尋當前 GameObject 的所有子物件，
    // 並從中取得 ParallaxLayer 組件，然後儲存在 parallaxLayers 列表中
    void SetLayers()
    {
        // 清空原有的列表，防止重複添加
        parallaxLayers.Clear();

        // 迭代所有子物件
        for (int i = 0; i < transform.childCount; i++)
        {
            // 嘗試從子物件取得 ParallaxLayer 組件
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // 如果子物件有 ParallaxLayer 組件
            if (layer != null)
            {
                // 將該子物件重新命名為 "Layer-i"（例如 Layer-0、Layer-1 等），便於辨識
                layer.name = "Layer-" + i;
                // 將該背景層加入列表中
                parallaxLayers.Add(layer);
            }
        }
    }

    // 當攝影機移動時，CameraManager 會透過事件傳入一個 delta 值，
    // 這個方法會讓所有的 ParallaxLayer 根據這個 delta 值進行移動，
    // 以達到視差效果
    void Move(float delta)
    {
        // 遍歷每一個背景層，呼叫它們的 Move() 方法
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}

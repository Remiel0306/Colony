using UnityEngine;

// [ExecuteInEditMode] 讓此腳本在編輯器模式下也執行，可以在 Scene 視窗中預覽攝影機移動帶來的效果
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    // 定義一個 delegate，用來傳遞攝影機移動的距離（deltaMovement）
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    // 當攝影機移動時，這個 delegate 會通知訂閱者（例如背景層）進行相應的位移
    public ParallaxCameraDelegate onCameraTranslate;

    // 紀錄上一次攝影機在 x 軸的位置，用來計算移動距離
    private float oldPosition;

    // Start() 在遊戲開始時呼叫
    void Start()
    {
        // 初始化 oldPosition 為攝影機當前的 x 軸位置
        oldPosition = transform.position.x;
    }

    // Update() 每幀呼叫
    void Update()
    {
        // 檢查攝影機的 x 軸位置是否有變化
        if (transform.position.x != oldPosition)
        {
            // 如果攝影機移動了，且 onCameraTranslate 有訂閱者
            if (onCameraTranslate != null)
            {
                // 計算攝影機移動的距離差 delta
                // 注意：這裡用 oldPosition - transform.position.x，可以根據你希望背景移動的方向調整
                float delta = oldPosition - transform.position.x;
                // 透過 delegate 將移動距離傳遞出去，背景層會根據這個值進行平移
                onCameraTranslate(delta);
            }

            // 更新 oldPosition 為目前的位置，準備下一次計算
            oldPosition = transform.position.x;
        }
    }
}

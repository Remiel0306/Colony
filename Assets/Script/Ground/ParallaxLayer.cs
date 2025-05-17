using UnityEngine;

// [ExecuteInEditMode] 讓該腳本在編輯器非播放模式下也會執行，方便在 Scene 視窗中預覽效果
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // parallaxFactor 表示該背景層移動的比例係數
    // 數值越大，該層相對於攝影機移動得越快；數值較小則移動較慢，產生視差效果
    public float parallaxFactor;

    /// <summary>
    /// 根據傳入的 delta 值調整背景層的水平位置
    /// </summary>
    /// <param name="delta">攝影機水平移動的距離差</param>
    public void Move(float delta)
    {
        // 取得當前本地位置
        Vector3 newPos = transform.localPosition;

        // 根據 parallaxFactor 調整 x 軸位置
        // 減去 delta * parallaxFactor 使得背景層向相反方向平移，達到視差效果
        newPos.x -= delta * parallaxFactor;

        // 更新背景層的本地位置
        transform.localPosition = newPos;
    }
}
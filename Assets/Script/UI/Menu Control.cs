using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] GameObject designersName;

    void Start()
    {
        designersName.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Start Story");
    }

    public void DesignersText()
    {
        designersName.SetActive(true);
    }

    public void Close()
    {
        designersName.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        // 如果在 Unity 編輯器中，就停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // 如果是 build 出去的遊戲，則退出程式
    Application.Quit();
#endif
    }
}

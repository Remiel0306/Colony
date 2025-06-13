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
        // �p�G�b Unity �s�边���A�N�����
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // �p�G�O build �X�h���C���A�h�h�X�{��
    Application.Quit();
#endif
    }
}

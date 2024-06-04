using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 背景音樂
    public string nightSceneName = "Night"; // 夜晚場景的名稱

    void Start()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Play(); // 播放背景音樂
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // 檢測 Enter 鍵按下
        {
            SceneManager.LoadScene(nightSceneName); // 切換到夜晚場景
        }
    }
}

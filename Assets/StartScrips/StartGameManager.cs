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

    public void LoadNightScene()
    {
        Debug.Log("Loading night scene: " + nightSceneName);
        SceneManager.LoadScene(nightSceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 背景音乐
    public string startSceneName = "Start"; // 开始场景的名称

    void Start()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Play(); // 播放背景音乐
        }
    }

    void Update()
    {
        // 当玩家按下 Enter 键时
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 切换到开始场景
            SceneManager.LoadScene(startSceneName);
        }
    }
}

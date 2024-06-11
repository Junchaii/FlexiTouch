using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NightGameManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 背景音樂
    public string winendSceneName = "WinEnd"; // Win結束場景的名稱
    public string lossendSceneName = "LossEnd"; // Loss結束場景的名稱
    public AudioSource fireHitSound; // 火焰打击音效

    private int turtleShellCount = 3; // TurtleShellPolyart 的數量

    void Start()
    {
        // 播放背景音樂
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    // 當 TurtleShellPolyart 被銷毀時調用
    public void TurtleShellDestroyed()
    {
        turtleShellCount--;

        // 檢查是否所有 TurtleShellPolyart 都已銷毀
        if (turtleShellCount <= 0)
        {
            // 進入 Win 結束場景
            SceneManager.LoadScene(winendSceneName);
        }
    }

    // 檢查玩家生命值
    public void CheckPlayerHealth(int health)
    {
        // 如果玩家生命值小於等於 0，進入 Loss 結束場景
        if (health <= 0)
        {
            SceneManager.LoadScene(lossendSceneName);
        }
    }

    // 播放火焰打击音效
    public void PlayFireHitSound()
    {
        if (fireHitSound != null)
        {
            fireHitSound.mute = false; // 取消静音
            fireHitSound.Play();
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NightGameManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 背景音樂
    public AudioSource enterMusic; 
    public string winendSceneName = "WinEnd"; // Win結束場景的名稱
    public string lossendSceneName = "LossEnd"; // Loss結束場景的名稱
    public AudioSource fireHitSound; // 火焰打击音效
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI cdTimeText;
    public TextMeshProUGUI countdownText; // 倒计时文字
    public GameObject[] monsters; // 所有的怪物

    private int turtleShellCount = 3; // TurtleShellPolyart 的數量

    void Start()
    {
        // 播放enterMusic
        // enterMusic.Play();

        // 开始倒计时协程
        StartCoroutine(CountdownAndStartGame());
    }

    private IEnumerator CountdownAndStartGame()
    {
        // 显示倒计时
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            enterMusic.Play();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "Ready";
        yield return new WaitForSeconds(1f);

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);

        // 隐藏倒计时
        countdownText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(true);
        cdTimeText.gameObject.SetActive(true);

        // 启动所有怪物的行动
        foreach (var monster in monsters)
        {
            monster.SetActive(true);
        }

        // 等待enterMusic播放完毕
        yield return new WaitWhile(() => enterMusic.isPlaying);
        backgroundMusic.mute = false;
        // 播放backgroundMusic
        backgroundMusic.Play();
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

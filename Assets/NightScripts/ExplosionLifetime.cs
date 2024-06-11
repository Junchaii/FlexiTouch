using System.Collections;
using UnityEngine;

public class ExplosionLifetime : MonoBehaviour
{
    public float lifetime = 3f; // 特效生命周期
    public AudioSource soundProtect;

    void Start()
    {
        // 检查对象标签是否为 "Protect"
        if (gameObject.CompareTag("Protect"))
        {
            if (soundProtect != null)
            {
                soundProtect.mute = false; // 取消静音
                soundProtect.Play(); // 播放音效
            }
        }

        // 启动协程来延迟销毁特效
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        // 等待一段时间后销毁特效
        yield return new WaitForSeconds(lifetime);

        // 在对象销毁前停止音效播放
        if (soundProtect != null)
        {
            soundProtect.Stop();
        }

        Destroy(gameObject);
    }
}

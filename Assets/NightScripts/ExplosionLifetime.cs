using System.Collections;
using UnityEngine;

public class ExplosionLifetime : MonoBehaviour
{
    public float lifetime = 3f; // 特效生命周期

    void Start()
    {
        // 启动协程来延迟销毁特效
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        // 等待一段时间后销毁特效
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}

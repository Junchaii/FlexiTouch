using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float Speed = 1f;
    public bool Hit = false;
    public bool Die = false; // 新增 Die 变量来控制死亡状态
    public float CurrentHP = 100; // 当前血量值
    private bool Moving = true;
    private Rigidbody rigidBody;
    private Animator animator;
    public GameObject Rock; // 投擲的石頭
    public float throwInterval = 2f; // 每隔2秒投擲一次
    private float elapsedTime = 0f; // 已過的時間
    public float pushForce = 10f; // 推力大小
    public NightGameManager nightGameManager; // 对应的 GameManager
    public float SinkSpeed = 10f; // 下沉速度

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // 使用 GetComponent 更简洁
        rigidBody = GetComponent<Rigidbody>(); // 使用 GetComponent 更简洁
        StartCoroutine(MoveAndPause());
        StartCoroutine(ThrowRock());
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving && !Hit && !Die) // 当怪物处于非受击状态且非死亡状态时才能移动
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    private IEnumerator MoveAndPause()
    {
        while (!Hit && !Die) // 当怪物处于非受击状态且非死亡状态时才会移动和暂停
        {
            // Move for 1 second
            Moving = true;
            yield return new WaitForSeconds(1f);
            
            // Pause for 3 seconds
            Moving = false;
            yield return new WaitForSeconds(3f);
        }
    }

    private IEnumerator ThrowRock()
    {
        while (!Die) // 当怪物处于非死亡状态时才能投掷石头
        {
            yield return new WaitForSeconds(throwInterval);
            Instantiate(Rock, transform.position, transform.rotation);
        }
    }

    IEnumerator DelayedHit(bool hitValue, float delay)
    {
        yield return new WaitForSeconds(delay);
        Hit = hitValue;
        animator.SetBool("Hit", Hit);
    }
    
    public void SinkOnDeath()
    {
        StartCoroutine(SinkCoroutine());
    }
    private IEnumerator SinkCoroutine()
    {
        while (transform.position.y > -50f) // 可以根据需要设定一个下沉的最低位置
        {
            // 在这里修改怪物的位置，使其Y坐标逐渐下沉
            transform.position -= Vector3.up * SinkSpeed * Time.deltaTime;

            yield return null; // 等待下一帧
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            // 受到伤害并更新血量
            CurrentHP -= 20;

            if (CurrentHP <= 0)
            {
                Die = true;
                animator.SetBool("Die", true); // 设置 Die 变量为 true 并播放死亡动画
                // 在怪物死亡时使其位置缓慢下沉
                SinkOnDeath();
                Destroy(gameObject, 6f); // 三秒后销毁怪物
                
            }
            else
            {
                Hit = true;
                animator.SetBool("Hit", Hit);

                // 延迟两秒后将 Hit 设置为 false
                StartCoroutine(DelayedHit(false, 2f));

                // 调用NightGameManager的MonsterHitByFire方法，以触发播放火焰打击音效
                if (nightGameManager != null)
                {
                    nightGameManager.MonsterHitByFire();
                }
            }
        }
    }

    // }
    void OnDestroy()
    {
        // 通知 GameManager 一个 TurtleShellPolyart 被销毁了
        if (nightGameManager != null)
        {
            nightGameManager.TurtleShellDestroyed();
        }
    }
}

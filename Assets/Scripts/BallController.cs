using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 0.1f;
    public GameObject gameObject;
    private float elapsedTime = 0.0f; // 已過的時間
   // Use this for initialization
   void Start() {

   }

   // Update is called once per frame
   void Update() {
       this.transform.Translate(new Vector3(0, 0, speed));
        
    }

    // 觸發事件
    // private void OnCollisionEnter(Collision collision)
    // {
    //     // 检查碰撞对象是否是Player
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // 如果是Player，銷毁Ball对象
    //         Destroy(gameObject);
    //     }
    // }
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否是Player
        if (other.gameObject.CompareTag("Player"))
        {
            // 如果是Player，銷毁Ball对象
            Destroy(gameObject);
        }
    }
}

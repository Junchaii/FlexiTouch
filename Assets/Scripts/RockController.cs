using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    public float speed = 0.1f;
   // Use this for initialization
   void Start() {

   }

   // Update is called once per frame
   void Update() {
       this.transform.Translate(new Vector3(0, 0, speed));
       Destroy(gameObject,3.0f);
    }
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

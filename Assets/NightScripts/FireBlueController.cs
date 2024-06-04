using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlueController : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 10.0f;
    public GameObject bluePrefab; // 撞击特效的Prefab

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            // 生成蓝色特效预制体
            GameObject explosion = Instantiate(bluePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // 销毁火球自身
        }
    }
}

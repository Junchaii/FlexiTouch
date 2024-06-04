using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    public float speed = 0.0001f;
    public GameObject explosionPrefab; // 撞击特效的Prefab

    void Update()
    {
        this.transform.Translate(new Vector3(0, 0, speed));
        Destroy(gameObject, 10.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     Destroy(gameObject);
        // }
        if (other.gameObject.CompareTag("Protect"))
        {
            // 生成特效Prefab
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
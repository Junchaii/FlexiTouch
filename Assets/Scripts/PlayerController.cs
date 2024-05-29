using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject portalPrefab; // 設置 Freeze circle 的預製體
    public Transform centerEyeAnchor; // CenterEyeAnchor 變換引用
    private GameObject currentPortal; // 跟踪當前生成的門戶

    // Start is called before the first frame update
    void Start()
    {
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor is not assigned. Please assign it in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return)) // 檢測 Enter 鍵保持按下
        {
            if (currentPortal == null) // 如果沒有生成的門戶，生成一個新的門戶
            {
                SpawnPortal();
            }

        }
        else
        {
            if (currentPortal != null) // 如果 Enter 鍵未按下且有生成的門戶，刪除該門戶
            {
                Destroy(currentPortal);
            }
        }
    }

    void SpawnPortal()
    {
        // 計算鏡頭前的位置信息
        Vector3 spawnPosition = centerEyeAnchor.position + centerEyeAnchor.forward * 0.0f; // 調整距離
        Quaternion spawnRotation = Quaternion.LookRotation(centerEyeAnchor.forward); // 設置朝向

        // 生成 Freeze circle 物件
        currentPortal = Instantiate(portalPrefab, spawnPosition, spawnRotation);
    }

    // void UpdatePortalPosition()
    // {
    //     // 更新門戶的位置和朝向
    //     currentPortal.transform.position = centerEyeAnchor.position + centerEyeAnchor.forward * 2.0f;
    //     currentPortal.transform.rotation = Quaternion.LookRotation(centerEyeAnchor.forward);
    // }
}

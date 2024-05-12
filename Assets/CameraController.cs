using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5f;

    void Update()
    {
        // 控制相機的移動
        MoveCamera();
    }

    void MoveCamera()
    {
        // 獲取按鍵輸入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 計算移動方向
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // 將方向轉換為相對於相機的方向
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0f;  // 將垂直方向設為0，只在水平方向移動

        // 移動相機
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
    }
}

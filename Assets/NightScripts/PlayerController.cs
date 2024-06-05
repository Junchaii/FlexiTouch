using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;

public class PlayerController : MonoBehaviour
{
    public GameObject portalPrefab; // Freeze circle
    public Transform centerEyeAnchor; // CenterEyeAnchor 
    public Transform positionAnchor;
    private GameObject currentPortal; // 跟蹤生成
    public GameObject fireballRed; // 火球
    public GameObject fireballBlue; // 火球
    public float fireballSpeed = 10f; // 火球的速度
    private Vector3 eyeDirection;
    public float flashDuration = 0.7f; // 閃爍時間
    public float flashIntensity = 0.1f; // 閃爍強度
    public TextMeshProUGUI healthText; // 顯示玩家生命值的 Text
    private int health = 100; // 玩家生命值
    public NightGameManager nightGameManager;
    public Image blackCoverImage; // 黑色遮罩層
    public Image bloodBlurImage;
    public TextMeshProUGUI cdTimeText; // 冷卻時間顯示的 Text

    private SerialPort serialPort; // SerialPort 變數
    public string port;
    public int baudRate = 9600;

    private bool isCooldown = false; // 冷卻狀態

    private bool returnPressedFromArduino = false;
    private bool zPressedFromArduino = false;
    private bool xPressedFromArduino = false;

    // Start is called before the first frame update
    void Start()
    {
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor is not assigned. Please assign it in the inspector.");
        }

        // 初始化 SerialPort
        serialPort = new SerialPort(port, baudRate);
        serialPort.ReadTimeout = 10000;
        serialPort.Open();

        // 初始化生命值
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        ReadFromArduino();
        eyeDirection = centerEyeAnchor.forward;

        if ((Input.GetKeyDown(KeyCode.Return) || returnPressedFromArduino) && !isCooldown)
        {
            serialPort.Write("1"); // 向Arduino發送指令
            StartCoroutine(EnterCooldown()); // 開始冷卻協程
            returnPressedFromArduino = false; // 重置狀態
            SpawnPortal();
        }

        // if ((Input.GetKey(KeyCode.Return) || returnPressedFromArduino) && !isCooldown) // 檢測 Enter 鍵保持按下且不在冷卻
        // {
        //     if (currentPortal == null) // 如果沒有生成的門戶，生成一個新的門戶
        //     {
        //         serialPort.Write("1"); // 向Arduino發送指令
        //         StartCoroutine(EnterCooldown()); // 開始冷卻協程
        //         returnPressedFromArduino = false; // 重置狀態
        //         SpawnPortal();
        //     }
        //     serialPort.Write("1"); // 向Arduino發送指令
        //     StartCoroutine(EnterCooldown()); // 開始冷卻協程
        //     returnPressedFromArduino = false; // 重置狀態
        // }
        // else
        // {
        //     if (currentPortal != null) // 如果 Enter 鍵未按下且有生成的門戶，刪除該門戶
        //     {
        //         Destroy(currentPortal);
        //     }
        // }

        if (Input.GetKeyDown(KeyCode.Z) || zPressedFromArduino) // 檢測 Z 鍵按下或 Arduino 信號
        {
            ShootFireball(KeyCode.Z);
            zPressedFromArduino = false; // 重置狀態
        }

        if (Input.GetKeyDown(KeyCode.X) || xPressedFromArduino) // 檢測 X 鍵按下或 Arduino 信號
        {
            ShootFireball(KeyCode.X);
            xPressedFromArduino = false; // 重置狀態
        }
    }

    private IEnumerator EnterCooldown()
    {
        isCooldown = true;
        float cooldownTime = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = cooldownTime - elapsedTime;
            cdTimeText.text = "Cooldown: " + remainingTime.ToString("F1") + "s";
            yield return null;
        }

        cdTimeText.text = "";
        isCooldown = false;
    }

    void SpawnPortal()
    {
        // 計算鏡頭前的位置信息
        Vector3 spawnPosition = centerEyeAnchor.position + positionAnchor.forward * 0.1f; // 調整距離
        Quaternion spawnRotation = Quaternion.LookRotation(positionAnchor.forward); // 設置朝向
        spawnPosition.y = 0;
        // 生成 Freeze circle 對象
        currentPortal = Instantiate(portalPrefab, spawnPosition, spawnRotation);
    }

    void ShootFireball(KeyCode key)
    {
        // 計算火球生成的位置和方向
        Vector3 spawnPosition
 = centerEyeAnchor.position + eyeDirection * 2f; // 調整距離
        Quaternion spawnRotation = Quaternion.LookRotation(eyeDirection); // 設置朝向

        // 生成火球
        GameObject fireball;
        if (key == KeyCode.Z)
        {
            fireball = Instantiate(fireballRed, spawnPosition, spawnRotation);
        }
        else if (key == KeyCode.X)
        {
            fireball = Instantiate(fireballBlue, spawnPosition, spawnRotation);
        }
        else
        {
            return; // 如果不是按下 Z 或 X 鍵，直接返回
        }

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = eyeDirection * fireballSpeed; // 設置火球的速度
        }
    }

    private void ReadFromArduino()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            string serialData = serialPort.ReadLine(); // 讀取串口數據
            if (serialData.Contains("Z"))
            {
                zPressedFromArduino = true; // 觸發 Z 鍵按下
            }
            else if (serialData.Contains("X"))
            {
                xPressedFromArduino = true; // 觸發 X 鍵按下
            }
            else if (serialData.Contains("Return"))
            {
                returnPressedFromArduino = true; // 觸發 Return 鍵按下
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            // 扣除玩家生命值
            health -= 20;
            if (health <= 0)
            {
                FadeToBlack();
            }
            // 更新生命值顯示
            UpdateHealthText();
            StartCoroutine(BlackScreenEffect());
        }
        else if (other.CompareTag("Monster"))
        {
            // 扣除玩家生命值
            health -= 50;
            if (health <= 0)
            {
                FadeToBlack();
            }
            // 更新生命值顯示
            UpdateHealthText();
            StartCoroutine(BlackScreenEffect());
        }
    }

    private IEnumerator BlackScreenEffect()
    {
        // 觸發黑屏效果
        blackCoverImage.color = Color.black;
        yield return new WaitForSeconds(0.1f); // 黑屏持續時間
        blackCoverImage.color = Color.clear; // 清除黑屏

        // 觸發閃爍效果
        for (int i = 0; i < 2; i++)
        {
            bloodBlurImage.color = new Color(1, 1, 1, 0.2f);
            yield return new WaitForSeconds(0.1f);
            bloodBlurImage.color = Color.clear;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FadeToBlack()
    {
        float duration = 3f; // 黑屏淡入時間
        float elapsedTime = 0f;
        Color startColor = Color.clear; // 開始顏色為完全透明
        Color endColor = Color.black; // 結束顏色為完全黑色

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // 使用 Mathf.Lerp 來獲取介於 startColor 和 endColor 之間的插值顏色
            blackCoverImage.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 當黑屏完成後，可以做一些額外的處理，比如進入特定場景或者其他操作
        // SceneManager.LoadScene("GameOverScene");
    }

    // 更新生命值顯示
    private void UpdateHealthText()
    {
        healthText.text = "HP: " + health.ToString();
        nightGameManager.CheckPlayerHealth(health);
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.Threading;
public class PracticeController : MonoBehaviour
{
    Thread threading;
    public Transform centerEyeAnchor; // CenterEyeAnchor 
    public Transform positionAnchor;
    public GameObject fireballRed; // 火球
    public GameObject fireballBlue; // 火球
    public float fireballSpeed = 10f; // 火球的速度
    private Vector3 eyeDirection;

    private SerialPort serialPort; // SerialPort 變數
    public string port;
    public int baudRate = 9600;

    private bool isCooldown = false; // 冷卻狀態

    private bool returnPressedFromArduino = false;
    private bool zPressedFromArduino = false;
    private bool xPressedFromArduino = false;

    // 添加紅色和藍色火球的音效
    public AudioSource audioSourceRed;
    public AudioSource audioSourceBlue;

    // Start is called before the first frame update
    void Start()
    {
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor is not assigned. Please assign it in the inspector.");
        }
        threading = new Thread(new ThreadStart(ReadFromArduino));
        threading.Start();
        
        // 初始化 SerialPort
        serialPort = new SerialPort(port, baudRate);
        serialPort.ReadTimeout = 100000;
        serialPort.Open();

        // 初始化音效
        
        audioSourceRed = gameObject.AddComponent<AudioSource>();
        audioSourceBlue = gameObject.AddComponent<AudioSource>();
        audioSourceBlue.playOnAwake = false; // 禁用播放
    }

    // Update is called once per frame
    void Update()
    {
        eyeDirection = centerEyeAnchor.forward;

        // 检测 Z 键按下或 Arduino 信号
        if (Input.GetKeyDown(KeyCode.Z) || zPressedFromArduino)
        {
            ShootFireball(KeyCode.Z);
            zPressedFromArduino = false; // 重置状态
        }

        // 检测 X 键按下或 Arduino 信号
        if (Input.GetKeyDown(KeyCode.X) || xPressedFromArduino) 
        {
            ShootFireball(KeyCode.X);
            xPressedFromArduino = false; // 重置状态
        }
    }

    

    void ShootFireball(KeyCode key)
    {
        // 計算火球生成的位置和方向
        Vector3 spawnPosition = centerEyeAnchor.position + eyeDirection * 2f; // 調整距離
        Quaternion spawnRotation = Quaternion.LookRotation(eyeDirection); // 設置朝向

        // 生成火球
        GameObject fireball;
        if (key == KeyCode.Z)
        {
            fireball = Instantiate(fireballRed, spawnPosition, spawnRotation);
            audioSourceRed.mute = false; // 取消静音
            audioSourceRed.Play(); // 播放紅色火球音效
        }
        else if (key == KeyCode.X)
        {
            fireball = Instantiate(fireballBlue, spawnPosition, spawnRotation);
            audioSourceBlue.mute = false; // 取消静音
            audioSourceBlue.Play(); // 播放藍色火球音效
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
        while(true)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                string serialData = serialPort.ReadLine(); // 读取串口数据
                if (serialData.Contains("Z"))
                {
                    zPressedFromArduino = true; // 触发 Z 键按下
                }
                else if (serialData.Contains("X"))
                {
                    xPressedFromArduino = true; // 触发 X 键按下
                }
                // else if (serialData.Contains("Return"))
                // {
                //     returnPressedFromArduino = true; // 触发 Return 键按下
                // }
            }
        }
    }

    // 销毁时关闭串口
    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGame : MonoBehaviour
{
    public StartGameManager startGameManager; // StartGameManager 的引用

    void Start()
    {
        if (startGameManager == null)
        {
            Debug.LogError("StartGameManager reference is not set.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with tag: " + other.gameObject.tag);
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Fire collider entered.");
            if (startGameManager != null)
            {
                startGameManager.LoadNightScene(); // 切換到夜晚場景
            }
            else
            {
                Debug.LogError("StartGameManager is not assigned in the inspector.");
            }
        }
    }
}

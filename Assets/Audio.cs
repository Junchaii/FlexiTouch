using UnityEngine;

public class Audio : MonoBehaviour
{
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 檢測按鍵
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayAudio();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            PauseAudio();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            StopAudio();
        }
    }

    public void PlayAudio()
    {
        audio.Play();
    }

    public void PauseAudio()
    {
        if (audio.isPlaying)
        {
            audio.Pause(); //音效暫停
            Time.timeScale = 0; //遊戲暫停
        }
        else
        {
            audio.UnPause();
            Time.timeScale = 1;
        }
    }

    public void StopAudio()
    {
        audio.Stop();
    }
}

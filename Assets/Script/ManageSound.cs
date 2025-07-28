using UnityEngine;

public class ManageSound : MonoBehaviour
{
    public static ManageSound Instance;

    public AudioSource sfxSource;
    

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Tự tạo AudioSource nếu chưa có
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Phát hiệu ứng âm thanh 1 lần (OneShot)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Bật/tắt âm thanh
    /// </summary>
    public void ToggleSFX(bool isOn)
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !isOn;
        }
    }
}

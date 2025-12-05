using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header(" Audio Source ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header(" Audio Clip ")]
    public AudioClip throwBall;


    float sfxCooldown = 1f;      
    float lastSFXTime = -999f;   

    public void PlaySFX(AudioClip clip)
    {
        if (Time.time - lastSFXTime >= sfxCooldown)
        {
            SFXSource.PlayOneShot(clip);
            lastSFXTime = Time.time;
        }
    }

}


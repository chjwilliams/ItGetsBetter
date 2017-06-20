using UnityEngine;

public class Pulse : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHeartbeatOne()
    {
        clip = Resources.Load<AudioClip>("Audio/SFX/Heartbeat_1");
        audioSource.PlayOneShot(clip, 1.0f);
    }

    public void PlayHeartbeatTwo()
    {
        clip = Resources.Load<AudioClip>("Audio/SFX/Heartbeat_2");
        audioSource.PlayOneShot(clip, 1.0f);
    }

}

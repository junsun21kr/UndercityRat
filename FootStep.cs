using UnityEngine;

public class FootStep : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip[] clothclips;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Step()
    {
        audioSource.volume = Random.Range(0.8f, 1f);
        AudioVolum(0.8f);
        AudioClip clip = GetRandomClip(clips);
        audioSource.PlayOneShot(clip);
    }
      
    public void Stepcloth()
    {
        audioSource.volume = Random.Range(0.3f, 0.5f);
        AudioVolum(0.4f);
        AudioClip clip = GetRandomClip(clothclips);
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip(AudioClip[] clip)
    {
        return clip[Random.Range(0, clip.Length)];
    }

    private void AudioVolum(float _pitch)
    {
        audioSource.pitch = _pitch;
    }
}

using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource m_AudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

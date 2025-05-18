using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource shotgunSFX;
    [Header("Audio Clip")]
    public AudioClip bgm;
    public AudioClip laserSound;
    public AudioClip rifleShotSound;
    public AudioClip shotgunSound;
    public AudioClip laserShotgunSound;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = bgm;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayShotgunSFX(AudioClip clip)
    {
        shotgunSFX.PlayOneShot(clip);
    }
}

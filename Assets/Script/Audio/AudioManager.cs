using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource shotgunSFX;
    [SerializeField] AudioSource hitBugSFX;
    [SerializeField] AudioSource playerSFX;
    [Header("Audio Clip")]
    public AudioClip bgm;
    public AudioClip laserSound;
    public AudioClip rifleShotSound;
    public AudioClip shotgunSound;
    public AudioClip laserShotgunSound;
    public AudioClip hitBug;
    public AudioClip playerHurt;

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

    public void SfxSource(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayShotgunSFX(AudioClip clip)
    {
        shotgunSFX.PlayOneShot(clip);
    }

    public void PlayHitBugSFX(AudioClip clip)
    {
        hitBugSFX.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip)
    {
        playerSFX.PlayOneShot(clip);
    } 
}

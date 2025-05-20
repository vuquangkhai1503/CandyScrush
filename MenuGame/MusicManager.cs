using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [Header("sound Music")]
    public AudioSource SoundGround;
    public AudioSource sfxSound;
    [Header("audio clip background")]
    public AudioClip BackGround;
    public AudioClip Loading;
    public List<AudioClip> game;
    [Header("audio clip sound")]
    public AudioClip button;
    public AudioClip swipe;
    public AudioClip desPiece;
    public AudioClip createBoom;
    public AudioClip Clear;
    public AudioClip buyItem;
    public AudioClip input;
    public AudioClip press;
    public AudioClip set;
    public AudioClip Break;
    public AudioClip win;
    public AudioClip lose;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if(clip != null)
        {
            SoundGround.clip = clip;
            SoundGround.loop = true;
            SoundGround.Play();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSound.clip = clip;
            sfxSound.Play();
        }
    }

    public void PlayMusicInGame()
    {
        int i = Random.Range(0, game.Count);
        SoundGround.clip = game[i];
        SoundGround.loop = true;
        SoundGround.Play();
    }

    public void SetValueMusicVolume(float value)
    {
        SoundGround.volume = value;
    }

    public void SetValueSoundVolume(float value)
    {
        sfxSound.volume = value;
    }
}

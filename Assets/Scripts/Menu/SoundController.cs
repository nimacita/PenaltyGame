using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Bg Music")]
    [SerializeField] private AudioSource BgMusic;

    [Header("Game Sounds")]
    [SerializeField] private AudioSource[] kickSounds;
    [SerializeField] private AudioSource[] goalInGateSounds;
    [SerializeField] private AudioSource metallGateSound;

    [Header("Addcoins")]
    [SerializeField] private AudioSource addCoins;
    [SerializeField] private AudioSource equipSound;
    [SerializeField] private AudioSource dailyRewardSound;

    [Header("Game Sounds")]
    [SerializeField] private AudioSource[] goalSounds;
    [SerializeField] private AudioSource gameOverSound;

    [Header("Button Sounds")]
    [SerializeField] private AudioSource btnClickSound;


    public static SoundController instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PlayBgMusic();
    }

    //играме музыку
    private void PlayBgMusic()
    {
        BgMusic.volume = GameSettings.instance.MusicVolume;
        BgMusic.Play();
    }

    //настройка звука мущыки
    public void ChangeMusicSound(float volume)
    {
        BgMusic.volume = volume;
    }

    //играем выбранный звук
    private void PlayCurrSound(AudioSource sound)
    {
        sound.volume = GameSettings.instance.SoundVolume;
        sound.Play();
    }

    public void PlayAddCoins() { PlayCurrSound(addCoins); }
    public void PlayEquipSound() { PlayCurrSound(equipSound); }
    public void PlayDailyRewardSound() {  PlayCurrSound(dailyRewardSound); }
    public void PlayGameOverSound() {  PlayCurrSound(gameOverSound); }
    public void PlayBtnClickSound() { PlayCurrSound(btnClickSound); }

    public void PlayKickSound()
    {
        int randSound = Random.Range(0, kickSounds.Length);
        PlayCurrSound(kickSounds[randSound]);
    }

    public void PlayGoalInGateSound()
    {
        int randSound = Random.Range(0, goalInGateSounds.Length);
        PlayCurrSound(goalInGateSounds[randSound]);
    }

    public void PlayGoalSound()
    {
        int randSound = Random.Range(0, goalSounds.Length);
        PlayCurrSound(goalSounds[randSound]);
    }

    public void PlayMetallGateSoundSound()
    {
        PlayCurrSound(metallGateSound);
    }
}

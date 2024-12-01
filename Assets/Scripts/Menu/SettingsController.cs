using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Button backBtn;

    [Header("Sound Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Header("Animation Settings")]
    [SerializeField] private Animation settingsAnim;
    [SerializeField] private AnimationClip settingsOn;
    [SerializeField] private AnimationClip settingsOff;

    [Header("Values")]
    private float musicVolume;
    private float soundVolume;

    [Header("View Settings")]
    [SerializeField] private GameObject settingsView;

    private GameSettings gameSettings;
    public static SettingsController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameSettings = GameSettings.instance;

        Load();

        backBtn.onClick.AddListener(Save);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
    }

    //��������� ����������� ��������
    private void Load()
    {
        musicVolume = gameSettings.MusicVolume;
        soundVolume = gameSettings.SoundVolume;

        UpdateSliders();
    }

    //��������� ��������
    private void Save()
    {
        gameSettings.MusicVolume = musicVolume;
        gameSettings.SoundVolume = soundVolume;

        SettingsOff();
    }

    //�������
    public void SettingsOn()
    {
        //�������� � ���������
        settingsAnim.Play(settingsOn.name);
    }

    //������� � ����
    private void SettingsOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(SettingsOffAnim());
    }

    //�������� ����� � ����
    private IEnumerator SettingsOffAnim()
    {
        settingsAnim.Play(settingsOff.name);
        yield return new WaitForSeconds(settingsOff.length);
        settingsView.SetActive(false);
    }

    //��������� ����������� ���������
    private void UpdateSliders()
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }

    //������ ��������� ������
    private void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        SoundController.instance.ChangeMusicSound(musicVolume);
    }

    //������ ��������� ������
    private void ChangeSoundVolume(float value)
    {
        soundVolume = value;
    }
}

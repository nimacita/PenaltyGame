using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private TMPro.TMP_Text coinTxt;

    [Header("Menu Settings")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button achiveBtn;
    [SerializeField] private Button weeklyBtn;

    [Header("Animation Settings")]
    [SerializeField] private Animation menuAnim;
    [SerializeField] private AnimationClip menuOn;
    [SerializeField] private AnimationClip menuOff;

    [Header("Views Settings")]
    [SerializeField] private GameObject menuView;
    [SerializeField] private ShopController shopController;
    [SerializeField] private GameObject settingsView;
    [SerializeField] private SettingsController settingsController;
    [SerializeField] private GameObject achiveView;
    [SerializeField] private AchiveController achiveController;
    [SerializeField] private GameObject weeklyView;
    [SerializeField] private WeeklyBonusController weeklyBonusController;

    [Header("Components")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private PlayerSkinController playerSkinController;

    public static MenuController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartViewSettings();
        ButtonSettings();

        UpdateCurrency();
    }

    //начальные настройки экранов
    private void StartViewSettings()
    {
        menuView.SetActive(true);
        settingsView.SetActive(false);
        achiveView.SetActive(false);
        weeklyView.SetActive(false);
    }

    //настройка кнопок
    private void ButtonSettings()
    {
        playBtn.onClick.AddListener(PlayClick);
        settingsBtn.onClick.AddListener(SettingsClick);
        shopBtn.onClick.AddListener(ShopClick);
        achiveBtn.onClick.AddListener(AchiveClick);
        weeklyBtn.onClick.AddListener(WeeklyClick);
    }

    //обновляем значения валюты
    public void UpdateCurrency()
    {
        coinTxt.text = $"{GameSettings.instance.Coins}";
    }

    //возращаемся в меню с анимацией
    public void MenuOn()
    {
        playerSkinController.DefineCurrentSkin();
        menuView.SetActive(true);
        StartCoroutine(MenuOnAnim());

        UpdateCurrency();
    }

    private IEnumerator MenuOnAnim()
    {
        menuAnim.Play(menuOn.name);
        yield return new WaitForSeconds(menuOn.length);
        ItteractBtn(true);
    }

    //выключаем меню
    private IEnumerator MenuOff()
    {
        //играем анимацию
        menuAnim.Play(menuOff.name);
        ItteractBtn(false);
        yield return new WaitForSeconds(menuOff.length);
        menuView.SetActive(false);
    }

    //нажатие на кнопку играть
    private void PlayClick()
    {
        StartCoroutine(openScene("GameScene"));
    }

    //нажатие на кнопку настроек
    private void SettingsClick()
    {
        StartCoroutine(MenuOff());
        settingsView.SetActive(true);
        settingsController.SettingsOn();
    }

    //нажатие на кнопку ачивок
    private void AchiveClick()
    {
        StartCoroutine(MenuOff());
        achiveView.SetActive(true);
        achiveController.AchiveOn();
    }

    //нажатие на кнопку ежедневных прихов
    private void WeeklyClick()
    {
        StartCoroutine(MenuOff());
        weeklyView.SetActive(true);
        weeklyBonusController.WeeklyOn();
    }

    //нажатие на кнопку магазина
    private void ShopClick()
    {
        StartCoroutine(MenuOff());
        shopController.ShopOn();
    }

    private void ItteractBtn(bool value)
    {
        playBtn.interactable = value;
        settingsBtn.interactable = value;
        shopBtn.interactable = value;
        achiveBtn.interactable = value;
        weeklyBtn.interactable = value;
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class WeeklyBonusController : MonoBehaviour
{

    private DateTime currentTime;

    [Header("Date Debug")]
    [SerializeField] private int lastClaimDay;
    [SerializeField] private bool canClaim;
    //[SerializeField] private TextMeshProUGUI timeToNextReward;
    [SerializeField] private TextMeshProUGUI currentClaimDay;


    [Space]
    [Header("Weekly Bonus")]
    [SerializeField] private int[] weeklyBonusValue;
 
    [Space]
    [Header("Streak")]
    [SerializeField] private int maxStreak;
    [SerializeField] private int currentStreak;
    [SerializeField] private int currentBonus;
    [SerializeField] private TMPro.TMP_Text streakTxt;

    [Space]
    [Header("View")]
    [SerializeField] private GameObject weeklyBonusView;
    [SerializeField] private GameObject anyWeeklyBonusView;
    [SerializeField] private GameObject claimBonusView;
    [SerializeField] private TextMeshProUGUI claimBonusValue;
    [SerializeField] private Image claimBonusCoin;
    [SerializeField] private Sprite[] coinSprites;
    public Button backBtn;
    public Button claimBtn;

    [Space]
    [Header("WeklyIcons")]
    [SerializeField]
    private GameObject[] weeklyIcons;
    private TextMeshProUGUI[] weeklyIconsValueTxt;
    private TextMeshProUGUI[] weeklyIconsDayTxt;
    private GameObject[] weeklyIconsLockedImage;
    private GameObject[] weeklyIconsOpenedImage;

    [Header("Animation Settings")]
    [SerializeField] private Animation weeklyAnim;
    [SerializeField] private AnimationClip weeklyOn;
    [SerializeField] private AnimationClip weeklyOff;

    [Space]
    [Header("Debug")]
    [SerializeField]
    private int addDays = 0;

    private bool dailySound = false;

    void Start()
    {
        //LastClaim = new DateTime();
        InitializedWeeklyIconsValueTxt();
        ButtonSettings();
        UpdateTime();
        CanClaimRewardUpdate();
        UpdateStreakTxt();
    }

    private void ButtonSettings()
    {
        backBtn.onClick.AddListener(WeeklyOff);
        claimBtn.onClick.AddListener(ClaimReward);
    }

    //последнйи забор наград
    public DateTime LastClaim
    {
        
        get
        {
            DateTime dateTime = new DateTime();
            if (!PlayerPrefs.HasKey("LastClaim"))
            {
                return dateTime;
            }
            else
            {
                return DateTime.Parse(PlayerPrefs.GetString("LastClaim"));
            }
        }
        set
        {
            PlayerPrefs.SetString("LastClaim", value.ToString());
        }
    }

    //сохраненный стрик подярд
    private int Streak
    {
        get
        {
            if (!PlayerPrefs.HasKey("Streak"))
            {
                PlayerPrefs.SetInt("Streak", 1);
            }
            else
            {
                if (PlayerPrefs.GetInt("Streak") < 1 || PlayerPrefs.GetInt("Streak") > maxStreak) 
                {
                    PlayerPrefs.SetInt("Streak", 1);
                }
            }
            return PlayerPrefs.GetInt("Streak");
        }

        set
        {
            if (value >= 1 )
            {
                if (value > maxStreak) 
                {
                    PlayerPrefs.SetInt("Streak", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("Streak", value);
                }
            }
        }
    }


    private void FixedUpdate()
    {
        currentStreak = Streak;
        currentBonus = weeklyBonusValue[Streak - 1];

        UpdateTime();
    }

    //обновляем проверку
    private void UpdateTime()
    {
        currentTime = DateTime.Now.AddDays(addDays);
        CanClaimRewardUpdate();
        UpdateStreakTxt();
    }

    private void UpdateStreakTxt()
    {
        streakTxt.text = $"{currentStreak - 1} days"; 
    }

    //можем ли забрать награду
   private void CanClaimRewardUpdate()
    {

        if (LastClaim.Year == currentTime.Year && LastClaim.Month == currentTime.Month)
        {
            if (LastClaim.AddDays(1).Day <= currentTime.Day 
                && LastClaim.AddDays(1).Month == currentTime.Month)
            {
                canClaim = true;

                if (LastClaim.AddDays(1).Day != currentTime.Day 
                    && LastClaim.AddDays(1).Month == currentTime.Month)
                {
                    Streak = 1;
                }
    
            }
            else
            {
                canClaim = false;
            }
        }
        else
        {
            canClaim = true;
            if (LastClaim.AddDays(1).Day != currentTime.Day)
            {
                Streak = 1;
            }
        }

        ViewControllerUpdate();
    }

    //обновляем экраны
    private void ViewControllerUpdate()
    {
        anyWeeklyBonusView.SetActive(true);
        claimBonusView.SetActive(false);

       
        if (canClaim)
        {//вью сбора бонуса
            ClaimBonusViewOpen();
        }
        else
        {//вью недельных бонусов
            claimBonusView.SetActive(false);
            anyWeeklyBonusView.SetActive(true);
            DefindWeeklyIconsLocked();
        }
    }

    //отображение экрана сбора награды
    private void ClaimBonusViewOpen()
    {
        claimBonusValue.text = "X" + weeklyBonusValue[Streak - 1].ToString();

        currentClaimDay.text = $"DAY {currentStreak}";
        claimBonusView.SetActive(true);

        claimBonusCoin.sprite = coinSprites[Streak - 1];
        //anyWeeklyBonusView.SetActive(true);

    }

    //задаем отоброжение компонентов
    private void InitializedWeeklyIconsValueTxt()
    {
        weeklyIconsValueTxt = new TextMeshProUGUI[weeklyIcons.Length];
        weeklyIconsOpenedImage = new GameObject[weeklyIcons.Length];
        weeklyIconsLockedImage = new GameObject[weeklyIcons.Length];
        weeklyIconsDayTxt = new TextMeshProUGUI[weeklyIcons.Length];

        //инициализация и определение внешнего вида всех эллементов ежедневного бонуса
        if (weeklyIcons.Length > 0)
        {
            for (int i = 0; i < weeklyIcons.Length; i++)
            {
                weeklyIconsOpenedImage[i] = weeklyIcons[i].transform.GetChild(1).gameObject;

                weeklyIconsValueTxt[i] = weeklyIcons[i].transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();


                weeklyIconsDayTxt[i] = weeklyIcons[i].transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                weeklyIconsLockedImage[i] = weeklyIcons[i].transform.GetChild(2).gameObject;

                weeklyIconsDayTxt[i].text = $"DAY {i + 1}";

                weeklyIconsValueTxt[i].text = weeklyBonusValue[i].ToString();
            }
        }

        DefindWeeklyIconsLocked();
    }

    //обновляем иконки наград если закрыты
    private void DefindWeeklyIconsLocked()
    {
        for (int i = 0; i < weeklyIconsLockedImage.Length; i++)
        {
            if (i < Streak - 1)
            {
                //открыта
                weeklyIconsLockedImage[i].SetActive(false);
                weeklyIconsOpenedImage[i].SetActive(true);
            }
            else
            {
                //закрыта
                weeklyIconsLockedImage[i].SetActive(true);
                weeklyIconsOpenedImage[i].SetActive(true);
            }
        }
    }

    //собираем награду
    public void ClaimReward()
    {
        if (canClaim)
        {
            SoundController.instance.PlayAddCoins();
            ClaimCurrentBonus();
            Streak += 1;
            LastClaim = currentTime;
            CanClaimRewardUpdate();
        }
    }

    private void ClaimCurrentBonus()
    {
        GameSettings.instance.Coins += weeklyBonusValue[Streak - 1];
    }

    //заходим
    public void WeeklyOn()
    {
        //анимация и появление
        weeklyAnim.Play(weeklyOn.name);
        dailySound = false;

        if (canClaim)
        {
            if (!dailySound)
            {
                SoundController.instance.PlayDailyRewardSound();
                dailySound = true;
            }
        }
        else
        {
            dailySound = false;
        }
    }

    //выходим в меню
    private void WeeklyOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(WeeklyOffAnim());
    }

    //анимация ухода в меню
    private IEnumerator WeeklyOffAnim()
    {
        weeklyAnim.Play(weeklyOff.name);
        yield return new WaitForSeconds(weeklyOff.length);
        weeklyBonusView.SetActive(false);
    }


}

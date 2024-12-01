using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [Header("Difficulty Settings")]
    [SerializeField] private Vector2 ballRotSpeedRange;
    private float currBallRotSpeed;
    [SerializeField] private Vector2 kickAngleSpeedRange;
    private float currKickAngleSpeed;
    //шаг прибавления к статам каждый гол
    private float addGoalKoef = 0.05f;
    private int defendersCountMin, defendersCountMax;
    [Tooltip("Количество голов, раз в которое увеличиввается доступное количетво защитников")]
    [SerializeField] private int goalsToAddDefenders;
    private int currDefendersMaxCount;
    private int currDefenders;

    [Header("Coins Settings")]
    [SerializeField] private int coinsToGoalKoef;

    [Header("Game View Settings")]
    public GameObject gameView;
    public GameObject forceView;
    public Button pauseBtn;
    public TMPro.TMP_Text goalCountTxt;

    [Header("Goal View Settings")]
    public GameObject goalView;
    public Animation goalViewAnim;
    public AnimationClip goalViewClip;

    [Header("Camera Animations")]
    public Animation cameraAnim;
    public AnimationClip cameraGoalClip;
    public AnimationClip cameraGameOverClip;

    [Header("Game Over View")]
    public GameObject gameOverView;
    public Animation gameOverAnim;
    public AnimationClip gameOverClip;
    public TMPro.TMP_Text gameOverGoalTxt;
    public TMPro.TMP_Text gameOverCoinTxt;
    public Button gameOverRestartBtn;
    public Button gameOverExitBtn;

    [Header("Pause View")]
    [SerializeField] private GameObject pauseView;
    [SerializeField] private Animation pauseAnim;
    [SerializeField] private AnimationClip pauseOnAnim;
    [SerializeField] private AnimationClip pauseOffAnim;
    [SerializeField] private Button pauseContinueBtn;
    [SerializeField] private Button pauseRestartBtn;
    [SerializeField] private Button pauseMenuBtn;


    [Header("Components")]
    public GameObject mainCamera;
    public GoalKeeperController keeperController;
    public DefendersController defendersController;

    //bool
    private bool isGame;
    private bool isGoal;
    private bool isPause;
    private bool isOver;

    private int goalCount = 0;

    //Events
    //событие включения-выключения паузы
    public static Action<bool> onPauseActived;
    public static Action onRestartedField;

    public static GameController instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        BallController.onGoaled += Goal;
        BallController.onStoped += GameOver;
    }

    private void OnDisable()
    {
        BallController.onGoaled -= Goal;
        BallController.onStoped -= GameOver;
    }

    void Start()
    {
        StartViewSettings();

        //задаем границы
        defendersCountMin = 1;
        defendersCountMax = defendersController.DefendersLength() + 1;
        //начальные настройки защитников
        currDefenders = defendersCountMin;
        currDefendersMaxCount = defendersCountMin;
        //начальные настройки скоростей
        currBallRotSpeed = ballRotSpeedRange.x;
        currKickAngleSpeed = kickAngleSpeedRange.x;
        UpdatePlayerSpeeds();

        //запускаем игру
        PlayerController.instance.StartGameSettings();

        //загружаем поле
        RestartField();
        StartSettings();

        ButtonSettings();
    }

    //настройка кнопок
    private void ButtonSettings()
    {
        gameOverExitBtn.onClick.AddListener(ExitToMenu);
        gameOverRestartBtn.onClick.AddListener(Restart);

        pauseBtn.onClick.AddListener(PauseOn);

        pauseContinueBtn.onClick.AddListener(PauseOff);
        pauseRestartBtn.onClick.AddListener(Restart);
        pauseMenuBtn.onClick.AddListener(ExitToMenu);
    }

    //начальные настройки экранов
    private void StartViewSettings()
    {
        UpdateGoalCount();
        gameView.SetActive(true);
        goalView.SetActive(false);
        gameOverView.SetActive(false);
        pauseView.SetActive(false);
    }

    //начальные настройки
    private void StartSettings()
    {
        isGoal = false;
        isPause = false;
        isGame = true;
        isOver = false;

        goalView.SetActive(false);
        forceView.SetActive(false);

    }

    //забили гол
    private void Goal()
    {
        if (!isGoal)
        {
            goalView.SetActive(true);
            isGoal = true;
            goalCount++;

            //звук
            SoundController.instance.PlayGoalSound();

            //добавляем сохраненный рекорд голов
            GameSettings.instance.CurrentGoals++;

            //обновляем рекорд
            UpdateGoalCount();
            //усложнаяем
            ChangeDifficulty();

            StartCoroutine(GoalAnim());
        }
    }

    //анимация гола
    private IEnumerator GoalAnim()
    {
        goalViewAnim.Play(goalViewClip.name);

        yield return new WaitForSeconds(goalViewClip.length /2f);
        //анимация проигралась

        cameraAnim.Play(cameraGoalClip.name);
        yield return new WaitForSeconds(cameraGoalClip.length);
        //анимация проигралась

        //запускаем после всех анимаций
        GoNext();
    }

    //усложнаяем в зависимости от гола
    private void ChangeDifficulty()
    {
        //обновляем скорость выбора
        if (currBallRotSpeed < ballRotSpeedRange.y)
        {
            currBallRotSpeed += addGoalKoef;
        }
        if (currKickAngleSpeed < kickAngleSpeedRange.y)
        {
            currKickAngleSpeed += addGoalKoef;
        }

        //обновляем количество защитников в зависимости от количество голов
        if (goalCount % goalsToAddDefenders == 0)
        {
            if (currDefendersMaxCount < defendersCountMax)
            {
                currDefendersMaxCount++;
            }
        }
        //рандомим количество защитников
        int currMinCount = currDefendersMaxCount / 2 + defendersCountMin;
        if (currMinCount < defendersCountMin) currMinCount = defendersCountMin;
        if (currMinCount > currDefendersMaxCount) currMinCount = currDefendersMaxCount - 1;
        int randDef = UnityEngine.Random.Range(currMinCount, currDefendersMaxCount + 1);
        currDefenders = randDef;

        //обновляем у игрока
        UpdatePlayerSpeeds();
    }

    //обновляем значения у игрока
    private void UpdatePlayerSpeeds()
    {
        PlayerController.instance.BallRotSpeed = currBallRotSpeed;
        PlayerController.instance.KickAngleSpeed = currKickAngleSpeed;
    }

    //перезагружаем поле, возвращаем мяч, спавним защитников и варатаря и т.д.
    public void RestartField()
    {
        //перезапускаем настройки игрока
        PlayerController.instance.RestartGameSettings();
        //ставим вратаря на новое место
        keeperController.SetNewKeeperPos();
        //ставим защитников
        defendersController.SetDefenders(currDefenders);

        onRestartedField?.Invoke();
    }

    //играем дальше
    private void GoNext()
    {
        StartSettings();
        PlayerController.instance.StartGameSettings();
    }

    //включаем - выключаем полоску силы удара
    public void SetForceView(bool value)
    {
        forceView.SetActive(value);
    }

    //обновляем счет голов
    private void UpdateGoalCount()
    {
        goalCountTxt.text = $"{goalCount}";
    }

    //конец игры
    private void GameOver()
    {
        if (!isGoal)
        {
            //отображаем экран конца игры
            gameOverGoalTxt.text = $"Goal: {goalCount}";

            //звук
            SoundController.instance.PlayGameOverSound();

            int currCoins = goalCount * coinsToGoalKoef;
            gameOverCoinTxt.text = $"{currCoins}";
            //сохраняем деньги
            GameSettings.instance.Coins += currCoins;

            isOver = true;

            gameOverView.SetActive(true);

            //анимация
            cameraAnim.Play(cameraGameOverClip.name);
            gameOverAnim.Play(gameOverClip.name);
        }
    }

    //включаем паузу
    private void PauseOn()
    {
        StartCoroutine("OnPauseAnim");
    }

    //анимация включения паузы
    private IEnumerator OnPauseAnim()
    {
        //вызываем событие
        onPauseActived?.Invoke(true);
        ItteractPauseBtn(false);
        pauseView.SetActive(true);
        pauseAnim.Play(pauseOnAnim.name);
        yield return new WaitForSeconds(pauseOnAnim.length);
        ItteractPauseBtn(true);
    }

    //выключаем паузу
    private void PauseOff()
    {
        StartCoroutine("OffPauseAnim");
    }

    //анимация выключения паузы
    private IEnumerator OffPauseAnim()
    {
        ItteractPauseBtn(false);
        pauseAnim.Play(pauseOffAnim.name);
        yield return new WaitForSeconds(pauseOffAnim.length);
        //вызываем событие
        onPauseActived?.Invoke(false);
        pauseView.SetActive(false);
    }

    private void ItteractPauseBtn(bool value)
    {
        pauseContinueBtn.interactable = value;
        pauseMenuBtn.interactable = value;
        pauseRestartBtn.interactable = value;
    }

    //перезапускаем уровень
    private void Restart()
    {
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //выходим в меню
    private void ExitToMenu()
    {
        //запускаем сцену меню
        StartCoroutine(openScene("MainMenu"));
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}

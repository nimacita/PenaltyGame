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
    //��� ����������� � ������ ������ ���
    private float addGoalKoef = 0.05f;
    private int defendersCountMin, defendersCountMax;
    [Tooltip("���������� �����, ��� � ������� �������������� ��������� ��������� ����������")]
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
    //������� ���������-���������� �����
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

        //������ �������
        defendersCountMin = 1;
        defendersCountMax = defendersController.DefendersLength() + 1;
        //��������� ��������� ����������
        currDefenders = defendersCountMin;
        currDefendersMaxCount = defendersCountMin;
        //��������� ��������� ���������
        currBallRotSpeed = ballRotSpeedRange.x;
        currKickAngleSpeed = kickAngleSpeedRange.x;
        UpdatePlayerSpeeds();

        //��������� ����
        PlayerController.instance.StartGameSettings();

        //��������� ����
        RestartField();
        StartSettings();

        ButtonSettings();
    }

    //��������� ������
    private void ButtonSettings()
    {
        gameOverExitBtn.onClick.AddListener(ExitToMenu);
        gameOverRestartBtn.onClick.AddListener(Restart);

        pauseBtn.onClick.AddListener(PauseOn);

        pauseContinueBtn.onClick.AddListener(PauseOff);
        pauseRestartBtn.onClick.AddListener(Restart);
        pauseMenuBtn.onClick.AddListener(ExitToMenu);
    }

    //��������� ��������� �������
    private void StartViewSettings()
    {
        UpdateGoalCount();
        gameView.SetActive(true);
        goalView.SetActive(false);
        gameOverView.SetActive(false);
        pauseView.SetActive(false);
    }

    //��������� ���������
    private void StartSettings()
    {
        isGoal = false;
        isPause = false;
        isGame = true;
        isOver = false;

        goalView.SetActive(false);
        forceView.SetActive(false);

    }

    //������ ���
    private void Goal()
    {
        if (!isGoal)
        {
            goalView.SetActive(true);
            isGoal = true;
            goalCount++;

            //����
            SoundController.instance.PlayGoalSound();

            //��������� ����������� ������ �����
            GameSettings.instance.CurrentGoals++;

            //��������� ������
            UpdateGoalCount();
            //����������
            ChangeDifficulty();

            StartCoroutine(GoalAnim());
        }
    }

    //�������� ����
    private IEnumerator GoalAnim()
    {
        goalViewAnim.Play(goalViewClip.name);

        yield return new WaitForSeconds(goalViewClip.length /2f);
        //�������� �����������

        cameraAnim.Play(cameraGoalClip.name);
        yield return new WaitForSeconds(cameraGoalClip.length);
        //�������� �����������

        //��������� ����� ���� ��������
        GoNext();
    }

    //���������� � ����������� �� ����
    private void ChangeDifficulty()
    {
        //��������� �������� ������
        if (currBallRotSpeed < ballRotSpeedRange.y)
        {
            currBallRotSpeed += addGoalKoef;
        }
        if (currKickAngleSpeed < kickAngleSpeedRange.y)
        {
            currKickAngleSpeed += addGoalKoef;
        }

        //��������� ���������� ���������� � ����������� �� ���������� �����
        if (goalCount % goalsToAddDefenders == 0)
        {
            if (currDefendersMaxCount < defendersCountMax)
            {
                currDefendersMaxCount++;
            }
        }
        //�������� ���������� ����������
        int currMinCount = currDefendersMaxCount / 2 + defendersCountMin;
        if (currMinCount < defendersCountMin) currMinCount = defendersCountMin;
        if (currMinCount > currDefendersMaxCount) currMinCount = currDefendersMaxCount - 1;
        int randDef = UnityEngine.Random.Range(currMinCount, currDefendersMaxCount + 1);
        currDefenders = randDef;

        //��������� � ������
        UpdatePlayerSpeeds();
    }

    //��������� �������� � ������
    private void UpdatePlayerSpeeds()
    {
        PlayerController.instance.BallRotSpeed = currBallRotSpeed;
        PlayerController.instance.KickAngleSpeed = currKickAngleSpeed;
    }

    //������������� ����, ���������� ���, ������� ���������� � �������� � �.�.
    public void RestartField()
    {
        //������������� ��������� ������
        PlayerController.instance.RestartGameSettings();
        //������ ������� �� ����� �����
        keeperController.SetNewKeeperPos();
        //������ ����������
        defendersController.SetDefenders(currDefenders);

        onRestartedField?.Invoke();
    }

    //������ ������
    private void GoNext()
    {
        StartSettings();
        PlayerController.instance.StartGameSettings();
    }

    //�������� - ��������� ������� ���� �����
    public void SetForceView(bool value)
    {
        forceView.SetActive(value);
    }

    //��������� ���� �����
    private void UpdateGoalCount()
    {
        goalCountTxt.text = $"{goalCount}";
    }

    //����� ����
    private void GameOver()
    {
        if (!isGoal)
        {
            //���������� ����� ����� ����
            gameOverGoalTxt.text = $"Goal: {goalCount}";

            //����
            SoundController.instance.PlayGameOverSound();

            int currCoins = goalCount * coinsToGoalKoef;
            gameOverCoinTxt.text = $"{currCoins}";
            //��������� ������
            GameSettings.instance.Coins += currCoins;

            isOver = true;

            gameOverView.SetActive(true);

            //��������
            cameraAnim.Play(cameraGameOverClip.name);
            gameOverAnim.Play(gameOverClip.name);
        }
    }

    //�������� �����
    private void PauseOn()
    {
        StartCoroutine("OnPauseAnim");
    }

    //�������� ��������� �����
    private IEnumerator OnPauseAnim()
    {
        //�������� �������
        onPauseActived?.Invoke(true);
        ItteractPauseBtn(false);
        pauseView.SetActive(true);
        pauseAnim.Play(pauseOnAnim.name);
        yield return new WaitForSeconds(pauseOnAnim.length);
        ItteractPauseBtn(true);
    }

    //��������� �����
    private void PauseOff()
    {
        StartCoroutine("OffPauseAnim");
    }

    //�������� ���������� �����
    private IEnumerator OffPauseAnim()
    {
        ItteractPauseBtn(false);
        pauseAnim.Play(pauseOffAnim.name);
        yield return new WaitForSeconds(pauseOffAnim.length);
        //�������� �������
        onPauseActived?.Invoke(false);
        pauseView.SetActive(false);
    }

    private void ItteractPauseBtn(bool value)
    {
        pauseContinueBtn.interactable = value;
        pauseMenuBtn.interactable = value;
        pauseRestartBtn.interactable = value;
    }

    //������������� �������
    private void Restart()
    {
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //������� � ����
    private void ExitToMenu()
    {
        //��������� ����� ����
        StartCoroutine(openScene("MainMenu"));
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}

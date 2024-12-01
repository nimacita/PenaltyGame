using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Main Settings")]

    [Header("Ball Rotate")]
    [SerializeField] private float ballRotAngle;
    [SerializeField]private float kickForce;
    private float ballRotSpeed;

    [Header("Kick Force Angle")]
    [SerializeField] private float minKickAngle = 0f;
    private float kickAngleSpeed;
    [Range(1, 35f)]
    [SerializeField] private float maxKickAngle = 35f;
    [SerializeField] private float currkickAngle;

    [Header("Animation Settings")]
    public Animator playerAnimator;
    private string[] danceClipsBools = 
        {"Dance1", "Dance2", "Dance3", "Dance4", "Dance5"};
    public Gradient forceSliderGrad;
    private float minRotation = -1f;
    private float maxRotation = 1f;
    private float minScale = 1f;
    private float maxScale = 1.1f;

    [Header("Components")]
    [SerializeField] private GameObject ball;
    [SerializeField] private BallController ballController;
    [SerializeField] private Button kickBtn;
    [SerializeField] private Slider forceSlider;
    [SerializeField] private RectTransform sliderRectTransform;
    [SerializeField] private Image sliderFill;
    private Quaternion startBallRot;
    private Quaternion targetBallRot;

    //bools
    private bool isChangeDir = false;
    private bool isChangeForce = false;
    private bool isKicked;
    private bool isPaused;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        BallController.onGoaled += GoalDancing;
        GameController.onPauseActived += IsPause;
        BallController.onStoped += PlayerLose;
    }

    private void OnDisable()
    {
        BallController.onGoaled -= GoalDancing;
        GameController.onPauseActived -= IsPause;
        BallController.onStoped -= PlayerLose;
    }

    void Start()
    {
        kickBtn.onClick.AddListener(KickBtnClick);

        StartBallSettings();
        StartSettings();
        //StartGameSettings();
    }

    private void StartSettings()
    {
        //SetBallArrow(false);
        GameController.instance.SetForceView(false);
    }

    //��������� ��������� ����
    public void StartGameSettings()
    {
        isChangeDir = true;
        isChangeForce = false;
        isPaused = false;
        isKicked = false;

        SetBallArrow(true);
    }

    public void RestartGameSettings()
    {
        ballController.StartSettings();
        ball.SetActive(true);
        GameController.instance.SetForceView(false);

        //������������� ��� � ���� �������� ��������
        forceSlider.minValue = minKickAngle;
        forceSlider.maxValue = maxKickAngle;

        //�������� ����
        DisableAllDance();
    }

    //��������� ��������� ����
    private void StartBallSettings()
    {
        //������������� ��������� ���� ��������
        startBallRot = Quaternion.Euler(0f, -ballRotAngle, 0f);
        //������������� ������� ���� ��������
        targetBallRot = Quaternion.Euler(0f, ballRotAngle, 0f);
    }
    
    void FixedUpdate()
    {
        RotateBallDir();
        SelectKickForce();
    }

    //�������� ����������� ����
    private void RotateBallDir()
    {
        if (isChangeDir)
        {
            float t = Mathf.PingPong(Time.time * ballRotSpeed, 1f);

            ball.transform.rotation = Quaternion.Lerp(startBallRot, targetBallRot, t);
        }
    }

    //�������� ���� ����� �����
    private void SelectKickForce()
    {
        if (isChangeForce && !isPaused)
        {
            float t = Mathf.PingPong(Time.time * kickAngleSpeed, 1);
            forceSlider.value = Mathf.Lerp(minKickAngle, maxKickAngle, t);

            float normalizedValue = (forceSlider.value - minKickAngle) / (maxKickAngle - minKickAngle);

            // �������� ���� �� ��������� � ��������� ��� � ���������� Image
            Color newColor = forceSliderGrad.Evaluate(normalizedValue);
            sliderFill.color = newColor;

            // �������� ������� � ������� ��������
            float newRotation = Mathf.Lerp(minRotation, maxRotation, normalizedValue);
            float newScale = Mathf.Lerp(minScale, maxScale, normalizedValue);

            // ��������� ������� � ������� � RectTransform ��������
            sliderRectTransform.localRotation = Quaternion.Euler(0, 0, newRotation);
            sliderRectTransform.localScale = new Vector3(newScale, newScale, 1);
        }
    }

    //������������� ����� �������� ����
    public void SetRot(bool value)
    {
        isChangeDir = value;
    }

    //��������-��������� ������� � ����
    public void SetBallArrow(bool value)
    {
        ballController.ArrowActive(value);
    }

    //������ ���
    private void BallKick()
    {
        if (!isKicked)
        {
            isChangeDir = false;
            isKicked = true;
            ballController.ArrowActive(false);

            playerAnimator.SetTrigger("Kick");
        }
    }

    //�����5��� ��� ����� ��������
    public void Kicked()
    {
        SoundController.instance.PlayKickSound();
        //��������� ���
        ballController.Kicked(kickForce, currkickAngle);
    }

    //���
    private void GoalDancing()
    {
        if (isKicked)
        {
            int randDance = Random.Range(0, danceClipsBools.Length);
            playerAnimator.SetBool(danceClipsBools[randDance], true);
        }
    }

    //��������� ��� �����
    private void DisableAllDance()
    {
        for (int i = 0; i < danceClipsBools.Length; i++) 
        {
            playerAnimator.SetBool(danceClipsBools[i], false);
        }
    }

    //���������
    private void PlayerLose()
    {
        playerAnimator.SetTrigger("Lose");
    }

    //������ �� ������ �����
    private void KickBtnClick()
    {
        if (!isPaused)
        {
            if (isChangeDir)
            {
                SetRot(false);

                forceSlider.value = minKickAngle;
                GameController.instance.SetForceView(true);
                isChangeForce = true;
            }
            else if (isChangeForce)
            {
                isChangeForce = false;
                currkickAngle = -forceSlider.value;

                GameController.instance.SetForceView(false);

                //������
                BallKick();
            }
        }
    }

    private void IsPause(bool value)
    {
        isPaused = value;
    }

    public float BallRotSpeed
    {
        get { return ballRotSpeed; }
        set { ballRotSpeed = value; }
    }

    public float KickAngleSpeed
    {
        get { return kickAngleSpeed; }
        set { kickAngleSpeed = value; }
    }
}

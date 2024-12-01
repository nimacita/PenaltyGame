using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [Header("Main Settings")]
    private float kickForce;
    private float kickAngle;
    private Rigidbody rb;
    private Vector3 startBallPos;
    private Vector3 ballDir;

    [Header("Components")]
    public GameObject arrow;
    public Animation arrowAnim;
    private float minVelocity = 0.05f;

    //bool
    [SerializeField]
    private bool isKicked;
    private bool isDir = true;
    private bool isGoal;
    private bool isPause = false;
    private bool isStop = false;
    private bool isMetall = false;

    //������ ���
    public static Action onGoaled;
    //��� �����������
    public static Action onStoped;

    void Start()
    {
        startBallPos = transform.position;
        rb = GetComponent<Rigidbody>();
        ballDir = new Vector3(0f, 0f, 1f);
    }

    public void StartSettings()
    {
        isKicked = false;
        isGoal = false;
        isPause = false;
        isStop = false;
        isDir = true;
        isMetall = false;

        //���� ���� ��������� �������, �� ���������� �� ����� ���
        if (startBallPos != Vector3.zero)
        {
            transform.position = startBallPos;
            rb.rotation = Quaternion.Euler(0f, 0f, 0f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            startBallPos = transform.position;
        }
    }

    public void ArrowActive(bool value)
    {
        arrow.SetActive(value);
        if (value)
        {
            arrowAnim.Play();
        }
    }

    private void FixedUpdate()
    {
    }

    //��������� �������� ��� � ������������ �����
    private IEnumerator CheckVelocity() 
    {
        while (!isDir && !isStop && !isGoal)
        {
            yield return new WaitForSeconds(1f);
            if (rb.velocity.z < minVelocity && rb.velocity.x < minVelocity && rb.velocity.y < minVelocity)
            {
                if (!isDir && !isStop && !isGoal)
                {
                    onStoped?.Invoke();
                    isStop = true;
                }
            }
        }
    }

    //��������� ��� �����
    private void KickStreight()
    {
        if (isKicked && !isPause && !isGoal)
        {
            rb.AddForce(transform.forward * kickForce, ForceMode.Impulse);
            //rb.velocity = ballDir * kickForce;
            isKicked = false;
        }
    }

    //��������� ��� �����
    private void KickInArc()
    {
        if (isKicked && !isPause && !isGoal)
        {
            // ������������ ����������� ������� �� ������ �������� �������� ������� � ����
            Vector3 launchDirection = Quaternion.Euler(kickAngle, transform.eulerAngles.y, 0) * Vector3.forward;
            // ��������� ���� �������� � ����������� ����
            rb.AddForce(launchDirection * kickForce, ForceMode.Impulse);
            isKicked = false;
            isDir = false;

            StartCoroutine(CheckVelocity());
        }
    }

    //����� ���
    public void Kicked(float force,float angle)
    {
        if (!isKicked)
        {
            kickForce = force;
            kickAngle = angle;
            isKicked = true;

            //KickStreight();
            KickInArc();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gate")
        {
            //������ � ������
            if (!isGoal && !isPause)
            {
                SoundController.instance.PlayGoalInGateSound();
                isGoal = true;
                isKicked = false;
                onGoaled?.Invoke();
                //gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Metall")
        {
            //������ � ������
            if (!isMetall)
            {
                SoundController.instance.PlayMetallGateSoundSound();
                isMetall = true;
            }
        }
    }
}

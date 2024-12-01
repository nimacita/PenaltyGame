using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchiveController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Button backBtn;
    [SerializeField] private TMPro.TMP_Text currentGoals;

    [Header("Animation Settings")]
    [SerializeField] private Animation achiveAnim;
    [SerializeField] private AnimationClip achiveOn;
    [SerializeField] private AnimationClip achiveOff;

    [Header("View Settings")]
    [SerializeField] private GameObject achiveView;


    void Start()
    {
        backBtn.onClick.AddListener(AchiveOff);
    }

    //�������
    public void AchiveOn()
    {
        //�������� � ���������
        achiveAnim.Play(achiveOn.name);
        UpdateGoals();
    }

    private void UpdateGoals()
    {
        currentGoals.text = $"{GameSettings.instance.CurrentGoals}";
    }

    //������� � ����
    private void AchiveOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(AchiveOffAnim());
    }

    //�������� ����� � ����
    private IEnumerator AchiveOffAnim()
    {
        achiveAnim.Play(achiveOff.name);
        yield return new WaitForSeconds(achiveOff.length);
        achiveView.SetActive(false);
    }
}

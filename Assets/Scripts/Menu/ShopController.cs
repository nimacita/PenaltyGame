using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private TMPro.TMP_Text coinTxt;

    [Header("Shop View")]
    [SerializeField] private GameObject shopView;
    [SerializeField] private Button shopBackBtn;

    [Header("Animations Settings")]
    [SerializeField] private Animation shopAnim;
    [SerializeField] private AnimationClip shopOn;
    [SerializeField] private AnimationClip shopOff;

    public static ShopController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartViewSettings();
        UpdateCurrency();
        ButtonSettings();
    }

    //��������� ��������� �������
    private void StartViewSettings()
    {
        shopView.SetActive(false);
    }

    //��������� ������
    private void ButtonSettings()
    {
        shopBackBtn.onClick.AddListener(ShopOff);
    }

    //�������� ����� ��������
    public void ShopOn()
    {
        shopView.SetActive(true);
        UpdateCurrency();
        StartCoroutine(ShopOnAnim());
    }

    private IEnumerator ShopOnAnim()
    {
        shopAnim.Play(shopOn.name);
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopOn.length);
        ItteractAllBtns(true);
    }

    //��������� ����� �������� 
    private void ShopOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(ShopOffAnim());
    }

    private void ItteractAllBtns(bool value)
    {
        shopBackBtn.interactable = value;
    }

    //��������� ����� �������� ��������
    private IEnumerator ShopOffAnim()
    {
        shopAnim.Play(shopOff.name);
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopOff.length);
        ItteractAllBtns(true);
        shopView.SetActive(false);
    }

    //��������� �������� ������
    public void UpdateCurrency()
    {
        coinTxt.text = $"{GameSettings.instance.Coins}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{

    enum SkinItem
    {
        hairSkin,
        glassSkin,
        backpackSkin
    }

    [Header("Item Settings")]
    [SerializeField] SkinItem skinItem;
    [Tooltip("���� ����� ��� ����������")]
    [Range(1, 3)]
    [SerializeField] private int currentSkinId;

    [Header("Coin Item")]
    [SerializeField] private int coinPrice;

    [Header("To Item Image")]
    public Sprite itemIconSprite;
    [SerializeField] private string skinName;

    [Space]
    [Header("Components")]
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private TMPro.TMP_Text itemName;
    [SerializeField] private GameObject shopBtn;
    [SerializeField] private TMPro.TMP_Text shopTxt;
    [SerializeField] private TMPro.TMP_Text coinTxt;
    [SerializeField] private GameObject coinBg;
    [SerializeField] private GameObject equiped;
    [SerializeField] private ShopController shopController;

    void Start()
    {
        itemIcon.GetComponent<Image>().sprite = itemIconSprite;
        coinTxt.text = $"{coinPrice}";
        shopBtn.GetComponent<Button>().interactable = true;
        shopBtn.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //����������� �������� ������ �� �����
    private bool IsSkinItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsSkinItemPurchased{currentSkinId}{skinItem}"))
            {
                if (PlayerPrefs.GetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}", 0);
                return false;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}", 0);
            }
        }
    }

    //���������� ��� ������
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);
        itemName.text = skinName;

        //��������� ������� ��
        if (!IsSkinItemPurchased)
        {
            //���� �� ������
            shopTxt.text = "Buy";
            equiped.SetActive(false);
            coinBg.SetActive(true);
            //��������� ������� �� �����
            if (GameSettings.instance.Coins < coinPrice)
            {
                shopBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                shopBtn.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            //���� ������
            equiped.SetActive(IsEquiped());
            coinBg.SetActive(false);
            shopTxt.text = "Select";
        }

    }

    //��������� ���� ���� ����������
    private bool IsEquiped()
    {
        bool isEquiped = false;

        switch (skinItem)
        {
            case SkinItem.hairSkin:
                {
                    if (GameSettings.instance.HairSkinId == currentSkinId)
                    {
                        isEquiped = true;
                    }
                    break;
                }
            case SkinItem.glassSkin:
                {
                    if (GameSettings.instance.GlassSkinId == currentSkinId)
                    {
                        isEquiped = true;
                    }
                    break;
                }
            case SkinItem.backpackSkin:
                {
                    if (GameSettings.instance.BackpackSkinId == currentSkinId)
                    {
                        isEquiped = true;
                    }
                    break;
                }
        }
        return isEquiped;
    }

    //����� �� ������, ������� ������ ������
    private void CanClaim()
    {
        if (!IsSkinItemPurchased)
        {
            //���� �� ������� - ��������
            if (GameSettings.instance.Coins < coinPrice)
            {
                //�� ����� ������

            }
            else
            {
                //����� ������
                GameSettings.instance.Coins -= coinPrice;
                SoundController.instance.PlayAddCoins();
                IsSkinItemPurchased = true;
                if (shopController != null) shopController.UpdateCurrency();
            }
        }
        else
        {
            //����� �� ������� - ���������
            EquipedSelectProduct();
        }
    }

    //��������� ��� �������
    private void EquipedSelectProduct()
    {
        switch (skinItem)
        {
            case SkinItem.hairSkin:
                {
                    if (GameSettings.instance.HairSkinId == currentSkinId)
                    {
                        GameSettings.instance.HairSkinId = 0;
                    }
                    else
                    {
                        //���������
                        SoundController.instance.PlayEquipSound();
                        GameSettings.instance.HairSkinId = currentSkinId;
                    }
                    break;
                }
            case SkinItem.glassSkin:
                {
                    if (GameSettings.instance.GlassSkinId == currentSkinId)
                    {
                        GameSettings.instance.GlassSkinId = 0;
                    }
                    else
                    {
                        //���������
                        SoundController.instance.PlayEquipSound();
                        GameSettings.instance.GlassSkinId = currentSkinId;
                    }
                    break;
                }
            case SkinItem.backpackSkin:
                {
                    if (GameSettings.instance.BackpackSkinId == currentSkinId)
                    {
                        GameSettings.instance.BackpackSkinId = 0;
                    }
                    else
                    {
                        //���������
                        SoundController.instance.PlayEquipSound();
                        GameSettings.instance.BackpackSkinId = currentSkinId;
                    }
                    break;
                }
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
